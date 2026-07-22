# Brewery Management API

Welcome to the Brewery Management API! This project was built to satisfy the requirements of a coding challenge for a junior backend developer role.

## 🎯 Funcionalidad y Objetivo del Proyecto

**Objetivo del Proyecto**
El objetivo principal de esta API es gestionar de manera centralizada el catálogo, inventario y las transacciones comerciales entre **Cervecerías** (productores), **Mayoristas** (distribuidores) y **Clientes** finales. El sistema asegura que las reglas de negocio, como la aplicación de descuentos por volumen y la verificación de stock, se ejecuten de manera precisa y consistente.

**¿Para qué sirve?**
La API funciona como el motor principal (*backend*) de una plataforma de distribución B2B (Business-to-Business). Permite:

1. A las **Cervecerías** registrar y gestionar su catálogo de cervezas.
2. A los **Mayoristas** administrar su inventario (qué cervezas venden y cuántas unidades tienen en stock).
3. A los **Clientes** (como bares, restaurantes o tiendas) solicitar cotizaciones de compra, donde el sistema calcula automáticamente el precio total, valida el stock disponible y aplica descuentos dinámicos según el volumen de compra.

### Principales Funcionalidades

* **Gestión de Catálogo**: Endpoints para operaciones CRUD de Cervecerías y Cervezas.
* **Manejo de Inventario**: Endpoint para registrar el reabastecimiento de cervezas a un Mayorista específico.
* **Cotizador Inteligente**: Un motor de cotizaciones que procesa pedidos validando que no haya productos duplicados, asegurando la existencia de stock, y aplicando un **10% de descuento** en compras de más de 10 unidades, o un **20% de descuento** si se superan las 20 unidades.

## 🧠 Thought Process

When approaching this challenge, I decided to use a **Clean Architecture** approach based on .NET 10 to ensure separation of concerns, high maintainability, and testability.

1. **Domain Driven Design**: I started by carefully reading the business requirements to extract the Core Entities: `Brewery`, `Beer`, `Wholesaler`, and the relationship between them via `WholesaleInventory`.
2. **Persistence Strategy**: I configured Entity Framework Core with Fluent API (instead of Data Annotations) to configure the keys, constraints, and relationships. To facilitate development and satisfy the challenge requirements without migrations, I utilized `EnsureDeleted` and `EnsureCreated` on startup, alongside `HasData` seeding methods in the entity configurations.
3. **CQRS Pattern**: I utilized the `MediatR` library to implement the Command Query Responsibility Segregation (CQRS) pattern. This keeps my API Controllers extremely lightweight and pushes all business logic and validations into dedicated Handler classes.
    * **Queries**: For reading data (e.g., getting all beers by a brewery).
    * **Commands**: For operations that change state (ajouter, update, delete, quotes, sales).
4. **Testing Strategy**: A critical part of the thought process was ensuring correctness.
    * **Unit Tests**: I implemented an `InMemory` EF Core provider to unit test the business constraints of the `RequestQuoteCommand` and `AddBeerSaleCommand` perfectly without touching a real database.
    * **Integration Tests**: I used an `SQLite` provider explicitly requested by the test requirements. The `BeerIntegrationTests` class spins up and deletes a unique `.db` file for each test (`EnsureCreated` and `EnsureDeleted`), guaranteeing isolation.

## 🚀 Challenges Encountered

* **Handling `BaseEntity` with `Guid`**: The template I started with defaulted to `Guid` for primary keys. I adapted all my entity structures to embrace `Guid` perfectly, parsing them during EF Data Seeding.
* **EF Core ChangeTracker behavior**: During Unit Tests, configuring tracking strategies was a bit tedious with the `InMemory` database when calling `Update`. I resolved the tracking conflicts by carefully clearing the change tracker (`_context.ChangeTracker.Clear()`) between Arrange and Act phases of my tests to emulate a realistic disconnected HTTP Context execution.
* **Dynamic Currency and Summarization**: Validating string outputs that include formatted currency outputs varied by culture. I ensured the mathematical logic of the discounts (10% or 20%) was tested directly alongside string assertions.

## 🛠️ How to run the app

### Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* SQL Server LocalDB (installed with Visual Studio, or available as a separate download, check connection string in `WebApi/appsettings.json` if you use a different SQL Server instance).

### 1. Configure the API Connection String

Open `WebApi/appsettings.json` and ensure the `DefaultConnection` points to a reachable SQL Server (e.g. `Server=(localdb)\\mssqllocaldb;Database=API_Cerveceria;Trusted_Connection=True;`). The current string tries to connect to a specific DESKTOP instance, please adjust if necessary.

### 2. Run the application

Open a terminal in the solution folder (or the `WebApi` folder) and execute:

```bash
dotnet run --project WebApi
```

The application will automatically drop any existing DB with that name, recreate it fresh, seed it with Breweries, Beers, Wholesalers, and Inventories, and be ready to accept requests!

### 3. Test the API Endpoints

Navigate to `https://localhost:[PORT]/swagger` in your browser. You can interact with all the generated endpoints for Breweries, Beers, Wholesalers, and Quotes.

### 4. Run the Tests

To execute all Unit and Integration Tests, execute the following command from the solution root:

```bash
dotnet test
```

*Note: You will see SQLite `.db` files created and rapidly deleted in the test project folder during the Integration test execution!*
