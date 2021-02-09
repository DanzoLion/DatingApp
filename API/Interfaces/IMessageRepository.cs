using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);                                             // these methods allows us to manage connections to our group
        void RemoveConnection(Connection connection);           // these methods allows us to manage connections to our group
        Task<Connection> GetConnection(string connectionId);    // these methods allows us to manage connections to our group
        Task<Group> GetMessageGroup(string groupName);         // these methods allows us to manage connections to our group

        Task<Group> GetGroupForConnection(string connectionId);      // required for group messaging identification of members
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams message);
        //Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int RecipientId);        // altered to return the string instead of id
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);   // we take the recipeintUsername as parameter in the controller // always have context of currentUsername via context of controller 
        Task<bool> SaveAllAsync();
        
    }
}