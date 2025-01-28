using ToDoListService.Framework.Utils.Testcontainers;

namespace ToDoListService.TestsIntegration.Controllers;

[CollectionDefinition("ControllerIntegrationTest")]
public class ControllerIntegrationTestCollection : ICollectionFixture<ContainerizedSqlServerFixture>
{
}
