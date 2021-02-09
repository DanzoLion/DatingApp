using System;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR                                       // via websockets // we need to use a query string of SignalR
{
    [Authorize]
    public class PresenceHub : Hub                       // virtual methods found within Hub can be overridden
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            //await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);                            
            var isOnline = await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);      // adjusted with optimisation refactor
            
            if (isOnline)                                                                                                                           // optimisation addition
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

            var currentUsers = await _tracker.GetOnlineUsers();                                                         // returns the update user when the user is online
         //   await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);                                      // optimisation
        }

        public override async Task OnDisconnectedAsync(Exception exception)                                 // this function/method needs to be the same as it is in parent class
        {
          //  await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            var isOffline =  await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);                 // optimisation adjustment
            
            if (isOffline)
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());          // optimisation adjustment

       //     var currentUsers = await _tracker.GetOnlineUsers();                                                  // optimisation
         //   await Clients.All.SendAsync("GetOnlineUsers", currentUsers);                                   // optimisation

            await base.OnDisconnectedAsync(exception);
        }
    }
}