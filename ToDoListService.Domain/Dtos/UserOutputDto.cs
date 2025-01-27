using ToDoListService.Framework.Dtos;

namespace ToDoListService.Domain.Dtos;

public class UserOutputDto: BaseOutputDto<string>
{
    public string UserName { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }
}
