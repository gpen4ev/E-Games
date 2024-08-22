# E-Games Web API

E-Games Web API is an ASP.NET Core application that serves as the backend for managing games, orders, and user profiles. It includes features such as authentication and integration with external services like Cloudinary for image management.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Installation](#installation)
3. [Configuration](#configuration)
   - [Connection String](#connection-string)
   - [Logging](#logging)
   - [SMTP Settings](#smtp-settings)
   - [Cloudinary Integration](#cloudinary-integration)
4. [API Endpoints](#api-endpoints)
   - [Auth](#auth)
   - [Games](#games)
   - [Orders](#orders)
   - [User](#user)
   - [Home](#home)
5. [Database](#database)
6. [License](#license)

## Prerequisites

Before you can run the E-Games Web API, ensure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version X.X or later)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- SMTP server access (for email functionality)
- [Cloudinary account](https://cloudinary.com/) (for image management)

## Installation

Follow these steps to set up the project:

1. **Clone the repository:**

   ```bash
   git clone https://github.com/george-pench/e-games-web-api.git

2. **Navigate to the project directory:**

   ```bash
   cd e-games-web-api

3. **Restore the required packages:**

   ```bash
   dotnet restore

4. **Build the project::**
   ```bash
   dotnet build

## Configuration

The application uses `appsettings.json` for configuration. Be sure to update the following sections according to your environment:

### Connection String

Update the `DefaultConnection` string in the `ConnectionStrings` section of `appsettings.json` to match your SQL Server setup:

\```
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server_name;Database=E-Games;User Id=your_username;Password=your_password;"
}
\```

### Logging

The application uses Serilog for logging. Logs are written to separate files based on log levels in the `logs/` directory. The logging configuration can be found in the `Logging` section of `appsettings.json`.

Here is an example of what the `Logging` section might look like:

\```
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft": "Warning",
    "Microsoft.Hosting.Lifetime": "Information"
  }
}
\```

### Cloudinary Integration

To manage images using Cloudinary, add your credentials in the `Cloudinary` section of `appsettings.json`:

\```
"Cloudinary": {
  "CloudName": "your_cloud_name",
  "ApiKey": "your_api_key",
  "ApiSecret": "your_api_secret"
}
\```

## API Endpoints

The API endpoints are organized by their respective functionalities. Below is a comprehensive list of available endpoints.

### Auth

- **POST** `/api/auth/signIn`: Signs in a user
- **POST** `/api/auth/signUp`: Signs up a new user
- **GET** `/api/auth/emailConfirm`: Confirms user email

### Games

- **GET** `/api/games/topPlatforms`: Retrieves top platforms based on game count
- **GET** `/api/games/search`: Searches for games with pagination
- **GET** `/api/games/id/{id}`: Retrieves game details by ID
- **DELETE** `/api/games/id/{id}`: Deletes a game by ID
- **POST** `/api/games`: Creates a new game
- **PUT** `/api/games`: Updates an existing game
- **POST** `/api/games/rating`: Updates a game's rating
- **DELETE** `/api/games/rating`: Removes a game's rating
- **GET** `/api/games/list`: Retrieves platforms based on query parameters

### Orders

- **POST** `/api/orders`: Creates a new order
- **GET** `/api/orders`: Retrieves order information
- **PUT** `/api/orders`: Updates an order
- **DELETE** `/api/orders`: Deletes an order
- **POST** `/api/orders/buy`: Processes a purchase

### User

- **GET** `/api/user`: Retrieves user profile
- **PUT** `/api/user`: Updates user profile
- **PATCH** `/api/user/password`: Updates user password

### Home

- **GET** `/`: Home page
- **GET** `/Home/GetInfo`: Retrieves information (Admin only)

### Database

The application uses SQL Server Express. Ensure that it is installed and running. The database is named **E-Games** as defined in the connection string.

### License

This project is licensed under the MIT License.
