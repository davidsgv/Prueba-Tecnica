# Prueba-Tecnica
Prueba Tecnica Angular y .Net Core

## Stack Tecnologico

- *Backend:* .NET Core 9
- *Orquestación:* .NET Aspire (Service Defaults & AppHost)
- *Base de Datos:* SQL Server (Contenerizado vía Aspire)
- *Frontend:* Angular (Node 20+)
- *Infraestructura:* Docker

## Arquitectura
El proyecto está diseñado bajo los principios de Clean Architecture y Domain-Driven Design (DDD), asegurando que la lógica de negocio sea independiente de los frameworks externos.

### Capas del Proyecto:

- Domain: El núcleo del sistema. Contiene entidades de dominio, lógica de negocio pura e interfaces (contratos) de los repositorios.
- Application: Orquestación de casos de uso, manejo de DTOs y validaciones.
- Infrastructure: Implementación de la persistencia de datos (Entity Framework Core) y servicios externos.
- API: Punto de entrada del sistema, encargado de los controladores y la configuración de dependencias.

### Patrones Aplicados:
- Dependency Inversion: Para desacoplar las capas de alto nivel de las de bajo nivel.
- Repository Pattern: Para abstraer la lógica de acceso a datos.

### Estructura de carpetas

- Domain (La capa principal que contiene entidades y repositorios)
- Application (Capa ue contiene los casos de uso y los DTOs)
- Infrastrcuture (Capa que implementa la persistencia a la base de datos e implementa los repositorios)
- API

## Ejecución del Proyecto

1. Backend (Orquestación con Aspire)
Gracias a .NET Aspire, no necesitas configurar manualmente la cadena de conexión ni levantar SQL Server de forma independiente; el AppHost se encarga de todo.

1. Abre una terminal en la raíz de la solución.
2. Ejecuta el proyecto de orquestación:

``` Bash
dotnet run --project FlashLogistic.AppHost
```

3. Se abrirá automáticamente el Aspire Dashboard. Si solicita autenticación, revisa la consola para obtener el token de acceso.
4. Desde el dashboard, podrás monitorear el estado de la API y la base de datos.

2. Frontend (Angular)

1. Navega a la carpeta del cliente:

```Bash
cd web
```

2. Instala las dependencias:

``` Bash
npm install
```

3. Configuración: Verifica en src/environments/environment.ts que la URL de la API coincida con la asignada dinámicamente por Aspire (puedes verla en la columna "Endpoints" del dashboard).

4. Inicia la aplicación:

``` Bash
npm start
```