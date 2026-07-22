# Recomendaciones y Mejoras Futuras

Este documento detalla las principales oportunidades de mejora identificadas para la arquitectura, escalabilidad y mantenibilidad del sistema **API Cervecería**. Las propuestas están enfocadas en llevar el proyecto a un estándar de calidad superior y listo para producción.

## 1. Arquitectura y Rendimiento

- [x] **Caché Distribuida (Redis)**: Implementar estrategias de caché para consultas de solo lectura con alta frecuencia y baja volatilidad (ej. el catálogo de cervecerías y cervezas). Esto reducirá la carga en la base de datos y mejorará los tiempos de respuesta.
- [x] **Paginación y Filtrado Dinámico**: Modificar los endpoints que devuelven colecciones (como `GET /api/v1/User/get-all` o consultas de inventario) para que soporten parámetros de paginación (`PageSize`, `PageNumber`) y filtros, evitando la saturación de memoria.
- [x] **Resiliencia con Polly**: Integrar políticas de reintentos (*retries*), *circuit breakers* y *timeouts* en la capa de infraestructura, especialmente si la API se comunica con servicios externos o para la resiliencia de la conexión a la base de datos.

## 2. Seguridad y Autorización

- [x] **Control de Acceso Basado en Roles (RBAC)**: Reforzar la seguridad añadiendo atributos de autorización específicos (ej. `[Authorize(Roles = "Admin")]`) en endpoints sensibles como la gestión de usuarios (`UserController`) y la gestión de inventario en `WholesalerController`.
- [x] **Rotación y Refresco de Tokens**: Implementar un flujo de *Refresh Tokens* en la autenticación JWT para mejorar la experiencia del usuario manteniendo la seguridad sin forzar inicios de sesión frecuentes.

## 3. Observabilidad y Monitoreo

- [x] **OpenTelemetry y Trazabilidad**: Configurar OpenTelemetry para exportar trazas, métricas y logs a herramientas como Jaeger, Prometheus o Grafana.
- [x] **Logging Estructurado**: Integrar **Serilog** enriquecido con identificadores de correlación (*Correlation IDs*) para rastrear el flujo completo de una solicitud a lo largo de todos los manejadores de MediatR.
- [x] **Health Checks Avanzados**: Extender los controles de salud nativos de ASP.NET Core (`/health`) para verificar no solo que la API esté viva, sino también la conectividad con la base de datos SQL y Redis.

## 4. Calidad de Código y Pruebas

- [x] **Pruebas de Integración (E2E)**: Complementar las pruebas unitarias existentes en `Application.UnitTests` con pruebas de integración reales utilizando `WebApplicationFactory` y bases de datos en memoria o contenedores de Docker (Testcontainers).
- [x] **Validación Centralizada (Pipeline Behaviors)**: Asegurar que todos los comandos y consultas pasen por un comportamiento de validación de MediatR (usando FluentValidation) que detenga la ejecución y retorne un error estandarizado si la solicitud es inválida.

## 5. Experiencia de Desarrollo y Documentación

- **Manejo de Errores Estandarizado (ProblemDetails)**: Implementar un Middleware global de manejo de excepciones que formatee todos los errores según el estándar RFC 7807 (`ProblemDetails`), proporcionando una estructura consistente para el cliente.
- **Documentación OpenAPI Moderna (Scalar / Redoc)**: Reemplazar el tradicional Swagger UI por interfaces más modernas y atractivas como **Scalar** o **Redoc**, integradas con `Microsoft.AspNetCore.OpenApi`. Añadir comentarios XML detallados (`/// <summary>`) y especificar los códigos HTTP posibles (200, 400, 401, 404).
- **Automatización CI/CD**: Crear pipelines en GitHub Actions o Azure DevOps para ejecutar automáticamente linters, pruebas unitarias y generación de reportes de cobertura en cada *Pull Request*.
