# CoworkingReservationSystem

A full-stack ASP.NET Core solution for managing meeting room bookings in coworking spaces. This project features JWT authentication, reservation management with time conflict validation, email notifications, and implements Clean Architecture with Domain-Driven Design (DDD) principles.

## Features

- **Meeting Room Booking**: Reserve coworking space rooms with validation to prevent scheduling conflicts.
- **Authentication**: Secure login and registration using JWT.
- **Email Notifications**: Automated email updates for booking confirmations and changes.
- **Clean Architecture & DDD**: Modular, maintainable code structure.
- **Tech Stack**: ASP.NET Core Web API, Entity Framework Core, Bootstrap 5.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- A compatible IDE (e.g., Visual Studio, Rider, VS Code)
- SQL Server (or another supported database)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/NathanRocha01/CoworkingReservationSystem.git
   cd CoworkingReservationSystem
   ```

2. **Configure the database connection:**
   - Update the connection string in `appsettings.json` with your database credentials.

3. **Apply Migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Run the application:**
   ```bash
   dotnet run --project src/YourProjectName.WebApi
   ```

### Usage

- Access the API at `https://localhost:5001` (or your configured port).
- Use tools like Postman or Swagger UI to interact with endpoints.

## Project Structure

- `Domain/` – Core domain models and logic (DDD)
- `Application/` – Application services and interfaces
- `Infrastructure/` – Data access, external integrations
- `WebApi/` – API controllers and web entry point

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for improvements or bug fixes.

## License

This project is currently unlicensed. Please contact the repository owner for more information.

## Author

[NathanRocha01](https://github.com/NathanRocha01)
