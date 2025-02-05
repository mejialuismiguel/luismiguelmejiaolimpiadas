# AthleteApi

AthleteApi es una API ASP.NET Core para gestionar atletas utilizando un procedimiento almacenado en SQL Server. La API incluye autenticación con token JWT y documentación generada por Swagger.

## Requisitos

- .NET 6.0 o superior
- SQL Server 2022
- Visual Studio Code (opcional)

## Estructura del Proyecto

```plaintext
AthleteApi/
├── AthleteApi/
│   ├── Controllers/
│   │   └── AthleteController.cs
│   ├── Data/
│   │   └── AthleteContext.cs
│   ├── Models/
│   │   └── Athlete.cs
│   ├── Services/
│   │   └── AthleteService.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── Startup.cs
├── AthleteApi.sln
└── README.md