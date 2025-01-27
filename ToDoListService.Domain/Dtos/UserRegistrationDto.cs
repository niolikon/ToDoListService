using System.ComponentModel.DataAnnotations;

namespace ToDoListService.Domain.Dtos;

public class UserRegistrationDto
{

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string PassWord { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Surname { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
