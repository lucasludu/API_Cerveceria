# Documentación Técnica - Beer & Wholesale Management API

Esta API ha sido diseñada como una solución robusta y escalable para la gestión de la cadena de suministro de cerveza, permitiendo la interacción entre cervecerías, mayoristas y procesos de venta/cotización.

## 1. Resumen de la API

La API facilita la administración del ecosistema de distribución de cerveza. Sus capacidades principales incluyen:

* **Gestión de Cervecerías y Productos:** CRUD completo de cervecerías y sus respectivos catálogos de cervezas.
* **Control de Inventario Mayorista:** Gestión de existencias de diferentes cervezas por cada mayorista.
* **Proceso de Ventas:** Registro de ventas de cerveza a través de mayoristas.
* **Sistema de Cotizaciones (Quotes):** Motor de cálculo para solicitudes de presupuesto, permitiendo a los clientes obtener precios basados en volumen y disponibilidad.
* **Seguridad y Autenticación:** Sistema de gestión de identidad (ASP.NET Core Identity) con autenticación basada en JWT (JSON Web Tokens) y autorización por roles.

---

## 2. Arquitectura Utilizada

El proyecto implementa una **Arquitectura Limpia (Clean Architecture)** u **Onion Architecture**, organizada en las siguientes capas:

* **Domain (Dominio):** Contiene las entidades base (`User`, `Order`, `OrderDetail`, `QuoteRequest`), interfaces base y lógica de negocio pura. No tiene dependencias externas.
* **Application (Aplicación):** Define la lógica de negocio, interfaces de servicios, DTOs y casos de uso. Implementa el patrón **CQRS** para separar las operaciones de lectura y escritura.
* **Persistence (Persistencia):** Implementación del acceso a datos mediante Entity Framework Core, configuraciones de base de datos (Fluent API), repositorios y migraciones.
* **Shared (Infraestructura Compartida):** Servicios transversales como la gestión de fechas (`DateTimeService`) y utilitarios comunes.
* **WebApi (Presentación):** Capa de entrada que expone los endpoints REST, gestiona el middleware de excepciones y la configuración de Swagger.

---

## 3. Patrones de Diseño Identificados

Se han aplicado múltiples patrones para garantizar la mantenibilidad y desacoplamiento:

1. **Mediator Pattern:** Utilizado a través de la librería `MediatR` para desacoplar los controladores de la lógica de negocio. Cada petición es un Command o Query procesado por su respectivo Handler.
2. **Repository Pattern:** Abstracción del acceso a datos mediante `IRepositoryAsync`, permitiendo que la capa de aplicación no dependa directamente de la tecnología de persistencia.
3. **CQRS (Command Query Responsibility Segregation):** Separación clara entre comandos que mutan el estado (`CreateBeerCommand`) y consultas que solo leen datos (`GetBeersByBreweryIdQuery`).
4. **Wrapper/Envelope Pattern:** Uso de las clases `Response<T>` y `PagedResponse<T>` para estandarizar las respuestas de la API, incluyendo mensajes de error y metadata.
5. **Fluent Validation:** Implementación de reglas de validación de forma externa a las entidades y comandos, mejorando la legibilidad.
6. **Dependency Injection:** Inversión de control gestionada nativamente por .NET para la resolución de dependencias entre capas.

---

## 4. Principios SOLID Aplicados

* **S - Single Responsibility Principle:** Cada Handler en la capa de aplicación se encarga de un único caso de uso. El middleware de errores (`ErrorHandlerMiddleware`) tiene la única función de centralizar excepciones.
* **O - Open/Closed Principle:** El uso de `IPipelineBehavior` para validaciones permite extender el comportamiento del flujo de las peticiones sin modificar los Handlers existentes.
* **L - Liskov Substitution Principle:** Las entidades heredan de `BaseEntity` y los repositorios implementan interfaces genéricas, asegurando que las subclases puedan ser tratadas como sus tipos base sin romper la funcionalidad.
* **I - Interface Segregation Principle:** Interfaces específicas como `IAuthService` o `IDateTimeService` aseguran que los consumidores solo dependan de los métodos que realmente necesitan.
* **D - Dependency Inversion Principle:** La capa de aplicación depende de interfaces (`IApplicationDbContext`, `IRepositoryAsync`), mientras que las implementaciones concretas se inyectan desde la capa de persistencia.

---

## 5. Otras Mejores Prácticas

* **Caché Distribuida:** Integración de Redis (`StackExchange.Redis`) y Decoradores para optimizar la velocidad de lectura en catálogos y disminuir la carga en la DB.
* **Resiliencia y Tolerancia a Fallos:** Configuración de políticas de reintento (`Retry`) y Timeout con la librería **Polly** para llamadas HTTP y bases de datos.
* **Observabilidad Avanzada:** Instrumentación con **OpenTelemetry** (OTLP), métricas y **Serilog** para logs estructurados y tiempos de respuesta.
* **Health Checks:** Monitorización continua a través de `/health` verificando conectividad a SQL Server y Redis.
* **Pruebas de Integración:** Entorno robusto E2E utilizando `WebApplicationFactory` y bases de datos en memoria con **xUnit**.
* **Global Exception Handling:** Uso de un Middleware personalizado para capturar errores y devolver códigos de estado HTTP apropiados (400, 404, 500) en un formato JSON consistente.
* **AutoMapper:** Mapeo automático entre Entidades de Dominio y DTOs para evitar el código "boilerplate" y proteger el modelo de dominio.
* **Pipeline Behaviors:** Validación automática de modelos antes de que lleguen al Handler mediante `ValidationBehaviour` y registro de métricas de tiempo de ejecución con `LoggingBehavior`.
* **Identity Framework:** Uso de estándares de la industria para la gestión de usuarios, contraseñas y roles, incluyendo Refresh Tokens (JWT) y Control de Acceso (RBAC).
* **Documentación con Swagger:** Configuración avanzada de Swagger para incluir soporte de seguridad JWT y versionamiento de API.
* **Paginación de API:** Parámetros unificados en los Handlers para devolver respuestas paginadas (`PagedResponse`) controlando el consumo de memoria en consultas extensas.

---
*Documento generado por el Arquitecto de Software a cargo del proyecto.*
