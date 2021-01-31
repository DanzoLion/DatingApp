using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// this is logic for creation of a messgae, saving it to database, and returning message DTO to the user

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository message, IMapper mapper)
        {
            _mapper = mapper;
            _messageRepository = message;
            _userRepository = userRepository;
        }

        [HttpPost]                                                                                                                                    // used to create a message
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);        // sender   // next we get hold of users in sender and recipient // we need to populate the message when we create it and go the other way when we return a DTO from this
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message// at this stage we know we are redy to create a new message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);              // method that adds our message

            if (await  _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));     // maps from the message we have created
            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams) // we are adding paging onto our header here 
{
    messageParams.Username = User.GetUsername();
    var messages = await _messageRepository.GetMessagesForUser(messageParams);
    Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
    return messages;
}

    [HttpGet("thread/{username}")]    // here we get our conversation/messages thread between two users // {username} is the other user, ie root parameter // we always have acces to currentUserName inside our controllers

    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
    {
        var currentUsername = User.GetUsername();
        return Ok(await _messageRepository.GetMessageThread(currentUsername, username));
    }

    [HttpDelete("{id}")]    // id of the message we want to delete
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUsername();
        var message = await _messageRepository.GetMessage(id);

        if (message.Sender.UserName != username && message.Recipient.UserName != username) 
        return Unauthorized(); // if sender username or recipient username != username then return unauthorised as this message has nothing to do with that user

        if (message.Sender.UserName == username) message.SenderDeleted = true;

        if (message.Recipient.UserName == username) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted) _messageRepository.DeleteMessage(message);

        if (await _messageRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting the message");    // if all else fails return this BadRequest message

    }

    }
}