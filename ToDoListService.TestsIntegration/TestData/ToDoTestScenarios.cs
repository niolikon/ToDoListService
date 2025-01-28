using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;

namespace ToDoListService.TestsIntegration.TestData;

public static class ToDoTestScenarios
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

    public static UserLoginDto SingleCompletedOwnerDto = new()
    {
        Username = "TestUsername",
        Password = "7357tP455w0rd!"
    };

    public static object[] SingleCompleted
    {
        get
        {
            ToDo todo = new () { 
                Title = "Short title",
                Description = "Short description adding details",
                IsCompleted = true,
                DueDate = DateOnly.FromDateTime(DateTime.Now),

            };

            User owner = new ()
            {
                UserName = SingleCompletedOwnerDto.Username,
                PasswordHash = SingleCompletedOwnerDto.Password,
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
