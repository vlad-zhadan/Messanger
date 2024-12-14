using Messenger.DAL.Entities;
using Messenger.DAL.Persistence;
using Messenger.DAL.Repositories.Interfaces.Specific;
using Messenger.DAL.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DAL.Repositories.Realizations.Specific;

public class MessageRepository : RepositoryBase<Message>, IMessageRepository
{
    public MessageRepository(MessengerDBContext context) : base(context)
    {
    }

    public async Task<int> GetNumberOfUnreadMessagesByUserOfChatAsync(int userOfChatId)
    {
        var numberOfUnreadMessagesForUserOfChat = await _dbContext.UserOfChats
            .Include(uoc => uoc.Chat)
            .Include(uoc => uoc.Messages)
            .ThenInclude(m => m.Receivers)
            .Where(uoc => uoc.ChatId == _dbContext.UserOfChats
                .Where(u => u.UserOfChatId == userOfChatId)
                .Select(u => u.ChatId)
                .FirstOrDefault())
            .SelectMany(uoc => uoc.Messages)
            .Where(m => m.Receivers.All(r => r.UserReceiverId != userOfChatId))
            .CountAsync();

        return numberOfUnreadMessagesForUserOfChat;
    }
}