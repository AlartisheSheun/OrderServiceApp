# 📦 Order API

A modern, production-ready REST API for managing orders. Built with .NET 8 and PostgreSQL, featuring clean architecture, comprehensive validation, and API documentation.

---

---

## ✨ Key Features

| Feature | Details |
|---------|---------|
| 🔌 **RESTful API** | Full CRUD operations with proper HTTP semantics |
| 📊 **PostgreSQL** | Enterprise-grade database with Entity Framework Core ORM |
| 📚 **Swagger/OpenAPI** | Interactive API documentation and testing |
| ✅ **Validation** | Automatic input validation with FluentValidation |
| 🏗️ **Clean Architecture** | Repository and Service layers for maintainability |
| 🔧 **Dependency Injection** | Loose coupling for testability and flexibility |
| 🛡️ **Error Handling** | Proper HTTP status codes and error responses |
| 📋 **DTOs** | Clean API contracts with Data Transfer Objects |

---

## 📋 System Requirements

| Requirement | Version |
|------------|---------|
| **.NET SDK** | 8.0 or later |
| **PostgreSQL** | 12 or later |
| **Operating System** | Windows, Linux, or macOS |
| **IDE (Optional)** | VS Code, Visual Studio, or JetBrains Rider |

---

## 🏃 Get Started Locally

### Step 1: Clone & Install Dependencies

```bash
# Clone the repository
git clone <your-repo-url>
cd OrderAPI

# Restore NuGet packages
dotnet restore
```

### Step 2: Configure Database Connection

```bash
# Copy example environment file
cp .env.example .env

# Edit .env with your database details
# (For local development, defaults will work)
```

**Default local configuration** (in `.env`):
```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=OrderDb
DB_USER=postgres
DB_PASSWORD=postgres
```

### Step 3: Create Database & Apply Migrations

```bash
# Apply Entity Framework migrations
dotnet ef database update
```

This automatically creates:
- PostgreSQL database `OrderDb`
- Required tables and schema

### Step 4: Run the Application

```bash
dotnet run
```

**API is now available at:**
- 🌐 Swagger UI: **http://localhost:5000** (or http://localhost:5000/swagger)
- 📡 API Base: **http://localhost:5000/api**
- 🔒 HTTPS: **https://localhost:44394** (if configured)

---

## 📡 API Endpoints

### Orders Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| **GET** | `/api/orders` | Get all orders |
| **GET** | `/api/orders/{id}` | Get specific order |
| **POST** | `/api/orders` | Create new order |
| **PUT** | `/api/orders/{id}` | Update order |
| **DELETE** | `/api/orders/{id}` | Delete order |

### Example Requests

**Create an Order:**
```bash
POST /api/orders
Content-Type: application/json

{
  "customerName": "John Doe",
  "amount": 99.99,
  "status": "Pending",
  "description": "Order description here"
}
```

**Get All Orders:**
```bash
GET /api/orders
```

---

## 📁 Project Structure

```
OrderAPI/
├── Controllers/              # 🔌 API endpoints
│   └── OrdersController.cs
├── Data/                     # 📊 Database
│   ├── OrderDbContext.cs
│   ├── OrderDbContextFactory.cs
│   └── Migrations/
├── DTOs/                     # 📋 Data Transfer Objects
│   └── OrderDtos.cs
├── Models/                   # 🏗️ Domain Models
│   └── Order.cs
├── Repositories/             # 📚 Data Access Layer
│   └── OrderRepository.cs
├── Services/                 # ⚙️ Business Logic
│   └── OrderService.cs
├── Validators/               # ✅ Input Validation
│   └── OrderValidators.cs
├── Program.cs                # 🚀 Application Setup
├── render.yaml               # ☁️ Render Deployment Config
├── .env.example              # 📝 Environment Template
├── appsettings.json          # ⚙️ App Configuration
└── OrderAPI.csproj           # 📦 Project File
```

---

## 🌍 Environment Variables

### Local Development

Create `.env` file in project root:

```env
# Database Configuration
DB_HOST=localhost
DB_PORT=5432
DB_NAME=OrderDb
DB_USER=postgres
DB_PASSWORD=your_password_here

# Application Settings
API_PORT=5000
```

---

## 🔧 Useful Commands

### Development

```bash
# Build project in Release mode
dotnet build -c Release

# Apply migrations
dotnet ef database update

# Create new migration
dotnet ef migrations add {MigrationName}

# List all migrations
dotnet ef migrations list

# Revert to previous migration
dotnet ef database update {PreviousMigrationName}
```

### Testing

```bash
# Access Swagger UI for testing
# Go to: http://localhost:5000/swagger

# Or use curl
curl http://localhost:5000/api/orders
```

### Database

```bash
# Connect to PostgreSQL (after psql is installed)
psql -U postgres -d OrderDb

# Drop and recreate database
psql -U postgres -c "DROP DATABASE IF EXISTS OrderDb;"
dotnet ef database update
```

---

### Technologies Used

- **[.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)** - Framework
- **[Entity Framework Core](https://learn.microsoft.com/efcore/)** - ORM
- **[PostgreSQL](https://www.postgresql.org/)** - Database
- **[Swagger/OpenAPI](https://swagger.io/)** - API Documentation
- **[FluentValidation](https://fluentvalidation.net/)** - Input Validation

---

## 🛠️ Order Model & Validation

### API Request Format

```json
{
  "customerName": "John Doe",
  "amount": 99.99,
  "status": "Pending",
  "description": "Optional order description"
}
```

### Validation Rules

| Field | Rules |
|-------|-------|
| **customerName** | Required, max 100 characters |
| **amount** | Required, must be > 0 |
| **status** | Optional, must be: Pending, Completed, or Cancelled |
| **description** | Optional, free text |

### Database Schema

```sql
CREATE TABLE "Orders" (
  "Id" SERIAL PRIMARY KEY,
  "CustomerName" VARCHAR(100) NOT NULL,
  "Amount" DECIMAL(18,2) NOT NULL,
  "Status" VARCHAR(50),
  "Description" TEXT,
  "CreatedAt" TIMESTAMP,
  "UpdatedAt" TIMESTAMP
);
```

---

## 📊 HTTP Status Codes

| Status | Meaning | When Used |
|--------|---------|-----------|
| **200** | OK | Successful GET/PUT request |
| **201** | Created | Successful POST (new resource) |
| **400** | Bad Request | Validation error in request |
| **404** | Not Found | Resource doesn't exist |
| **500** | Server Error | Internal application error |

---

