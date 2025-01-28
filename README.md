# ToDoListService

📚 **ToDoListService** is a simple Web API project designed to manage Personal Tasks as a ToDo list.
This project demonstrates fundamental software development concepts such as BDD (Behavior-Driven Development), unit testing, and clean architecture principles.

---

## 🚀 Features

- **ToDo Management**: Manage ToDos with CRUD operations.
- **User Management**: Manage system users with CRUD operations.
- **Dependency Injection**: Decouple components for better testability and maintainability.
- **Rest exceptions Management**: Centralize JSON error response management with middleware for better separation of concerns.

---

## 📖 User Stories

- ✅ [ToDo Marking](https://github.com/niolikon/ToDoListService/issues/1)

---

## 🛠️ Getting Started

### Prerequisites

- [.NET 8 or later](https://dotnet.microsoft.com/download)
- A text editor or IDE (e.g., [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/))

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/niolikon/ToDoListService.git
   cd ToDoListService
   ```
   
2. Restore dependencies
   ```bash
   dotnet restore
   ```
   
4. Run the project
   ```bash
   dotnet run
   ```
   
### Deploy on container
   
1. Create build artifact
   ```bash
   dotnet publish ToDoListService.Api/ToDoListService.Api.csproj -c Development -o ./output
   ```
   
2. Create project image
   ```bash
   docker build -t todolistservice-api:latest .
   ```
   
3. Configure database password
   ```bash
   echo "SA_PASSWORD=YourStrong@Passw0rd" >.env
   ```
   
4. Compose docker container
   ```bash
   docker-compose up -d
   ```

---

## 📬 Feedback

If you have suggestions or improvements, feel free to open an issue or create a pull request. Contributions are welcome!

---

## 📝 License

This project is licensed under the MIT License.