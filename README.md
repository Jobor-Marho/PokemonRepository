# PokemonReviewApp

## Overview
PokemonReviewApp is a C# Web API application designed to manage and review Pokemon. This application follows a structured architecture with interfaces, models, repositories, and services.

## Project Structure
- **Interfaces**: Contains interface definitions for the application.
- **Models**: Contains the data models used in the application.
- **Repository**: Contains the repository classes for data access.
- **Services**: Contains service classes, including an `ErrorMessage` class for handling errors.
- **Seed.cs**: Contains the seeding logic for initializing the database with default data.

## Getting Started
### Prerequisites
- .NET SDK
- SQL Server or any other supported database

### Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/PokemonReviewApp.git
    ```
2. Navigate to the project directory:
    ```sh
    cd PokemonReviewApp
    ```
3. Restore the dependencies:
    ```sh
    dotnet restore
    ```

### Database Setup
1. Update the connection string in `appsettings.json` to point to your database.
2. Run the database migrations:
    ```sh
    dotnet ef database update
    ```
3. Seed the database:
    ```sh
    dotnet run --project Seed.cs
    ```

### Running the Application
Start the application:
```sh
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## Usage
### Endpoints
- `GET /api/pokemon` - Retrieves a list of all Pokemon.
- `GET /api/pokemon/{id}` - Retrieves a specific Pokemon by ID.
- `POST /api/pokemon` - Creates a new Pokemon.
- `PUT /api/pokemon/{id}` - Updates an existing Pokemon.
- `DELETE /api/pokemon/{id}` - Deletes a Pokemon.

### Error Handling
Errors are managed by the `ErrorMessage` class in the Services folder.

## Contributing
Contributions are welcome! Please fork the repository and create a pull request.

## License
This project is licensed under the MIT License.


