using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;

namespace ToDoListService.TestsIntegration.TestData;

public static class ToDosTestScenarios
{
    public static UserLoginDto EmptyOwnerDto = new()
    {
        Username = "TestUsername",
        Password = "7357tP455w0rd!"
    };

    public static object[] Empty
    {
        get
        {
            User owner = new()
            {
                UserName = EmptyOwnerDto.Username,
                PasswordHash = EmptyOwnerDto.Password,
                Name = "Name",
                Surname = "Surname",
                Email = "name.surname@example.com"
            };

            object[] result = [owner];
            return result;
        }
    }

    public static UserLoginDto SingleOwnerDto = new()
    {
        Username = "TestUsername",
        Password = "7357tP455w0rd!"
    };

    public static object[] SingleToDo
    {
        get
        {
            ToDo todo = new () { 
                Id = 1, 
                Title = "Title text",
                Description = "Description long text",
                IsCompleted = true,
                DueDate = DateOnly.FromDateTime(DateTime.Now),

            };

            User owner = new ()
            {
                UserName = SingleOwnerDto.Username,
                PasswordHash = SingleOwnerDto.Password,
                Name = "Name",
                Surname = "Surname",
                Email = "name.surname@example.com"
            };
            todo.Owner = owner;

            object[] result = [todo, owner];
            return result;
        }
    }
}
