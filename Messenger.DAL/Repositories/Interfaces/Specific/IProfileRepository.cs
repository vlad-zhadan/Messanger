using Messenger.DAL.Entities;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.DAL.Repositories.Interfaces.Specific;

public interface IProfileRepository : IRepositoryBase<Profile>
{
    public Task<IEnumerable<Profile>> GetAllProfilesInChatTypeWithChatStatusAsync(int userId, ChatType chatType,
        ChatStatus chatStatus);
}