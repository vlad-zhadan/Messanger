using Messenger.DAL.Entities;
using Messenger.DAL.Persistence;
using Messenger.DAL.Repositories.Interfaces.Specific;
using Messenger.DAL.Repositories.Realizations.Base;

namespace Messenger.DAL.Repositories.Realizations.Specific;

public class MessageReceiverRepository : RepositoryBase<MessageReceiver>, IMessageReceiverRepository
{
    public MessageReceiverRepository(MessengerDBContext context) : base(context)
    {
    }
}