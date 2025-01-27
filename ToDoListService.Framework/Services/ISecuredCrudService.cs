using ToDoListService.Framework.Dtos;
using ToDoListService.Framework.Security.Authentication;

namespace ToDoListService.Framework.Services;

public interface ISecuredCrudService<TId, TInputDto, TOutputDto>
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
{
    Task<TOutputDto> CreateAsync(TInputDto dto, AuthenticatedUser user);

    Task<IEnumerable<TOutputDto>> ReadAllAsync(AuthenticatedUser user);

    Task<TOutputDto> ReadAsync(TId id, AuthenticatedUser user);

    Task<TOutputDto> UpdateAsync(TId id, TInputDto dto, AuthenticatedUser user);

    Task DeleteAsync(TId id, AuthenticatedUser user);
}
