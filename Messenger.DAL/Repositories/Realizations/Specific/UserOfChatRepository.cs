using Messenger.DAL.Enums;
using Messenger.DAL.Persistence;
using Messenger.DAL.Repositories.Interfaces.Specific;
using Messenger.DAL.Repositories.Realizations.Base;

namespace Messenger.DAL.Repositories.Realizations.Specific;

public class UserOfChatRepository : RepositoryBase<UserOfChat>, IUserOfChatRepository
{
    public UserOfChatRepository(MessengerDBContext context) : base(context)
    {
    }
}