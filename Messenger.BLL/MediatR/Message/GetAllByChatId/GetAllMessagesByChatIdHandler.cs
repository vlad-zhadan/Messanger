using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.PersonalMessage.GetAllByChatId;

public class GetAllMessagesByChatIdHandler : IRequestHandler<GetAllMessagesByChatIdQuery, Result<IEnumerable<MessageReceiveDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;


    public GetAllMessagesByChatIdHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<MessageReceiveDto>>> Handle(GetAllMessagesByChatIdQuery request, CancellationToken cancellationToken)
    {
        var messages = await _wrapper.MessageRepository.GetAllAsync(
            predicate: m => m.MessageOwner.ChatId == request.ChatId, 
            include: source => source
                .Include(m => m.MessageOwner)
                .Include(m => m.Receivers)
            );

        try
        {
            var messagesDto = _mapper.Map<IEnumerable<MessageReceiveDto>>(messages);
            return Result.Ok(messagesDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}