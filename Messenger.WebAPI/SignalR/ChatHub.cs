using MediatR;
using Mesagger.BLL.DTO.PersonalChatMessageDTO;
using Mesagger.BLL.DTO.Profile;
using Mesagger.BLL.MediatR.PersonalChat.GetAllForUser;
using Mesagger.BLL.MediatR.PersonalMessage.Create;
using Mesagger.BLL.MediatR.Profile.UpdateConnection;
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
        
        // need to store the connectionId + TO DO add validation for the profileId
        // need to track the userId for each connection so latter i will be able to map from the id of profile to the connection id 
        await _mediator.Send(new UpdateProfileConnectionCommand(new ProfileUpdateConnectionDto()
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
        var chatRoomId = "chatRoom123";  // Example: dynamic value based on the user's session or preferences
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        Console.WriteLine($"User {Context.ConnectionId} disconnected and removed from group {chatRoomId}");
    }
}