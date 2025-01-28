using ToDoListService.Domain.Dtos;

namespace ToDoListService.TestsIntegration.TestData;

public static class ToDoTestData
{

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

    #region ToDoPatchDto
    public static ToDoPatchDto TODO_PATCH_COMPLETED => new ToDoPatchDto() { IsCompleted = true };
    public static ToDoPatchDto TODO_PATCH_INCOMPLETE => new ToDoPatchDto() { IsCompleted = false };
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
