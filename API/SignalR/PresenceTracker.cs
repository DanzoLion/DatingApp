using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace API.SignalR
{
    public class PresenceTracker
    {
        static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();   // Dictionary has key/value pairs, with string username as Key/ List<string> value connection ID as string
        // our dictionary is shared with everyone who is connected to our server

       // public Task UserConnected(string username, string connectionId)
        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;                                                                                      // optimisation   // false, set as initial bool setting
            lock (OnlineUsers)                                         // the approach is to lock the user until we finish using the dictionary
            {
                if (OnlineUsers.ContainsKey(username))          // check for users key
                {
                    OnlineUsers[username].Add(connectionId); // add  List of new connection Id
                }

                else
                {
                    OnlineUsers.Add(username, new List<string>{connectionId});     // else no user create new list and add connection Id
                    isOnline = true;                                                                            // optimisation
                }
            }

           // return Task.CompletedTask;                                   // outside of the lock we return a completed Task     // this takes care of when a user is connected // removed with optimisation
            return Task.FromResult(isOnline);                                   // outside of the lock we return a completed Task     // this takes care of when a user is connected
        }

       // public Task UserDisconnected(string username, string connectionId)
        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock(OnlineUsers)
            {
               // if (!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;
                if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);        // optimisation refactor

                OnlineUsers[username].Remove(connectionId);
                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                    isOffline = true;
                }
            }

            //return Task.CompletedTask;
            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()                    // this is our method to get the users currently online  // we return an array of users
        {
            string[] onlineUsers;
            lock(OnlineUsers)                                                   // this is all stored in memory and not on database
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock(OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}