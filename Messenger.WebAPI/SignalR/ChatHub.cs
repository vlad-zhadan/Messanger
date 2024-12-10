using Mesagger.BLL.DTO.PersonalChatMessageDTO;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.SignalR;

public class ChatHub : Hub
{
    // private readonly IMediator _mediator;
    //
    // public ChatHub(IMediator mediator)
    // {
    //     _mediator = mediator;
    // }
    
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var activityId = httpContext.Request.Query["messageId"];
        
        await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
        
        var result = "You are now logged in " + activityId;
        await Clients.Caller.SendAsync("LoadComments", result);
        // need to track the userId for each connection so latter i will be able to map from the id of profile to the connection id 
    }

    public async Task SendMessage(SendPersonalMessageDto message)
    {
        // var messageResult = await _mediator.Send(new SendPersonalMessageCommand(message));
        // await Clients.All.SendAsync("ReceiveMessage", messageResult);

        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    // public async Task DeleteMessage(int messageId)
    // {
    //     int deletedMessageId;
    //     await Clients.All.SendAsync("ReceiveDeletedMessage", deletedMessageId);
    // }
    //
    // public async Task EditMessage(EditPersonalMessageDto message)
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