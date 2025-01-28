using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;
using ToDoListService.Framework.Utils.Testcontainers;
using ToDoListService.Framework.Utils.EntityFrameworkCore;
using ToDoListService.Infrastructure.Repositories;
using ToDoListService.TestsIntegration.TestData;
using ToDoListService.Framework.Security.Jwt;

namespace ToDoListService.TestsIntegration.Controllers;

[Collection("ControllerIntegrationTest")]
public class ToDosControllerIntegrationTest
{
    private readonly ContainerizedWebApplicationFactory<Program, AppDbContext> _webApplicationFactory;

    public ToDosControllerIntegrationTest(ContainerizedSqlServerFixture fixture)
    {
        _webApplicationFactory = new ContainerizedWebApplicationFactory<Program, AppDbContext>(fixture);
    }

    private IdentityDatabasePreSeederPostCleaner<AppDbContext, User, int> GetSeederCleaner()
    {
        AppDbContext dbContext = _webApplicationFactory.GetService<AppDbContext>();
        UserManager<User> userManager = _webApplicationFactory.GetService<UserManager<User>>();
        return new IdentityDatabasePreSeederPostCleaner<AppDbContext, User, int>(dbContext, userManager);
    }

    [Fact]
    public async Task Get_All_Should_Return_No_Entities_On_Empty_Repository()
    {
        using var seederCleaner = GetSeederCleaner();
        seederCleaner.PopulateDatabase(ToDoTestScenarios.Empty);

        HttpClient httpClient = _webApplicationFactory.CreateClient();
        UserLoginDto loginDto = ToDoTestScenarios.EmptyOwnerDto;
        HttpResponseMessage loginPostResponse = await httpClient.PostAsJsonAsync("/api/Users/login", loginDto);
        loginPostResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        loginPostResponse.Content.Should().NotBeNull();
        JwtTokenDto? userTokenDto = await loginPostResponse.Content.ReadFromJsonAsync<JwtTokenDto>();
        userTokenDto.Should().NotBeNull();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userTokenDto.Token);

        IEnumerable<ToDoOutputDto>? returnedCourses = await httpClient.GetFromJsonAsync<IEnumerable<ToDoOutputDto>>("/api/ToDos");

        returnedCourses.Should().NotBeNull();
        returnedCourses.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Support_Marking_ToDo_WorkFlow()
    {
        using var seederCleaner = GetSeederCleaner();
        seederCleaner.PopulateDatabase(ToDoTestScenarios.SingleCompleted);

        HttpClient httpClient = _webApplicationFactory.CreateClient();
        UserLoginDto loginDto = ToDoTestScenarios.SingleCompletedOwnerDto;
        HttpResponseMessage loginPostResponse = await httpClient.PostAsJsonAsync("/api/Users/login", loginDto);
        loginPostResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        loginPostResponse.Content.Should().NotBeNull();
        JwtTokenDto? userTokenDto = await loginPostResponse.Content.ReadFromJsonAsync<JwtTokenDto>();
        userTokenDto.Should().NotBeNull();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userTokenDto.Token);

        IEnumerable<ToDoOutputDto>? returnedToDos = await httpClient.GetFromJsonAsync<IEnumerable<ToDoOutputDto>>("/api/ToDos");
        returnedToDos.Should().NotBeNull();
        returnedToDos.Should().HaveCount(1);

        IEnumerable<ToDoOutputDto>? pendingToDos = await httpClient.GetFromJsonAsync<IEnumerable<ToDoOutputDto>>("/api/ToDos/pending");
        pendingToDos.Should().NotBeNull();
        pendingToDos.Should().HaveCount(0);

        HttpResponseMessage todoPostResponse = await httpClient.PostAsJsonAsync("/api/ToDos", ToDoTestData.TODO_1_INPUT);
        todoPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        todoPostResponse.Content.Should().NotBeNull();
        todoPostResponse.Headers.Should().ContainKey("Location");
        ToDoOutputDto? postedToDo = await todoPostResponse.Content.ReadFromJsonAsync<ToDoOutputDto>();
        postedToDo.Should().NotBeNull();
        postedToDo.Id.Should().NotBe(0);
        postedToDo.IsCompleted.Should().BeFalse();

        returnedToDos = await httpClient.GetFromJsonAsync<IEnumerable<ToDoOutputDto>>("/api/ToDos");
        returnedToDos.Should().NotBeNull();
        returnedToDos.Should().HaveCount(2);

        pendingToDos = await httpClient.GetFromJsonAsync<IEnumerable<ToDoOutputDto>>("/api/ToDos/pending");
        pendingToDos.Should().NotBeNull();
        pendingToDos.Should().HaveCount(1);

        HttpResponseMessage todoPatchResponse = await httpClient.PatchAsJsonAsync($"/api/ToDos/{postedToDo.Id}", ToDoTestData.TODO_PATCH_COMPLETED);
        todoPatchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        todoPatchResponse.Content.Should().NotBeNull();
        ToDoOutputDto? patchedToDo = await todoPatchResponse.Content.ReadFromJsonAsync<ToDoOutputDto>();
        patchedToDo.Should().NotBeNull();
        patchedToDo.Id.Should().Be(postedToDo.Id);
        patchedToDo.IsCompleted.Should().BeTrue();

        returnedToDos = await httpClient.GetFromJsonAsync<IEnumerable<ToDoOutputDto>>("/api/ToDos");
        returnedToDos.Should().NotBeNull();
        returnedToDos.Should().HaveCount(2);

        pendingToDos = await httpClient.GetFromJsonAsync<IEnumerable<ToDoOutputDto>>("/api/ToDos/pending");
        pendingToDos.Should().NotBeNull();
        pendingToDos.Should().HaveCount(0);
    }
}
