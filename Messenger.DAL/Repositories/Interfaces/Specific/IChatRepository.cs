using Messenger.DAL.Entities;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.DAL.Repositories.Interfaces.Specific;

public interface IChatRepository : IRepositoryBase<Chat>
{
    public Task<Chat?> GetPersonalOrDefaultChatAsync(int firstUserId, int secondUserId);
}