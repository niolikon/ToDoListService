using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;
using ToDoListService.Framework.Utils.Testcontainers;
using ToDoListService.Framework.Utils.EntityFrameworkCore;
using ToDoListService.Infrastructure.Repositories;
using ToDoListService.TestsIntegration.Scenarios;
using System.Net.Http.Headers;
using System.Net;
using ToDoListService.Framework.Security.Jwt;

namespace ToDoListService.TestsIntegration.Api.Controllers;

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
        var seederCleaner = GetSeederCleaner();
        seederCleaner.PopulateDatabase(ToDosTestScenarios.Empty);

        HttpClient httpClient = _webApplicationFactory.CreateClient();
        UserLoginDto loginDto = ToDosTestScenarios.EmptyOwnerDto;
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
}
