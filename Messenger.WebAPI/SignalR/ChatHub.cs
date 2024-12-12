using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.LastSeen;
using Mesagger.BLL.DTO.PersonalChat;
using Mesagger.BLL.DTO.PersonalChatMessageDTO;
using Mesagger.BLL.DTO.Profile;
using Mesagger.BLL.MediatR.Connection.GetPersonByConnection;
using Mesagger.BLL.MediatR.PersonalChat.GetAllForUser;
using Mesagger.BLL.MediatR.PersonalMessage.Create;
using Mesagger.BLL.MediatR.Profile.UpdateConnection;
using Mesagger.BLL.MediatR.Profile.UpdateLastSeenOnConnect;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.SignalR;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    
    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        int profileId = int.Parse(httpContext.Request.Query["userId"]);
        
        //need to change the status to online 
        await _mediator.Send(new UpdateProfileLastSeenCommand(new LastSeenDto()
        {
            ProfileId = profileId,
            IsOnline = true,
        }));
        
        // need to store the connectionId + TO DO add validation for the profileId
        // need to track the userId for each connection so latter i will be able to map from the id of profile to the connection id 
        await _mediator.Send(new CreateProfileConnectionCommand(new ProfileUpdateConnectionDto()
        {
            ProfileId = profileId,
            ConnectionId = Context.ConnectionId
        }));

        // need to get all the groups/chats and add user to them
        var userChats = await _mediator.Send(new GetAllPersonalChatsForUserQuery(profileId));
        
        foreach (var userChat in userChats.Value)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userChat.ChatId.ToString());
        }
        
        // need to send all the groups/chats to user ? 
        // need to think what i need when i initially connected to the server 
        // need to rename to LoadChats
        await Clients.Caller.SendAsync("LoadComments", userChats.Value);
      
    }

    public async Task SendMessage(PersonalMessageSendDto message)
    {
        // need to store the message
        var savedMessage = await _mediator.Send(new CreatePersonalMessageCommand(message));
        
        // need to send the message to group 
        if (savedMessage.IsSuccess)
        {
            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", savedMessage.Value);
        }
        
        // need to send the some responce to the sender(?)
        
        
    }
    public async Task SubscribeToLastSeenUpdate(int profileId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Profile{profileId}");
    }
    
    public async Task UnsubscribeToLastSeenUpdate(int profileId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Profile{profileId}");
    }
    
    public async Task SendLastSeenUpdate(LastSeenDto updatedLastSeenDto)
    {
        await Clients.Group($"Profile{updatedLastSeenDto.ProfileId}").SendAsync("ReceiveLastSeenUpdate", updatedLastSeenDto);
    }
    

    // public async Task DeleteMessage(int messageId)
    // {
    //     int deletedMessageId;
    //     await Clients.All.SendAsync("ReceiveDeletedMessage", deletedMessageId);
    // }
    //
    // public async Task EditMessage(PersonalMessageEditDto message)
    // {
    //     //some logic
    //     await Clients.All.SendAsync("ReceiveEditedMessage", message);
    // }
    
    public async Task OnDisconnectedAsync(Exception exception)
    {
        // need to get user from the connection id
        var profileId = await _mediator.Send(new GetPersonIdByConnectionCommand(Context.ConnectionId));

        // need to change the status to not online 
        // need to send this change to those who listenng (?)
        var lastSeen = new LastSeenDto()
        {
            ProfileId = profileId.Value,
            IsOnline = false,
            LastSeen = DateTime.Now
        };
        await _mediator.Send(new UpdateProfileLastSeenCommand(lastSeen));

        await SendLastSeenUpdate(lastSeen);
        
        Result<IEnumerable<PersonalChatDto>> userChats = new ();
        if (profileId.IsSuccess)
        {
            // need to get all the groups/chats and add user to them
            userChats = await _mediator.Send(new GetAllPersonalChatsForUserQuery(profileId.Value));
        }
        
        foreach (var userChat in userChats.Value)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userChat.ChatId.ToString());
        }
        
    }
    
    // need to add all operations that is connected with groups/chats creating/deleting 
}