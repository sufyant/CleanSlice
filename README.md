# CleanSlice - .NET 9 WebAPI Template

[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A clean, production-ready .NET 9 WebAPI template following Clean Architecture principles with CQRS, Result patterns, and modern development practices.

## What's Inside

### Architecture

- **Clean Architecture** with clear layer separation
- **CQRS** pattern using MediatR
- **Result Pattern** for error handling
- **Pipeline Behaviors** for cross-cutting concerns
- **Multi-tenant** support
- **Domain Events** and **Value Objects**

### Project Structure

```
src/
├── CleanSlice.Api/          # Controllers, Middleware
├── CleanSlice.Application/  # Commands, Queries, Behaviors
├── CleanSlice.Domain/       # Entities, Value Objects
├── CleanSlice.Infrastructure/ # External Services
├── CleanSlice.Persistence/  # Database, Repositories
└── CleanSlice.Shared/       # Common Types, Results
```

### Key Features

- **Pipeline Behaviors**: Exception handling, logging, validation, caching, transactions
- **Result Pattern**: Type-safe error handling without exceptions
- **Value Objects**: Email, Money, FullName with validation
- **Auditable Entities**: CreatedAt, UpdatedAt tracking
- **Pagination**: Built-in paged results
- **Request Correlation**: Request tracking across the pipeline

## Quick Start

### Install Template

```bash
dotnet new install CleanSlice.Template
```

### Create New Project

```bash
dotnet new cleanslice -n YourProjectName
cd YourProjectName
dotnet run --project src/YourProjectName.Api
```

## Usage Examples

### Creating a Command

```csharp
public record CreateUserCommand(string Email, string Name) : ICommand<Guid>;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email);
        var user = User.Create(email, command.Name);

        // Save user...

        return Result.Success(user.Id);
    }
}
```

### Creating a Query

```csharp
public record GetUserQuery(Guid Id) : IQuery<UserResponse>;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        // Get user...
        return Result.Success(userResponse);
    }
}
```

### API Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
```

## Built-in Patterns

### Result Pattern

```csharp
// Success
Result<User> result = Result.Success(user);

// Failure
Result<User> result = Result.Failure<User>(Error.NotFound("User.NotFound", "User not found"));

// Usage
if (result.IsSuccess)
    return Ok(result.Value);
else
    return BadRequest(result.Error);
```

### Value Objects

```csharp
var email = Email.Create("user@example.com"); // Validates format
var money = Money.Create(100, "USD");
var name = FullName.Create("John", "Doe");
```

### Caching Queries

```csharp
public record GetUserQuery(Guid Id) : ICachedQuery<UserResponse>
{
    public string CacheKey => $"user-{Id}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(15);
}
```

### Transactional Commands

```csharp
public record CreateUserCommand(string Email) : ITransactionalCommand<Guid>;
// Automatically wrapped in database transaction
```

## Technologies Used

- .NET 9.0
- ASP.NET Core
- MediatR
- FluentValidation
- Serilog

## What You Get

✅ Clean Architecture setup  
✅ CQRS with MediatR  
✅ Result pattern for error handling  
✅ Pipeline behaviors (logging, validation, caching, transactions)  
✅ Value objects with validation  
✅ Multi-tenant architecture support  
✅ Pagination support  
✅ Request correlation tracking  
✅ OpenAPI/Swagger integration

## License

MIT License - see [LICENSE](LICENSE) file.

## Author

Sufyan Taskin
