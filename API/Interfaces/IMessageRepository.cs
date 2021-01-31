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
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams message);
        //Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int RecipientId);        // altered to return the string instead of id
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);   // we take the recipeintUsername as parameter in the controller // always have context of currentUsername via context of controller 
        Task<bool> SaveAllAsync();
        
    }
}