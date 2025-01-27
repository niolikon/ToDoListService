using DotNet.Testcontainers.Containers;

namespace ToDoListService.Framework.Utils.Testcontainers;

public interface IContainerizedDatabaseFixture
{
    IDatabaseContainer Container { get; }
    string ConnectionString { get; }
}
