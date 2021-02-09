using System.Security.AccessControl;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;           // we now have access of _presencehub within message hub
        private readonly PresenceTracker _tracker;

        public MessageHub(IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository, IHubContext<PresenceHub> presenceHub, PresenceTracker tracker)
        {
            _tracker = tracker;
            _presenceHub = presenceHub;
            _userRepository = userRepository;
            _mapper = mapper;
            _messageRepository = messageRepository;
        }
        public override async Task OnConnectedAsync()                                     // we create a route for each user and define a route name // we use a combo of username and username in alphabetical order
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            // await AddToGroup(Context, groupName);                      
            var group = await AddToGroup(groupName);                                     // added with Optimising Messages implementation
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);        // added with Optimising Messages implementation
            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
           // await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);                  // added with Optimising Messages implementation
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
           // await RemoveFromMessageGroup(Context.ConnectionId);                   // added with Optimising Messages implementation
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);                                                           // when disconnected the user is automatically removed from the new group
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)          // this code block copied over from MessagesController.cs
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);        // sender   // next we get hold of users in sender and recipient // we need to populate the message when we create it and go the other way when we return a DTO from this
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("Not Found User");
            var message = new Message                                                             // at this stage we know we are redy to create a new message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _messageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);

                if(connections != null)
                {                                                          // if the user is online and not connected to the same group ¬
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new {username = sender.UserName, knownAs = sender.KnownAs});     // { } represents an anonymous object
                }
            }
                _messageRepository.AddMessage(message);              // method that adds our message

            if (await _messageRepository.SaveAllAsync())
            {
                // var group = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));   // maps from the message we have created // we receive a message from our client
                                                                                                            // we need to update our message hub thread variable to update when a message is received
                // return BadRequest("Failed to send message");              // not required as we are not returning from this
            }
        }
        // private async Task<bool> AddToGroup(HubCallerContext context, string groupName)         // improvement refactor
        private async Task<Entities.Group> AddToGroup(string groupName)             // message will be sent back to group so members will always know who is inside their group
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Entities.Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);
           // return await _messageRepository.SaveAllAsync();                               // group refactoring
            if(await _messageRepository.SaveAllAsync()) return group;

            throw new HubException("Failed to join group");
        }
        //private async Task RemoveFromMessageGroup(string connectionId)               // improvement refactor
        private async Task<Entities.Group> RemoveFromMessageGroup()    // we need the group then we need to get the connection from inside the group
        {
            //var connection = await _messageRepository.GetConnection(Context.ConnectionId);       // we use Context.ConnectionId as we are still inside the hub // we will need to get the group for this conneciton
            var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);       // we use Context.ConnectionId as we are still inside the hub // we will need to get the group for this conneciton
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);   // added with Optimising Messages implementation
            _messageRepository.RemoveConnection(connection);
           // await _messageRepository.SaveAllAsync();                                           
           if (await _messageRepository.SaveAllAsync()) return group;                     // added with Optimising Messages implementation

           throw new HubException("Failed to remove from group");                        // added with Optimising Messages implementation
        }
        private string GetGroupName(string caller, string other)             // ensure alphabetical order
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}