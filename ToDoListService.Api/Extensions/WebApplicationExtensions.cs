﻿using ToDoListService.Infrastructure.Repositories;

namespace ToDoListService.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication EnsureDatabaseCreated(this WebApplication webApplication)
    {
        using (var scope = webApplication.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }

        return webApplication;
    }
}
