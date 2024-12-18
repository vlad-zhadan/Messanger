using Messenger.DAL.Entities;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.DAL.Repositories.Interfaces.Specific;

public interface IMessageRepository : IRepositoryBase<Message>
{
    public Task<int> GetNumberOfUnreadMessagesByUserOfChatAsync(int userOfChatId);
}