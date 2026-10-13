# EMedicine — Full-stack educational e-commerce project

React + Vite frontend, ASP.NET Core 8 REST API, Entity Framework Core, SQL Server, JWT authentication.

## Features
- Customer registration, login, profile updates
- Browse and search medicines, cart, checkout (demo only), order history
- Admin: create/edit/deactivate medicines, view customers, manage orders, dashboard counts and revenue
- Role-based authorization, hashed passwords, database relations, stock updates during checkout

## Requirements
- .NET 8 SDK (Windows 10 compatible)
- Node.js 20 LTS or 22 LTS and npm
- SQL Server Express or Developer Edition (local SQL Server)
- Visual Studio 2022 or VS Code

## Setup (Windows)
1. Open `backend/appsettings.json`. Change `ConnectionStrings:Default` to your SQL Server connection string. For SQL Express often: `Server=localhost\SQLEXPRESS;Database=EMedicine;Trusted_Connection=True;TrustServerCertificate=True;`.
2. **Replace `Jwt:Key` with a long unpredictable secret (32+ characters)**. Never commit a production secret. Set a different secret in production via environment variables / user secrets.
3. In a terminal:
   ```powershell
   cd backend
   dotnet restore
   dotnet tool install --global dotnet-ef --version 8.0.11
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   dotnet run --urls http://localhost:5000
   ```
4. Open a second terminal:
   ```powershell
   cd frontend
   npm install
   npm run dev
   ```
5. Open http://localhost:5173. API docs: http://localhost:5000/swagger.

## Create an admin (local development only)
Register a normal account through the website first. Then execute in SQL Server Management Studio against the EMedicine database:
```sql
UPDATE Users SET Role = 'Admin' WHERE Email = 'your-own-email@example.com';
```
Log out and log back in to receive an admin JWT. Never expose this SQL operation in the public API.

## Add example medicine (admin panel)
After admin login, open Manage Medicines and enter name, manufacturer, price, discount percentage (0-100), quantity, expiry date, etc. Only medicines with active status, stock and future expiry appear in the storefront.

## Important limitations
This is a learning/portfolio demonstration, **not** a production pharmacy. There is no real payment gateway, prescription handling, identity verification, delivery, refunds, tax calculation, audit logging, rate limiting, or regulated pharmacy compliance. Orders are demo orders without payment. Do not use this application to dispense or sell medicine. The frontend stores its JWT in sessionStorage for demo convenience; a production deployment needs stronger session security. Uploading images is not implemented: admin enters an image URL. No demo admin password is embedded.

## Database entities
Users, Medicines, Cart, Orders, OrderItems. Order items preserve price/name snapshots. Discounts are percentage values. Amounts are decimal in SQL Server.
