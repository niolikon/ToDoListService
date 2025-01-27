using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoListService.Domain.Entities;

namespace ToDoListService.Infrastructure.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options): IdentityDbContext<User>(options)
{
    public override DbSet<User> Users { get; set; }
    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new {l.LoginProvider, l.ProviderKey});
        builder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new {r.UserId, r.RoleId});
        builder.Entity<IdentityUserToken<string>>()
            .HasKey(t => new {t.UserId, t.LoginProvider, t.Name});

        builder.Entity<ToDo>()
            .ToTable("Todos");

        builder.Entity<User>()
            .ToTable("Users");

        builder.Entity<User>()
            .HasMany(user => user.ToDos)
            .WithOne(todo => todo.Owner)
            .HasForeignKey(todo => todo.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
