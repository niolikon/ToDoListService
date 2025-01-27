using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoListService.Api.Controllers;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Services;
using ToDoListService.Framework.Exceptions.Rest;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Tests.TestData;

namespace ToDoListService.Tests.Controllers;

public class ToDosControllerTest
{
    private readonly Mock<IToDoService> _todosServiceMock;
    private readonly Mock<IAuthenticatedUserFactory> _authuserFactoryMock;
    private readonly ToDosController _controller;

    public ToDosControllerTest()
    {
        _todosServiceMock = new Mock<IToDoService>();
        _authuserFactoryMock = new Mock<IAuthenticatedUserFactory>();
        _controller = new ToDosController(_todosServiceMock.Object, _authuserFactoryMock.Object);
    }

    [Fact]
    [Trait("Story", "TLS1")]
    public async Task Given_AuthenticatedUser_When_UserRequestsPendingList_Then_RequestIsRelayed()
    {
        AuthenticatedUser authenticatedUserDummy = (new Mock<AuthenticatedUser>()).Object;
        _authuserFactoryMock
            .Setup(authuserFactory => authuserFactory.BuildAuthenticatedUser(It.IsAny<HttpContext>()))
            .Returns(authenticatedUserDummy);
        _todosServiceMock
            .Setup(service => service.ReadAllPendingAsync(authenticatedUserDummy))
            .ReturnsAsync([]);

        await _controller.ReadAllPendingAsync();

        _todosServiceMock.Verify(service => service.ReadAllPendingAsync(It.IsAny<AuthenticatedUser>()), Times.Once());
    }

    [Fact]
    [Trait("Story", "TLS1")]
    public async Task Given_NotAuthenticatedUser_When_UserRequestsPendingList_Then_RequestIsNotRelayed()
    {
        _authuserFactoryMock
            .Setup(authuserFactory => authuserFactory.BuildAuthenticatedUser(It.IsAny<HttpContext>()))
            .Throws(new UnauthorizedRestException("Error in user authentication"));

        Func<Task<ActionResult<IEnumerable<ToDoOutputDto>>>> function = async () => await _controller.ReadAllPendingAsync();
        await function.Should().ThrowAsync<UnauthorizedRestException>();

        _todosServiceMock.Verify(service => service.ReadAllPendingAsync(It.IsAny<AuthenticatedUser>()), Times.Never());
    }

    [Fact]
    [Trait("Story", "TLS1")]
    public async Task Given_AuthenticatedUser_When_UserRequestsMarkingCompleted_Then_RequestIsRelayed()
    {
        int id = 1;
        ToDoPatchDto patchDto = ToDoTestData.TODO_PATCH_COMPLETED;

        AuthenticatedUser authenticatedUserDummy = (new Mock<AuthenticatedUser>()).Object;
        _authuserFactoryMock
            .Setup(authuserFactory => authuserFactory.BuildAuthenticatedUser(It.IsAny<HttpContext>()))
            .Returns(authenticatedUserDummy);
        _todosServiceMock
            .Setup(service => service.PatchAsync(id, patchDto, authenticatedUserDummy))
            .ReturnsAsync((new Mock<ToDoOutputDto>()).Object);

        await _controller.PatchAsync(id, patchDto);

        _todosServiceMock.Verify(service => service.PatchAsync(id, patchDto, authenticatedUserDummy), Times.Once());
    }

    [Fact]
    [Trait("Story", "TLS1")]
    public async Task Given_NotAuthenticatedUser_When_UserRequestsMarkingCompleted_Then_RequestIsNotRelayed()
    {
        int id = 1;
        ToDoPatchDto patchDto = ToDoTestData.TODO_PATCH_COMPLETED;

        _authuserFactoryMock
            .Setup(authuserFactory => authuserFactory.BuildAuthenticatedUser(It.IsAny<HttpContext>()))
            .Throws(new UnauthorizedRestException("Error in user authentication"));

        Func<Task<ActionResult<ToDoOutputDto>>> function = async () => await _controller.PatchAsync(id, patchDto);
        await function.Should().ThrowAsync<UnauthorizedRestException>();

        _todosServiceMock.Verify(service => 
            service.PatchAsync(
                It.IsAny<int>(),
                It.IsAny<ToDoPatchDto>(),
                It.IsAny<AuthenticatedUser>()), 
            Times.Never());
    }
}
