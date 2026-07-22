# Flujo de Cervecerías (`BreweryController`)

El controlador `BreweryController` administra la consulta de cervecerías y las cervezas producidas por cada establecimiento.

## Endpoints Disponibles

* `GET /api/v1/Brewery/{id}/beers` - Obtiene el listado completo de cervezas asociadas a una cervecería específica.

## Diagrama de Secuencia

```mermaid
sequenceDiagram
    participant Cliente
    participant BreweryController
    participant Mediator
    participant QueryHandler
    participant Repository

    Cliente->>BreweryController: GET /api/v1/Brewery/{id}/beers
    BreweryController->>Mediator: Send(GetBeersByBreweryIdQuery)
    Mediator->>QueryHandler: Handle(GetBeersByBreweryIdQuery)
    QueryHandler->>Repository: GetBeersByBreweryIdAsync(id)
    Repository-->>QueryHandler: List<Beer>
    QueryHandler-->>Mediator: List<BeerDto>
    Mediator-->>BreweryController: Result
    BreweryController-->>Cliente: 200 OK (Lista de Cervezas)
```

## Flujo de Consulta

1. El cliente consulta las cervezas de una cervecería mediante su ID (GUID).
2. Se envía la consulta `GetBeersByBreweryIdQuery` a través de **MediatR**.
3. El manejador realiza la búsqueda en el repositorio y retorna la lista mapeada a DTOs de respuesta.
