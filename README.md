# 📦 ECommerce Orders API

[![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)](https://www.mysql.com/)
[![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef/)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)

---

## 📖 Overview

The **ECommerce Orders API** is a microservice for managing customer orders in an e‑commerce system. It’s built on **ASP.NET Core 8**, uses **Entity Framework Core** with **MySQL**, and exposes CRUD endpoints for orders (and their items if enabled). It includes validation, mapping, and centralized exception handling. Swagger UI is enabled for API exploration.

> **Note on routes:** Some setups use REST‑style routes like `/api/orders/{id}`, while others use action‑based routing via `[Route("api/[controller]/[action]")]` (for example: `/api/orders/get/{id}`). **Always prefer the live Swagger UI as the source of truth for the exact endpoints in your build.**

---

## 🗂️ Solution structure

```
ECommerceOrders.API.sln
├── ECommerceOrders.API      # ASP.NET Core Web API (presentation layer)
├── BusinessLogicLayer       # Domain entities, DTOs, services, validators, mapping
└── DataAccessLayer          # EF Core DbContext, entity configs, migrations (MySQL)
```

---

## 🛠️ Technology stack

- .NET 8 / ASP.NET Core Web API  
- **MySQL** (relational database)  
- **Entity Framework Core** (ORM, migrations) — typically via **Pomelo.EntityFrameworkCore.MySql** provider  
- AutoMapper (DTO ↔ entity mapping) *(if included in your solution)*  
- FluentValidation *(or similar)* for input validation *(if included)*  
- Centralized exception‑handling middleware  
- Swagger (Swashbuckle)  
- Docker (multi‑stage build)

---

## 🧩 Domain model (example)

> Your exact entities may differ; adjust names/fields as per your code.

```csharp
public class Order
{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerID { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
```

### ✅ Validation guidelines
- `CustomerID` → required  
- `Items` → non‑empty collection  
- For each `OrderItem`: `ProductID` required, `Quantity` ≥ 1, `UnitPrice` ≥ 0  
- `TotalAmount` → computed/verified to match sum of item totals

---

## 📦 DTOs (example)

```csharp
public record OrderDto(
    int OrderID,
    DateTime OrderDate,
    int CustomerID,
    decimal TotalAmount,
    OrderStatus Status,
    ICollection<OrderItemDto> Items);

public record OrderItemDto(
    int OrderItemID,
    int ProductID,
    int Quantity,
    decimal UnitPrice);

public record CreateOrderRequest(
    int CustomerID,
    ICollection<CreateOrderItemRequest> Items);

public record CreateOrderItemRequest(
    int ProductID,
    int Quantity,
    decimal UnitPrice);

public record UpdateOrderStatusRequest(
    OrderStatus Status);
```

---

## 🌐 API endpoints (check Swagger for the authoritative list)

> If your controller uses REST‑style routing (`[Route("api/[controller]")]`), endpoints will look like this:

| Method | Route                   | Description            | Body                         | Response |
|--------|-------------------------|------------------------|------------------------------|----------|
| GET    | `/api/orders`           | List orders            | —                            | `200 OK` `OrderDto[]` |
| GET    | `/api/orders/{id}`      | Get order by ID        | —                            | `200 OK` `OrderDto` or `404` |
| POST   | `/api/orders`           | Create order           | `CreateOrderRequest`         | `201 Created` `OrderDto` |
| PUT    | `/api/orders/{id}/status` | Update order status  | `UpdateOrderStatusRequest`   | `200 OK` `OrderDto` or `404` |
| DELETE | `/api/orders/{id}`      | Delete (or cancel)     | —                            | `204 No Content` or `404` |

> If your controller uses action‑based routing (`[Route("api/[controller]/[action]")]`), typical actions map like:  
> • `GET /api/orders/getall` • `GET /api/orders/get/{id}` • `POST /api/orders/create` • `PUT /api/orders/updatestatus/{id}` • `DELETE /api/orders/delete/{id}`.  
> **Your Swagger UI reflects the exact form used in this repo.**

---

## 🔍 Example requests

**Create**
```bash
curl -X POST http://localhost:5000/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerID": 12345,
    "items": [
      { "productID": 501, "quantity": 2, "unitPrice": 399.90 },
      { "productID": 777, "quantity": 1, "unitPrice": 1299.00 }
    ]
  }'
```

**Update status**
```bash
curl -X PUT http://localhost:5000/api/orders/1/status \
  -H "Content-Type: application/json" \
  -d '{ "status": "Processing" }'
```

**Get by ID**
```bash
curl http://localhost:5000/api/orders/1
```

---

## 🗄️ Database setup (MySQL + EF Core)

### Connection string (environment variable)
```bash
export ConnectionStrings__MySQL="server=localhost;port=3306;database=ECommerceOrders;user=root;password=yourpassword"
```

### EF Core provider registration (example)
```csharp
// Program.cs
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySQL"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQL"))
    ));
```

### Applying migrations
If your solution keeps the DbContext and migrations in `DataAccessLayer`:
```bash
# from the solution root
dotnet tool install --global dotnet-ef

# add an initial migration (adjust project names if different)
dotnet ef migrations add InitialCreate \
  --project DataAccessLayer \
  --startup-project ECommerceOrders.API

# apply to the database
dotnet ef database update \
  --project DataAccessLayer \
  --startup-project ECommerceOrders.API
```

> If migrations live in another project/folder, adjust `--project` accordingly.

---

## 🐳 Docker (API) + local MySQL

**Build & run API**
```bash
docker build -t ecommerce-orders-api -f ECommerceOrders.API/Dockerfile .

docker run --rm -p 8080:8080 -p 8081:8081 \
  -e ConnectionStrings__MySQL="server=host.docker.internal;port=3306;database=ECommerceOrders;user=root;password=yourpassword" \
  ecommerce-orders-api
```

**Quick MySQL container (optional)**
```bash
docker run --name mysql-orders -e MYSQL_ROOT_PASSWORD=yourpassword \
  -e MYSQL_DATABASE=ECommerceOrders -p 3306:3306 -d mysql:8
```

> Use `host.docker.internal` when the DB runs on your host and the API runs in Docker.

---

## 🚀 Running locally (without Docker)
```bash
dotnet restore ECommerceOrders.API.sln
dotnet run --project ECommerceOrders.API/ECommerceOrders.API.csproj
```

Swagger UI:  
👉 https://localhost:5001/swagger

---

## 🏗️ Architecture highlights
- **ECommerceOrders.API** → Controllers, Swagger, exception handling, CORS  
- **BusinessLogicLayer** → Entities, DTOs, mapping profiles, validators, services/use‑cases  
- **DataAccessLayer** → EF Core `DbContext`, entity configurations, repositories (if any), migrations  

---

## ⚠️ Error handling
- Validation errors → `400 Bad Request` with details  
- Not found (order or item) → `404 Not Found`  
- Unhandled exceptions → centralized middleware returns JSON `{ "message", "type" }`

**Example error**
```json
{
  "message": "Order not found",
  "type": "System.Exception"
}
```

---

## 🔮 Extensibility
- Add paging & filtering (by `CustomerID`, `Status`, date range)  
- Support soft deletion or cancellation workflow  
- Webhooks/domain events for status changes  
- Unit & integration tests; consider CQRS if read/write needs diverge  
- API versioning

---

## 🛠️ Troubleshooting
- Ensure MySQL is reachable and credentials are correct  
- Run EF Core migrations before starting the API  
- Adjust CORS for your front‑end origin in `Program.cs`  
- In Docker, use the correct host/ports for MySQL

---

## 📜 License
This project currently has no explicit license.  
Please add one before distributing or hosting publicly.
