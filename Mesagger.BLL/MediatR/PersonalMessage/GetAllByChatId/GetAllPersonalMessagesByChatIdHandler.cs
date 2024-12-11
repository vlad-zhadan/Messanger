using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.PersonalMessage.GetAllByChatId;

public class GetAllPersonalMessagesByChatIdHandler : IRequestHandler<GetAllPersonalMessagesByChatIdQuery, Result<IEnumerable<PersonalMessageDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;


    public GetAllPersonalMessagesByChatIdHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<PersonalMessageDto>>> Handle(GetAllPersonalMessagesByChatIdQuery request, CancellationToken cancellationToken)
    {
        var messages = await _wrapper.MessageRepository.FindAll(
            predicate: m => m.MessageOwner.ChatId == request.ChatId, 
            include: source => source
                .Include(m => m.MessageOwner)
                .Include(m => m.Receivers)
            ).ToListAsync();

        try
        {
            var messagesDto = _mapper.Map<IEnumerable<PersonalMessageDto>>(messages);
            return Result.Ok(messagesDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}