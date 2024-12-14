using FluentResults;
using MediatR;
using Mesagger.BLL.MediatR.Connection.Get;
using Mesagger.BLL.MediatR.Message.Delete;
using Mesagger.BLL.MediatR.Message.Edit;
using Mesagger.BLL.MediatR.PersonalChat.Block;
using Messenger.BLL.DTO.Chat;
using Messenger.BLL.DTO.LastSeen;
using Messenger.BLL.DTO.MessageReceiver;
using Messenger.BLL.DTO.PersonalChat;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.BLL.DTO.Profile;
using Messenger.BLL.MediatR.Connection.GetPersonByConnection;
using Messenger.BLL.MediatR.Message.GetById;
using Messenger.BLL.MediatR.MessageReceiver.Create;
using Messenger.BLL.MediatR.MessageReceiver.Get;
using Messenger.BLL.MediatR.PersonalChat.GetAllForUser;
using Messenger.BLL.MediatR.PersonalMessage.Create;
using Messenger.BLL.MediatR.Profile.UpdateConnection;
using Messenger.BLL.MediatR.Profile.UpdateLastSeenOnConnect;
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
        var userChats = await _mediator.Send(new GetAllChatsForUserQuery(profileId));
        List<MessageReceiveDto> messages = new List<MessageReceiveDto>();
        List<IEnumerable<MessageReceiverReceiveDto>> messagesReceived = new List<IEnumerable<MessageReceiverReceiveDto>>();
        
        foreach (var userChat in userChats.Value)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userChat.ChatId.ToString());
            
            if (userChat.LastMessageId is null )
            {
                continue;
            }
            
            var message = await _mediator.Send(new GetMessageByIdQuery(userChat.LastMessageId.Value));
            if (message.IsSuccess)
            {
                messages.Add(message.Value);
            }

            var receivers = await _mediator.Send(new GetReceiversForMessageQuery(userChat.LastMessageId.Value));
            if (receivers.IsSuccess)
            {
                messagesReceived.Add(receivers.Value);
            }


        }
        
        // need to send all the groups/chats to user ? 
        // need to think what i need when i initially connected to the server 
        // need to rename to LoadChats
        await Clients.Caller.SendAsync("GetChats", new {
            Chats = userChats.Value,
            Messages = messages,
            Receivers = messagesReceived
        });
      
    }

    public async Task SendMessage(MessageSendDto message)
    {
        // need to store the message
        var savedMessage = await _mediator.Send(new CreateMessageCommand(message));
        
        // need to send the message to group 
        if (savedMessage.IsSuccess)
        {
            await Clients.Group(message.ChatId.ToString()).SendAsync("GetMessage", savedMessage.Value);
        }
        
        // need to send the some responce to the sender(?)
        
        
    }
    public async Task MarkMessageAsReceived(MessageReceiverSendDto messageReceiver)
    {
        var savedMessageReceiver = await _mediator.Send(new CreateMessageReceiverCommand(messageReceiver));

        if (savedMessageReceiver.IsSuccess)
        {
            await Clients.All.SendAsync("GetReceivedMessageInformation", savedMessageReceiver.Value);
        }
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

    public async Task DeleteMessage(int messageId, int userId)
    {
        var groupIdWhereDeleted = await _mediator.Send(new DeleteMessageQuery(messageId, userId));
        if (groupIdWhereDeleted.IsSuccess)
        {
            await Clients.Group(groupIdWhereDeleted.Value.ToString()).SendAsync("ReceiveDeletedMessage", messageId);
            await Clients.Caller.SendAsync("ConfirmDeleteMessage", messageId);
        }
        else
        {
            await Clients.Caller.SendAsync("ConfirmDeleteMessage", -1);
        }
        
    }
    
    public async Task EditMessage(MessageEditDto message, int userId)
    {
        var groupIdWhereEdited = await _mediator.Send(new EditMessageQuery(message, userId));
        if (groupIdWhereEdited.IsSuccess)
        {
            await Clients.Group(groupIdWhereEdited.Value.ToString()).SendAsync("ReceiveEditedMessage", groupIdWhereEdited.Value);
            await Clients.Caller.SendAsync("ConfirmEditedMessage", message.MessageId);
        }
        else
        {
            await Clients.Caller.SendAsync("ConfirmEditedMessage", -1);
        }
    }
    
    public async Task BlockUser(int userToBlockId, int userId)
    {
        var personalChatThatIsBlocked = await _mediator.Send(new BlockPersonalChatCommand(userToBlockId, userId));

        if (personalChatThatIsBlocked.IsFailed)
        {
            await Clients.Caller.SendAsync("ConfirmBlockingPersonalChat", -1);
            return;
        }
        
        await Clients.Group(personalChatThatIsBlocked.Value.ToString()).SendAsync("ReceiveBlockedUser", userId);

        var allConnectionOfUser = await _mediator.Send(new GetConectionsOfUserQuery(userId));
        if (allConnectionOfUser.IsSuccess)
        {
            foreach (var connectionOfUser in allConnectionOfUser.Value)
            {
                await Groups.RemoveFromGroupAsync(connectionOfUser, personalChatThatIsBlocked.Value.ToString());
            }
        }
        
        var allConnectionOfBlockedUser = await _mediator.Send(new GetConectionsOfUserQuery(userToBlockId));
        if (allConnectionOfBlockedUser.IsSuccess)
        {
            foreach (var connectionOfUser in allConnectionOfBlockedUser.Value)
            {
                await Groups.RemoveFromGroupAsync(connectionOfUser, personalChatThatIsBlocked.Value.ToString());
            }
        }
        
        await Clients.Caller.SendAsync("ConfirmBlockingPersonalChat", personalChatThatIsBlocked.Value);
    }
    
    public async Task OnDisconnectedAsync(Exception exception)
    {
        // need to get user from the connection id
        var profileId = await _mediator.Send(new GetPersonIdByConnectionCommand(Context.ConnectionId));

        if (profileId.IsFailed)
        {
            return;
        }
        
        var allConnectionOfUser = await _mediator.Send(new GetConectionsOfUserQuery(profileId.Value));

        if (allConnectionOfUser.IsFailed)
        {
            return;
        }

        if (allConnectionOfUser.Value.Count() > 1)
        {
            return;
        }

        // need to change the status to not online if person disconnects from the last device 
        // need to send this change to those who listenng (?)
        var lastSeen = new LastSeenDto()
        {
            ProfileId = profileId.Value,
            IsOnline = false,
            LastSeen = DateTime.Now
        };
        await _mediator.Send(new UpdateProfileLastSeenCommand(lastSeen));

        await SendLastSeenUpdate(lastSeen);
        
        Result<IEnumerable<ChatDto>> userChats = new ();
        if (profileId.IsSuccess)
        {
            // need to get all the groups/chats and add user to them
            userChats = await _mediator.Send(new GetAllChatsForUserQuery(profileId.Value));
        }
        
        foreach (var userChat in userChats.Value)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userChat.ChatId.ToString());
        }
        
    }
    
    // need to add all operations that is connected with groups/chats creating/deleting //UpdateProfile
    // public async Task CreatePersonalChat(PersonalChatUsersDto personalChatUsers)
    // {
    //    var newPersonalChat = await _mediator.Send(new CreatePersonalChatCommand(personalChatUsers))
    // }
}