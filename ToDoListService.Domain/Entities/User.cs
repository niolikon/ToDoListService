using Microsoft.AspNetCore.Identity;

namespace ToDoListService.Domain.Entities;

public class User : IdentityUser, IEquatable<User>
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public List<ToDo> ToDos { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is User user)
        {
            return Equals(user);
        }

        return false;
    }

    public bool Equals(User? other)
    {
        if (other == null)
        {
            return false;
        }

        return Id.Equals(other.Id) && 
            object.Equals(UserName, other.UserName) &&
            object.Equals(PasswordHash, other.PasswordHash) &&
            Name.Equals(other.Name) && 
            Surname.Equals(other.Surname) &&
            Email.Equals(other.Email);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, UserName, Email, PasswordHash, Name, Surname, Email);
    }
}
