# E-Games

# E-Games Web API

## Description
E-Games Web API is an ASP.NET Core application that provides a backend for managing games, orders, and user profiles. It includes authentication features and integrates with external services like Cloudinary for image management.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Installation](#installation)
3. [Configuration](#configuration)
4. [API Endpoints](#api-endpoints)
5. [Database](#database)
6. [Logging](#logging)
7. [Email Configuration](#email-configuration)
8. [Cloudinary Integration](#cloudinary-integration)
9. [Contributing](#contributing)
10. [License](#license)

## Prerequisites
- .NET Core SDK (version X.X or later)
- SQL Server Express
- SMTP server access (for email functionality)
- Cloudinary account (for image management)

## Installation
1. Clone the repository: git clone https://github.com/george-pench/e-games-web-api.git

2. Navigate to the project directory: cd e-games-web-api

3. Restore the required packages: dotnet restore

4. Build the project: dotnet build
   
## Configuration
The application uses `appsettings.json` for configuration. Make sure to update the following sections:

### Connection String
Update the `DefaultConnection` string in the `ConnectionStrings` section:

5. Logging
The application uses Serilog for logging. Logs are written to separate files based on log levels in the logs/ directory.

6. SMTP Settings.
Update the SmtpSettings section with your email server details:

"SmtpSettings": {
  "Server": "smtp.server.com",
  "Port": 44311,
  "UseSsl": true,
  "Username": "your_username",
  "Password": "your_password",
  "SenderEmail": "your_email@example.com",
  "SenderName": "Your Name"
}

7. Cloudinary
Add your Cloudinary credentials in the Cloudinary section:

"Cloudinary": {
  "CloudName": "your_cloud_name",
  "ApiKey": "your_api_key",
  "ApiSecret": "your_api_secret"
}

## API Endpoints

Auth

POST /api/auth/signIn: Signs in a user
POST /api/auth/signUp: Signs up a new user
GET /api/auth/emailConfirm: Confirms user email

Games

GET /api/games/topPlatforms: Retrieves top platforms based on game count
GET /api/games/search: Searches for games with pagination
GET /api/games/id/{id}: Retrieves game details by ID
DELETE /api/games/id/{id}: Deletes a game by ID
POST /api/games: Creates a new game
PUT /api/games: Updates an existing game
POST /api/games/rating: Updates a game's rating
DELETE /api/games/rating: Removes a game's rating
GET /api/games/list: Retrieves platforms based on query parameters

Orders

POST /api/orders: Creates a new order
GET /api/orders: Retrieves order information
PUT /api/orders: Updates an order
DELETE /api/orders: Deletes an order
POST /api/orders/buy: Processes a purchase

User

GET /api/user: Retrieves user profile
PUT /api/user: Updates user profile
PATCH /api/user/password: Updates user password

Home

GET /: Home page
GET /Home/GetInfo: Retrieves info (Admin only)

8. Database

The application uses SQL Server Express. Ensure you have it installed and running. The database name is set to E-Games in the connection string.

## License
This project is licensed under the MIT License.
