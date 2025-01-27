﻿using Microsoft.AspNetCore.Identity;
using ToDoListService.Framework.Services;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;
using ToDoListService.Domain.Mappers;
using ToDoListService.Domain.Repositories;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Framework.Exceptions.Persistence;
using ToDoListService.Framework.Exceptions.Rest;

namespace ToDoListService.Domain.Services;

public class ToDoService : BaseSecuredCrudService<ToDo, int, ToDoInputDto, ToDoOutputDto, User>, IToDoService
{
    public ToDoService(IToDoRepository repository, IToDoMapper mapper, UserManager<User> userManager) : base(repository, mapper, userManager) { }

    public async Task<IEnumerable<ToDoOutputDto>> ReadAllPendingAsync(AuthenticatedUser user)
    {
        throw new NotImplementedException();
    }

    public async Task<ToDoOutputDto> PatchAsync(int id, ToDoPatchDto patchDto, AuthenticatedUser user)
    {
        throw new NotImplementedException();
    }
}
