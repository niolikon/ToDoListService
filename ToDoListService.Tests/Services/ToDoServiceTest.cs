using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;
using ToDoListService.Domain.Mappers;
using ToDoListService.Domain.Repositories;
using ToDoListService.Domain.Services;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Tests.TestData;

namespace ToDoListService.Tests.Services;

public class ToDoServiceTest
{
    private readonly Mock<IToDoRepository> _todoRepositoryMock;
    private readonly Mock<IToDoMapper> _todoMapperMock;
    private readonly Mock<UserManager<User>> _usermanagerMock;
    private readonly ToDoService _service;

    public ToDoServiceTest()
    {
        _todoRepositoryMock = new Mock<IToDoRepository>();
        _todoMapperMock = new Mock<IToDoMapper>();
        _usermanagerMock = new Mock<UserManager<User>>(
            (new Mock<IUserStore<User>>()).Object,
            null, // IOptions<IdentityOptions>
            null, // IPasswordHasher<User>
            null, // IEnumerable<IUserValidator<User>>
            null, // IEnumerable<IPasswordValidator<User>>
            null, // ILookupNormalizer
            null, // IdentityErrorDescriber
            null, // IServiceProvider
            null  // ILogger<UserManager<User>>
        );
        _service = new ToDoService(_todoRepositoryMock.Object, _todoMapperMock.Object, _usermanagerMock.Object);
    }

    [Fact]
    [Trait("Story", "TLS1")]
    public async Task Given_OwnerWithPendingToDos_When_UserRequestsPendingList_Then_ListIsProvided()
    {
        User owner = ToDoTestData.USER_1;
        AuthenticatedUser authenticatedUser = new() { Id = owner.Id, UserName = owner.UserName };

        _usermanagerMock.Setup(usermanager => usermanager.FindByIdAsync(authenticatedUser.Id))
            .ReturnsAsync(owner);
        _todoRepositoryMock.Setup(repository => repository.ReadAllAsync(owner, It.IsAny<Expression<Func<ToDo, bool>>>()))
            .ReturnsAsync([ToDoTestData.TODO_1, ToDoTestData.TODO_2]);
        _todoMapperMock.Setup(mapper => mapper.mapToOutputDto(It.IsAny<ToDo>()))
            .Returns((new Mock<ToDoOutputDto>()).Object);

        await _service.ReadAllPendingAsync(authenticatedUser);

        _todoRepositoryMock.Verify(repository =>
            repository.ReadAllAsync(owner, It.IsAny<Expression<Func<ToDo, bool>>>()),
            Times.Once);
    }

    [Fact]
    [Trait("Story", "TLS1")]
    [Trait("Scenario", "1")]
    public async Task Given_ToDoNotComplete_When_OwnerRequestsMarkingComplete_Then_ToDoIsUpdated()
    {
        ToDo targetTodo = ToDoTestData.TODO_NOT_COMPLETED;
        int id = targetTodo.Id;
        ToDoPatchDto patchDto = ToDoTestData.TODO_PATCH_COMPLETED;
        User owner = targetTodo.Owner;
        AuthenticatedUser authenticatedUser = new() { Id = owner.Id, UserName = owner.UserName };
        
        _usermanagerMock.Setup(usermanager => usermanager.FindByIdAsync(authenticatedUser.Id))
            .ReturnsAsync(owner);
        _todoRepositoryMock.Setup(repository => repository.ReadAsync(id, owner))
            .ReturnsAsync(targetTodo);
        _todoRepositoryMock.Setup(repository => repository.UpdateAsync(It.IsAny<ToDo>(), owner))
            .ReturnsAsync((new Mock<ToDo>()).Object);
        _todoMapperMock.Setup(mapper => mapper.mapToOutputDto(It.IsAny<ToDo>()))
            .Returns((new Mock<ToDoOutputDto>()).Object);

        await _service.PatchAsync(id, patchDto, authenticatedUser);

        _todoRepositoryMock.Verify(repository => 
            repository.UpdateAsync(
                It.Is<ToDo>(t => t.IsCompleted == true), 
                owner), 
            Times.Once);
    }

    [Fact]
    [Trait("Story", "TLS1")]
    [Trait("Scenario", "2")]
    public async Task Given_ToDoComplete_When_OwnerRequestsMarkingComplete_Then_ToDoIsNotUpdated()
    {
        ToDo targetTodo = ToDoTestData.TODO_COMPLETED;
        int id = targetTodo.Id;
        ToDoPatchDto patchDto = ToDoTestData.TODO_PATCH_COMPLETED;
        User owner = targetTodo.Owner;
        AuthenticatedUser authenticatedUser = new() { Id = owner.Id, UserName = owner.UserName };

        _usermanagerMock.Setup(usermanager => usermanager.FindByIdAsync(authenticatedUser.Id))
            .ReturnsAsync(owner);
        _todoRepositoryMock.Setup(repository => repository.ReadAsync(id, owner))
            .ReturnsAsync(targetTodo);
        _todoRepositoryMock.Setup(repository => repository.UpdateAsync(It.IsAny<ToDo>(), owner))
            .ReturnsAsync((new Mock<ToDo>()).Object);
        _todoMapperMock.Setup(mapper => mapper.mapToOutputDto(It.IsAny<ToDo>()))
            .Returns((new Mock<ToDoOutputDto>()).Object);

        await _service.PatchAsync(id, patchDto, authenticatedUser);

        _todoRepositoryMock.Verify(repository => 
            repository.UpdateAsync(
                It.IsAny<ToDo>(),
                owner), 
        Times.Never);
    }
}