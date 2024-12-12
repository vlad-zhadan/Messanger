using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChat;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.PersonalChat.GetAllForUser;

public class GetAllPersonalChatsForUserHandler : IRequestHandler<GetAllPersonalChatsForUserQuery, Result<IEnumerable<PersonalChatDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetAllPersonalChatsForUserHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    
    public async Task<Result<IEnumerable<PersonalChatDto>>> Handle(GetAllPersonalChatsForUserQuery request, CancellationToken cancellationToken)
    {
        var userOfChats = await _wrapper.UserOfChatRepository.FindAll(
            predicate: cou => cou.ProfileId == request.UserId,
            include: source => source.Include(cou => cou.Chat)
        ).ToListAsync(cancellationToken);;

        try
        {
            List<PersonalChatDto> userChats = new();
            foreach (var userOfChat in userOfChats)
            {
                var chatDto = _mapper.Map<PersonalChatDto>(userOfChat.Chat);
                chatDto.Status = userOfChat.Status;
                userChats.Add(chatDto);
            }
            
            return Result.Ok(userChats.AsEnumerable());

        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}