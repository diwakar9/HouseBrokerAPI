# House Broker API

A comprehensive .NET 8 Web API for a house broker application built with Clean Architecture principles, featuring user authentication, property management, and advanced search capabilities.

## Features

### Core Functionality
- **User Authentication**: JWT-based authentication with role differentiation (House Seekers vs Brokers)
- **Property Management**: Complete CRUD operations for property listings
- **Advanced Search**: Filter properties by location, price range, property type, and more
- **Image Management**: Support for multiple property images with primary image designation
- **Property Features**: Customizable property features and amenities

### Technical Features
- **Clean Architecture**: Well-structured codebase with proper separation of concerns
- **Entity Framework Core**: Database operations with MSSQL Server
- **FluentValidation**: Comprehensive input validation
- **AutoMapper**: Object-to-object mapping
- **Swagger/OpenAPI**: Complete API documentation
- **Serilog**: Structured logging
- **JWT Authentication**: Secure token-based authentication

## Architecture

The solution follows Clean Architecture principles with four main layers:

1. **Domain Layer** (`HouseBroker.Domain`): Core business entities and interfaces
2. **Application Layer** (`HouseBroker.Application`): Business logic, DTOs, and services
3. **Infrastructure Layer** (`HouseBroker.Infrastructure`): Data access and external dependencies
4. **API Layer** (`HouseBroker.API`): Web API controllers and configuration

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone the repository** and navigate to the project directory

2. **Update Connection String**: 
   - Open `appsettings.json` in the API project
   - Update the `DefaultConnection` string to match your SQL Server instance

3. **Build the solution**:
   ```bash
   dotnet build
   ```

4. **Run the application**:
   ```bash
   cd src/HouseBroker.API
   dotnet run
   ```

5. **Access the API**:
   - API: `https://localhost:7000` or `http://localhost:5000`
   - Swagger UI: `https://localhost:7000` (root URL)

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user (House Seeker or Broker)
- `POST /api/auth/login` - User login
- `GET /api/auth/me` - Get current user information

### Properties
- `GET /api/properties` - Get all properties (paginated)
- `GET /api/properties/{id}` - Get property by ID
- `POST /api/properties/search` - Search properties with filters
- `GET /api/properties/broker/{brokerId}` - Get properties by broker
- `POST /api/properties` - Create property (Broker only)
- `PUT /api/properties/{id}` - Update property (Broker only)
- `DELETE /api/properties/{id}` - Delete property (Broker only)

## Sample Usage

### Register a Broker
```json
POST /api/auth/register
{
  "email": "broker@example.com",
  "password": "SecurePass123!",
  "firstName": "Diwakar",
  "lastName": "Dhungana",
  "role": "Broker"
}
```

### Create a Property
```json
POST /api/properties
Authorization: Bearer {your-jwt-token}
{
  "title": "Beautiful Dhungana Home",
  "description": "Spacious 4-bedroom house in quiet neighborhood",
  "propertyType": 1,
  "price": 450000,
  "address": "Pepsicola, Kathmandu",
  "city": "Kathmandu",
  "state": "Bagmati",
  "zipCode": "44600",
  "country": "Nepal",
  "latitude": 39.7817,
  "longitude": -89.6501,
  "bedrooms": 4,
  "bathrooms": 3,
  "squareFeet": 2500,
  "yearBuilt": 2015,
  "imageUrls": [
    "https://example.com/image1.jpg",
    "https://example.com/image2.jpg"
  ],
  "features": [
    {
      "name": "Open Roof Top",
      "description": "Spacious open rooftop terrace with panoramic views"
    },
    {
      "name": "Garage",
      "description": "2-car attached garage"
    }
  ]
}
```

### Search Properties
```json
POST /api/properties/search
{
  "city": "Kathmandu",
  "minPrice": 300000,
  "maxPrice": 500000,
  "propertyType": "House",
  "minBedrooms": 3,
  "pageNumber": 1,
  "pageSize": 10
}
```

## Database Schema

The application uses the following main entities:
- **Users**: User accounts with roles (House Seeker/Broker)
- **Properties**: Property listings with all details
- **PropertyImages**: Property photos with display order
- **PropertyFeatures**: Custom features and amenities

## Security

- JWT tokens expire after 24 hours
- Role-based authorization (Brokers can manage properties)
- Input validation using FluentValidation
- Secure password requirements
- CORS configuration for cross-origin requests

## Configuration

Key configuration options in `appsettings.json`:
- **ConnectionStrings**: Database connection
- **JWT**: Authentication settings (Secret, Issuer, Audience)
- **Serilog**: Logging configuration

## Third-Party Integration

The API is designed to be consumed by third-party applications:
- RESTful endpoints with standard HTTP methods
- JSON request/response format
- JWT authentication for secure access
- Comprehensive API documentation via Swagger

## Property Types

The API supports the following property types:
1. House
2. Apartment
3. Condo
4. Townhouse
5. Villa
6. Studio
7. Duplex
8. Commercial
9. Land

## User Roles

- **House Seeker**: Can search and view properties
- **Broker**: Can create, update, and delete their own properties
- **Admin**: System administration (can be extended)

## Logging

The application uses Serilog for structured logging:
- Console output for development
- File logging with daily rotation
- Configurable log levels
- Request/response logging for debugging

## Error Handling

Comprehensive error handling with:
- Proper HTTP status codes
- Descriptive error messages
- Validation error details
- Structured error responses

## Testing

The solution is designed to be easily testable with:
- Dependency injection throughout
- Separated concerns
- Interface-based design
- Mock-friendly architecture
