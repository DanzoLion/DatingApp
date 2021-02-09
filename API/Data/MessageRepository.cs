using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)    // added IMapper
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)        // implemented from interface IMesssageRepository.cs // returns a group here
        {
            return await _context.Groups
            .Include(c => c.Connections)              // our related entity
            .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
            .Include(x => x.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName);        // these are simple queries to get/retrieve our group name
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)   // added MessageParms messageParams here
        {
            // we want to create an IQueryable here
            var query = _context.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false),            // this serves as our inbox // if we are recipient and read it this is what goes back from here
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username && u.SenderDeleted == false),            // this serves as our outbox // if we are recipient and read it this is what goes back from here
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)            // this serves as our default option within the switch statement
            };

            // from here we want to project and return DTOs from this here // we need to bring IMapper into this repository also
            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);     // we projectTo Message Dto

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)           // here we get the message thread between two users, ie the conversation
        {
            var messages = await _context.Messages                                                                                                                              // first we get the conversation between users
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)       // displays users photo in the message design also
            .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false && m.Sender.UserName == recipientUsername || m.Recipient.UserName == recipientUsername && m.Sender.UserName == currentUsername && m.SenderDeleted == false)
            .OrderBy(m => m.MessageSent).ToListAsync();

        var unreadMessages = messages.Where(m => m.DateRead == null && m.Recipient.UserName == currentUsername).ToList();  // we end up looping over this object as a list // get any unread messages

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
               // message.DateRead = DateTime.Now;
                message.DateRead = DateTime.UtcNow;       // date standardisation 
            }

            await _context.SaveChangesAsync();
        }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);    // here we return the message DTO as a list // returned as IEnumerable of MessageDto, and pass in messages
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;   // > 0 so that we can return a boolean value from this
        }
    }
}