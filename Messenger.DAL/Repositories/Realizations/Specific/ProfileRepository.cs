using Messenger.DAL.Entities;
using Messenger.DAL.Enums;
using Messenger.DAL.Persistence;
using Messenger.DAL.Repositories.Interfaces.Specific;
using Messenger.DAL.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DAL.Repositories.Realizations.Specific;

public class ProfileRepository : RepositoryBase<Profile>, IProfileRepository
{
    public ProfileRepository(MessengerDBContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Profile>> GetAllProfilesInChatTypeWithChatStatusAsync(int userId, ChatType chatType, ChatStatus chatStatus)
    {
        var blockedUsers = await _dbContext.UserOfChats
            .Include(u => u.Chat)
            .Include(u => u.Profile)
            .Where(uoc => uoc.ProfileId == userId) 
            .Where(uoc => uoc.Chat.Type == chatType) 
            .SelectMany(uoc => uoc.Chat.UsersOfChat) 
            .Where(uoc => uoc.Status == chatStatus && uoc.ProfileId != userId ) 
            .Select(uoc => uoc.Profile) 
            .Distinct()  
            .AsNoTracking()
            .ToListAsync();
                
        return blockedUsers;        
    }
}