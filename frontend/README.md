# EMedicine – screenshot-style full-stack demo

React (Vite) frontend, ASP.NET Core 8 Web API, EF Core and SQL Server. Includes login, registration, admin medicine CRUD, customer listing, admin-managed demo funds, order statuses, profile, product cards, cart, checkout and order history. UI follows the supplied screenshots; product pictures are user-provided URLs (no copyrighted sample product pictures bundled).

## Requirements
.NET 8 SDK, SQL Server Express or SQL Server, Node.js 18+, Visual Studio/VS Code.

## Database + API
1. Edit `backend/appsettings.json` connection string to match your SQL Server. Replace the example JWT key with a random secret at least 32 characters long (keep private).
2. In `backend` run `dotnet restore`. Install EF CLI: `dotnet tool install --global dotnet-ef --version 8.*` (if not installed).
3. Run `dotnet ef migrations add InitialCreate` then `dotnet ef database update`.
4. Create an admin securely with environment variables **before first start**. In PowerShell:
   `$env:EMED_ADMIN_EMAIL='admin@example.com'`
   `$env:EMED_ADMIN_PASSWORD='ChooseYourOwnLongSecurePassword!'`
   `dotnet run --urls http://localhost:5000`
   The app only creates this admin when email is not already registered. Do not share the password or commit it to source control.
5. API docs: http://localhost:5000/swagger

## React
From `frontend` run `npm install` and `npm run dev -- --port 3000`. Open http://localhost:3000. The default API URL is http://localhost:5000. Set `VITE_API_URL` in a local `.env` to change it. CORS allows ports 3000 and 5173.

## Notes
- Registration always creates a User, not an Admin. Passwords are hashed and auth uses JWT. Admin-only endpoints require the Admin role.
- `Manage Funds` is a **demo balance** field; it does not process money and checkout does not deduct it.
- Image field uses a public image URL, not file uploads.
- No Facebook/Twitter login, forgot-password delivery, or real payment integration.
- Screenshots showed plaintext passwords in tables; this version intentionally **never displays or stores plaintext passwords**.
- This is a learning/demo project, not suitable for real medicine sales, patient data, or regulated pharmacy operations.
- Source has not been compiled in this environment; install dependencies and test locally.
