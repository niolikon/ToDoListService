using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;

namespace ToDoListService.Tests.TestData;

public static class ToDoTestData
{
    #region Owners
    public static User USER_1 => new User()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Jack",
        Surname = "The Tester",
        Email = "jack.thetester@example.com"
    };
    public static User USER_2 => new User()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Chuck",
        Surname = "The Thester",
        Email = "chuck.thetester@example.com"
    };
    #endregion

    #region ToDo
    public static ToDo TODO_1 => new ToDo()
    {
        Id = 1,
        Title = "Check the plants!",
        Description = "Give some water to the plants",
        IsCompleted = false,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
        Owner = USER_1
    };

    public static ToDo TODO_2 => new ToDo()
    {
        Id = 2,
        Title = "Check the water!",
        Description = "Put some water in the Tank",
        IsCompleted = false,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
        Owner = USER_1
    };

    public static ToDo TODO_3 => new ToDo()
    {
        Id = 3,
        Title = "Feed the dog",
        Description = "Give some food to the dog",
        IsCompleted = true,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
        Owner = USER_2
    };

    public static ToDo TODO_4 => new ToDo()
    {
        Id = 4,
        Title = "Check dog food",
        Description = "If there is no enough food we have to buy some dog food",
        IsCompleted = false,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
        Owner = USER_2
    };

    public static ToDo TODO_NOT_COMPLETED => TODO_1;

    public static ToDo TODO_COMPLETED => TODO_3;
    #endregion

    #region ToDoPatchDto
    public static ToDoPatchDto TODO_PATCH_COMPLETED => new ToDoPatchDto() { IsCompleted = true };
    public static ToDoPatchDto TODO_PATCH_INCOMPLETE => new ToDoPatchDto() { IsCompleted = false };
    #endregion

    #region ToDoInputDto
    public static ToDoInputDto TODO_1_INPUT => new ToDoInputDto()
    {
        Title = "Check the plants!",
        Description = "Give some water to the plants",
        IsCompleted = false,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15))
    };

    public static ToDoInputDto TODO_2_INPUT => new ToDoInputDto()
    {
        Title = "Check the water!",
        Description = "Put some water in the Tank",
        IsCompleted = false,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15))
    };

    public static ToDoInputDto TODO_3_INPUT => new ToDoInputDto()
    {
        Title = "Feed the dog",
        Description = "Give some food to the dog",
        IsCompleted = false,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15))
    };

    public static ToDoInputDto TODO_4_INPUT => new ToDoInputDto()
    {
        Title = "Check dog food",
        Description = "If there is no enough food we have to buy some dog food",
        IsCompleted = false,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15))
    };
    #endregion

    #region ToDoOutputDto
    public static ToDoOutputDto TODO_1_OUTPUT_COMPLETED => new ToDoOutputDto()
    {
        Id = 1,
        Title = "Check the plants!",
        Description = "Give some water to the plants",
        IsCompleted = true,
        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15))
    };
    #endregion
}
