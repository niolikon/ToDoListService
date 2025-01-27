using ToDoListService.Framework.Dtos;
using ToDoListService.Framework.Entities;
using ToDoListService.Framework.Exceptions.Persistence;
using ToDoListService.Framework.Exceptions.Rest;
using ToDoListService.Framework.Mappers;
using ToDoListService.Framework.Repositories;

namespace ToDoListService.Framework.Services;

public class BaseCrudService<TEntity, TId, TInputDto, TOutputDto> : ICrudService<TId, TInputDto, TOutputDto>
    where TEntity : BaseEntity<TId>
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
{
    protected ICrudRepository<TEntity, TId> _repository;
    protected IMapper<TEntity, TInputDto, TOutputDto> _mapper;

    public BaseCrudService(ICrudRepository<TEntity, TId> repository, 
        IMapper<TEntity, TInputDto, TOutputDto> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TOutputDto> CreateAsync(TInputDto dto)
    {
        TEntity entity = _mapper.mapToEntity(dto);
        try
        {
            TEntity entityInDb = await _repository.CreateAsync(entity);
            return _mapper.mapToOutputDto(entityInDb);
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not create entity");
        }
    }

    public async Task<IEnumerable<TOutputDto>> ReadAllAsync()
    {
        IEnumerable<TEntity> entities = await _repository.ReadAllAsync();
        return entities.Select(_mapper.mapToOutputDto).ToList();
    }

    public async Task<TOutputDto> ReadAsync(TId id)
    {
        try
        {
            TEntity entity = await _repository.ReadAsync(id);
            return _mapper.mapToOutputDto(entity);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
    }

    public async Task<TOutputDto> UpdateAsync(TId id, TInputDto dto)
    {
        TEntity entityWithUpdatedData = _mapper.mapToEntity(dto);
        entityWithUpdatedData.Id = id;

        try
        {
            TEntity entityUpdated = await _repository.UpdateAsync(entityWithUpdatedData);
            return _mapper.mapToOutputDto(entityUpdated);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not update entity");
        }
    }
    
    public async Task DeleteAsync(TId id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not delete entity");
        }
    }
}
