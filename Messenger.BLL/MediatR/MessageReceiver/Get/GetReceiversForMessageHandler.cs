using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.BLL.DTO.MessageReceiver;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.MessageReceiver.Get;

public class GetReceiversForMessageHandler : IRequestHandler<GetReceiversForMessageQuery, Result<IEnumerable<MessageReceiverReceiveDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetReceiversForMessageHandler(IRepositoryWrapper wrapper, IMapper mapper )
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<MessageReceiverReceiveDto>>> Handle(GetReceiversForMessageQuery request, CancellationToken cancellationToken)
    {
        var message = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageId,
            include: source => source
                .Include(m => m.Receivers)
                .ThenInclude(r => r.UserReceiver)
        );

        if (message is null)
        {
            var errorMessage = $"Message with id {request.MessageId} does not exist.";
            return Result.Fail(errorMessage);
        }

        var receivers = message.Receivers;
        var receiversDto = _mapper.Map<IEnumerable<MessageReceiverReceiveDto>>(receivers);
        
        return Result.Ok(receiversDto);
    }
}