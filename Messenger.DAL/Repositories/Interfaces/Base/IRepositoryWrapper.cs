using Messenger.DAL.Repositories.Interfaces.Specific;

namespace Messenger.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    public IChatRepository ChatRepository { get; }
    public IMessageRepository MessageRepository { get; }
    public IMessageReceiverRepository MessageReceiverRepository { get; }
    public IProfileRepository ProfileRepository { get; }
    public IUserContactRepository UserContactRepository { get; }
    public IUserOfChatRepository UserOfChatRepository { get; }
    
    public int SaveChanges();
    
    public Task<int> SaveChangesAsync();
}