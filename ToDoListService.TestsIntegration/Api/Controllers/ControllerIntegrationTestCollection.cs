using ToDoListService.Framework.Utils.Testcontainers;

namespace ToDoListService.TestsIntegration.Api.Controllers;

[CollectionDefinition("ControllerIntegrationTest")]
public class ControllerIntegrationTestCollection : ICollectionFixture<ContainerizedSqlServerFixture>
{
}
