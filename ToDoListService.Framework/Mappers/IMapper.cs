namespace ToDoListService.Framework.Mappers;

public interface IMapper<TEntity, TInputDto, TOutputDto>
{
    TEntity mapToEntity(TInputDto dto);

    TOutputDto mapToOutputDto(TEntity entity);
}
