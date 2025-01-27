using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ToDoListService.Framework.Entities;

namespace ToDoListService.Framework.Utils.EntityFrameworkCore;

public class IdentityDatabasePreSeederPostCleaner<TDbContext, TUser, TEntityId> : DatabasePreSeederPostCleaner<TDbContext>
    where TDbContext : DbContext
    where TUser : IdentityUser
{
    protected readonly UserManager<TUser> _userManager;

    public IdentityDatabasePreSeederPostCleaner(TDbContext context, UserManager<TUser> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public override void PopulateDatabase(object[] data)
    {
        _context.Database.EnsureCreated();
        DisableForeignKeys();

        var ownerships = from owner in data
                         where owner is TUser
                         select new {
                            Owner = owner as TUser,
                            Belongings = (from obj in data
                                        where (obj is BaseOwnedEntity<TEntityId, TUser> owned) &&
                                                (owned.Owner == owner)
                                        select obj as BaseOwnedEntity<TEntityId, TUser>).ToList()
                         };

        foreach(var entry in ownerships)
        {
            IdentityResult result = _userManager.CreateAsync(entry.Owner, entry.Owner.PasswordHash).GetAwaiter().GetResult();
            if (! result.Succeeded)
            {
                StringBuilder errorStringBuilder = new StringBuilder();
                foreach(IdentityError error in result.Errors)
                {
                    errorStringBuilder.Append($"{error.Code}: {error.Description},");
                }
                errorStringBuilder[^1] = ' ';
                ;
                throw new InvalidDataException(errorStringBuilder.ToString().TrimEnd());
            }

            foreach(var belonging in entry.Belongings)
            {
                BaseOwnedEntity<TEntityId, TUser> entity = belonging;
                entity.Owner = entry.Owner;
                _context.Add(belonging);
            }
        }

        var ownershipEntities = ownerships.SelectMany(entry => {
            List<object> result = new List<object>();
            result.Add(entry.Owner);
            result.AddRange(entry.Belongings);
            return result;
        }).Distinct();

        var otherEntities = data.Where(entity => !ownershipEntities.Contains(entity));
        foreach (var entry in otherEntities)
        {
            _context.Add(entry);
        }

        _context.SaveChanges();

        EnableForeignKeys();
    }
}
