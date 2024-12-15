using Messenger.DAL.Entities;
using Messenger.DAL.Enums;
using Messenger.DAL.Persistence;
using Messenger.DAL.Repositories.Interfaces.Specific;
using Messenger.DAL.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DAL.Repositories.Realizations.Specific;

public class ChatRepository : RepositoryBase<Chat>, IChatRepository
{
    public ChatRepository(MessengerDBContext context) : base(context)
    {
    }
    
    public async Task<Chat?> GetPersonalOrDefaultChatAsync(int firstUserId, int secondUserId)
    {
        var personalChat = await _dbContext.UserOfChats
            .Include(u => u.Chat)
            .Where(uoc => (uoc.ProfileId == firstUserId || uoc.ProfileId == secondUserId) && uoc.Chat.Type == ChatType.PersonalChat )
            .GroupBy(uoc => uoc.Chat)
            .Select(g => new
            {
                Chat = g.Key, 
                Count = g.Count()
            })
            .Where(g => g.Count == 2 )
            .Select(g => g.Chat)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return personalChat;
    }
}