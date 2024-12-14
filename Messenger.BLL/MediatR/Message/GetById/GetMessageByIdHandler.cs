using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.MessageReceiver;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.Message.GetById;

public class GetMessageByIdHandler : IRequestHandler<GetMessageByIdQuery, Result<MessageReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetMessageByIdHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<MessageReceiveDto>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        var message = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageId,
            include: source => source
                .Include(m => m.MessageOwner)
                .Include(m => m.Receivers)
            );

        if (message is null)
        {
            var errorMessage = $"Message with id: {request.MessageId} does not exist";
            return Result.Fail(errorMessage);
        }
        
        var messageDto = _mapper.Map<MessageReceiveDto>(message);
        
        return Result.Ok(messageDto);
    }
}