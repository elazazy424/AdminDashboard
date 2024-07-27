# AdminDashboard

AdminDashboard is a modular web application developed using ASP.NET Core. The project is organized into three main layers: `AdminDashboard`, `AdminDashboard.BLL`, and `AdminDashboard.DAL`. Each layer follows a clear separation of concerns, promoting maintainability and scalability.

## Project Structure

### AdminDashboard
The main web application layer, responsible for handling HTTP requests and returning appropriate responses.

- **Controllers**
  - `AuthController.cs`: Handles authentication-related actions.
  - `MessageController.cs`: Manages message-related operations.
  - `TestController.cs`: Contains test actions and endpoints.

- **Properties**
  - Contains project properties and settings.

- **appsettings.json**
  - Configuration file for application settings.

- **Program.cs**
  - The entry point for the application.

### AdminDashboard.BLL (Business Logic Layer)
This layer contains the business logic and the data transfer objects (DTOs).

- **Constants**
  - Contains constant values used across the BLL.

- **DTOs**
  - **Auth**
    - `LoginDto.cs`
    - `LoginServiceResponseDto.cs`
    - `RegisterDto.cs`
    - `UpdateRoleDto.cs`
    - `UserInfoResultDto.cs`
  - **General**
    - `GeneralServiceResponseDto.cs`
  - **Log**
    - `GetLogDto.cs`
  - **Message**
    - `CreateMessageDto.cs`
    - `GetMessageDto.cs`

- **Interfaces**
  - `IAuthService.cs`
  - `ILogService.cs`
  - `IMessageService.cs`

- **Services**
  - `AuthService.cs`: Implements authentication logic.
  - `LogService.cs`: Manages logging operations.
  - `MessageService.cs`: Handles message-related business logic.

### AdminDashboard.DAL (Data Access Layer)
This layer is responsible for data access and database interactions.

- **Config**
  - Contains configuration settings for data access.

- **Entity**
  - `ApplicationDbContext.cs`: Database context for Entity Framework Core.
  - `ApplicationUser.cs`: Entity representing application users.
  - `BaseEntity.cs`: Base class for all entities.
  - `Log.cs`: Entity representing log entries.
  - `Message.cs`: Entity representing messages.

- **Migrations**
  - Contains database migration files.

## Getting Started

To get started with AdminDashboard, follow these steps:

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/AdminDashboard.git
   ```

2. **Navigate to the project directory**
   ```bash
   cd AdminDashboard
   ```

3. **Set up the database**
   Update the connection string in `appsettings.json` and run the migrations.
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

**Swagger Screenshots**

![image](https://github.com/user-attachments/assets/dd08696b-36d1-4d26-9f96-caad611e46df)

![image](https://github.com/user-attachments/assets/55ab20c2-b6df-495e-b4fc-878b8bb7aa47)

![image](https://github.com/user-attachments/assets/d0a66bf1-7248-465d-a406-1ce764115406)
