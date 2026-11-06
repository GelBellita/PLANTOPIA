# 🌿 Plantopia

Plantopia is an online plant shop built with ASP.NET Core MVC. Customers can browse indoor, outdoor, succulent and hanging plants, save favorites, manage a cart, check out and track their orders.

## Features

**Shopping**
- Browse plants by category: Indoor, Outdoor, Succulents and Hanging Plants
- Search, plus Best Sellers, New Arrivals and Sale pages with sorting
- Plant Care guide page
- Wishlist (add or remove with one click)
- Cart with quantity updates and a live cart count
- Checkout for selected cart items, with order placement and cancellation

**Accounts**
- Register and log in with cookie authentication. Passwords are hashed with BCrypt.
- Edit profile and upload a profile photo
- Change password
- Manage saved addresses and set a default
- Purchase history with order status
- Notifications with unread count and mark-as-read

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | C#, ASP.NET Core MVC (.NET 10) |
| Database | SQL Server (LocalDB), Entity Framework Core 10 |
| Auth | Cookie authentication, sessions, BCrypt.Net |
| Frontend | Razor views, HTML, CSS, JavaScript, Bootstrap, jQuery |

## Project Structure

```
Plantopia/
├── Controllers/   Home, Store, Account, Auth, Notification
├── Data/          AppDbContext (EF Core model and plant seed data)
├── Migrations/    EF Core migrations
├── Models/        Entities (User, Plant, Order, CartItem, ...) and view models
├── Views/         Razor views for each controller, plus shared layouts
└── wwwroot/       CSS, JavaScript, images, client libraries
```

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB. It comes with Visual Studio's "ASP.NET and web development" workload.
- EF Core CLI tools: `dotnet tool install --global dotnet-ef`

### Run locally

1. Clone the repository:
   ```bash
   git clone https://github.com/GelBellita/PLANTOPIA.git
   cd PLANTOPIA/Plantopia
   ```
2. Check the connection string in `appsettings.json`. By default it uses LocalDB:
   ```json
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PlantopiDB;TrustServerCertificate=True;"
   ```
3. Create the database and load the plant catalog:
   ```bash
   dotnet ef database update
   ```
4. Run the app:
   ```bash
   dotnet run
   ```
   Then open `https://localhost:7027` or `http://localhost:5184`.

You can also open `Plantopia.slnx` in Visual Studio and press **F5**. Run `Update-Database` in the Package Manager Console first.

## Author

**Engel B. Bellita**, BSIT student at the University of Cebu – Main Campus
GitHub: [@GelBellita](https://github.com/GelBellita)
