using Messenger.DAL.Persistence;
using Messenger.DAL.Repositories.Interfaces.Base;
using Messenger.DAL.Repositories.Interfaces.Specific;
using Messenger.DAL.Repositories.Realizations.Specific;

namespace Messenger.DAL.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly MessengerDBContext _context;

    public RepositoryWrapper(MessengerDBContext context)
    {
        _context = context;
    }

    private IChatRepository _chatRepository;
    private IMessageRepository _messageRepository;
    private IMessageReceiverRepository _messageReceiverRepository;
    private IProfileRepository _profileRepository;
    private IUserContactRepository _userContactRepository;
    private IUserOfChatRepository _userOfChatRepository;
    
    public IChatRepository ChatRepository => _chatRepository ??= new ChatRepository(_context);
    public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context); 
    public IMessageReceiverRepository MessageReceiverRepository => _messageReceiverRepository ??=new MessageReceiverRepository(_context);
    public IProfileRepository ProfileRepository => _profileRepository ??= new ProfileRepository(_context);
    public IUserContactRepository UserContactRepository => _userContactRepository ??= new UserContactRepository(_context);
    public IUserOfChatRepository UserOfChatRepository => _userOfChatRepository ??= new UserOfChatRepository(_context);
    
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}