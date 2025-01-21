# jungle-cafe-website
Final project for TIN (Internet Technologies) class at PJATK (V sem)

## Project Description
Jungle Cafe is a unique cafe experience where customers can enjoy their favorite beverages and meals while being surrounded by exotic animals. The project aims to provide an online platform for customers to view the menu, book tables, and stay updated on upcoming events. The main features of the project include user authentication, table reservations, event management, and an interactive menu.

## Setup Instructions

### Prerequisites
- Node.js (v14 or higher)
- npm (v6 or higher)
- .NET 6 SDK
- SQL Server

### Installation Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/Szafranee/jungle-cafe-website.git
   cd jungle-cafe-website
   ```

2. Install client dependencies:
   ```bash
   cd JungleCafe.Client
   npm install
   ```

3. Install server dependencies:
   ```bash
   cd ../JungleCafe.Server
   dotnet restore
   ```

4. Set up the database:
   - Create a new SQL Server database.
   - Update the connection string in `appsettings.json` with your database details.

5. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

6. Run the project locally:
   - Start the server:
     ```bash
     cd JungleCafe.Server
     dotnet run
     ```
   - Start the client:
     ```bash
     cd ../JungleCafe.Client
     npm run dev
     ```

## Usage Guidelines
- Navigate to the homepage to view the welcome section and animal gallery.
- Use the navigation bar to access different sections such as booking a table, viewing the menu, and checking out upcoming events.
- Register or log in to make reservations and access the employee panel (if you have the appropriate role).

## API Documentation
The API provides endpoints for managing animals, events, menu items, reservations, tables, and users. Below are some of the key endpoints:

- `GET /api/animals` - Retrieve a list of all animals.
  - **Request Format**: None
  - **Response Format**: JSON array of animal objects
  - **Authentication**: None
  - **Example Usage**:
    ```bash
    curl -X GET https://your-api-url/api/animals
    ```

- `POST /api/animals` - Create a new animal (requires authentication).
  - **Request Format**: JSON object with animal details
  - **Response Format**: JSON object of the created animal
  - **Authentication**: Bearer token required
  - **Example Usage**:
    ```bash
    curl -X POST https://your-api-url/api/animals -H "Authorization: Bearer <token>" -H "Content-Type: application/json" -d '{"name": "Lion", "species": "Panthera leo", "category": "Mammal", "imageUrl": "https://example.com/lion.jpg"}'
    ```

- `GET /api/events` - Retrieve a list of all events.
  - **Request Format**: None
  - **Response Format**: JSON array of event objects
  - **Authentication**: None
  - **Example Usage**:
    ```bash
    curl -X GET https://your-api-url/api/events
    ```

- `POST /api/events` - Create a new event (requires authentication).
  - **Request Format**: JSON object with event details
  - **Response Format**: JSON object of the created event
  - **Authentication**: Bearer token required
  - **Example Usage**:
    ```bash
    curl -X POST https://your-api-url/api/events -H "Authorization: Bearer <token>" -H "Content-Type: application/json" -d '{"title": "Safari Night", "description": "An exciting night safari event", "startDate": "2023-12-01T18:00:00", "endDate": "2023-12-01T21:00:00", "isPublic": true}'
    ```

- `GET /api/menu` - Retrieve the menu.
  - **Request Format**: None
  - **Response Format**: JSON array of menu item objects
  - **Authentication**: None
  - **Example Usage**:
    ```bash
    curl -X GET https://your-api-url/api/menu
    ```

- `POST /api/menu` - Create a new menu item (requires authentication).
  - **Request Format**: JSON object with menu item details
  - **Response Format**: JSON object of the created menu item
  - **Authentication**: Bearer token required
  - **Example Usage**:
    ```bash
    curl -X POST https://your-api-url/api/menu -H "Authorization: Bearer <token>" -H "Content-Type: application/json" -d '{"name": "Espresso", "category": "Beverage", "price": 2.5, "isAvailable": true}'
    ```

- `GET /api/reservations` - Retrieve a list of all reservations (requires authentication).
  - **Request Format**: None
  - **Response Format**: JSON array of reservation objects
  - **Authentication**: Bearer token required
  - **Example Usage**:
    ```bash
    curl -X GET https://your-api-url/api/reservations -H "Authorization: Bearer <token>"
    ```

- `POST /api/reservations` - Create a new reservation (requires authentication).
  - **Request Format**: JSON object with reservation details
  - **Response Format**: JSON object of the created reservation
  - **Authentication**: Bearer token required
  - **Example Usage**:
    ```bash
    curl -X POST https://your-api-url/api/reservations -H "Authorization: Bearer <token>" -H "Content-Type: application/json" -d '{"tableId": 1, "reservationDate": "2023-12-01T19:00:00", "guestCount": 4, "specialRequests": "Window seat"}'
    ```

- `GET /api/tables` - Retrieve a list of all tables.
  - **Request Format**: None
  - **Response Format**: JSON array of table objects
  - **Authentication**: None
  - **Example Usage**:
    ```bash
    curl -X GET https://your-api-url/api/tables
    ```

- `GET /api/users` - Retrieve a list of all users (requires authentication).
  - **Request Format**: None
  - **Response Format**: JSON array of user objects
  - **Authentication**: Bearer token required
  - **Example Usage**:
    ```bash
    curl -X GET https://your-api-url/api/users -H "Authorization: Bearer <token>"
    ```

## Features
- User authentication and authorization
- Table reservations
- Event management
- Interactive menu
- Animal gallery
- Employee panel

## Dependencies
- Client:
  - Svelte
  - Vite
  - Tailwind CSS
  - Svelte Routing
  - Marked
- Server:
  - .NET 6
  - Entity Framework Core
  - Microsoft IdentityModel Tokens
  - Swashbuckle (Swagger)

## Deployment Instructions
To deploy the project, follow these steps:

1. Build the client:
   ```bash
   cd JungleCafe.Client
   npm run build
   ```

2. Publish the server:
   ```bash
   cd ../JungleCafe.Server
   dotnet publish -c Release -o ./publish
   ```

3. Deploy the client and server to your preferred hosting environment (e.g., Azure, AWS, or a VPS).

## Project Architecture and Directory Structure
The project is divided into two main parts: the client and the server.

- `JungleCafe.Client`: Contains the frontend code built with Svelte.
  - `src`: Source code for the client.
    - `lib`: Contains reusable components and stores.
    - `assets`: Static assets such as images and styles.
    - `main.js`: Entry point for the client application.
- `JungleCafe.Server`: Contains the backend code built with .NET 6.
  - `Controllers`: API controllers for handling requests.
  - `Models`: Entity models and DbContext.
  - `Services`: Business logic and service classes.
  - `DTOs`: Data transfer objects.
  - `RequestModels`: Models for handling API requests.
  - `ResponseModels`: Models for handling API responses.

## Related Resources
- [Svelte Documentation](https://svelte.dev/docs)
- [Vite Documentation](https://vitejs.dev/guide/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)
