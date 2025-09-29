# MillionRealEstatecompany.API Test Suite

## Tecnologías y Herramientas

### Testing Framework
- **NUnit 3.14.0**: Framework de pruebas unitarias para .NET
- **FluentAssertions 6.12.0**: Biblioteca para assertions más legibles y expresivos
- **Moq 4.20.70**: Framework de mocking para crear objetos simulados

### Testing Infrastructure
- **Microsoft.AspNetCore.Mvc.Testing 8.0.8**: Para pruebas de integración de ASP.NET Core
- **Microsoft.EntityFrameworkCore.InMemory 8.0.8**: Base de datos en memoria para testing
- **AutoMapper 12.0.1**: Para testing de mapeos entre DTOs y entidades
- **Coverlet 6.0.0**: Para generación de reportes de cobertura

## Cobertura de Código (Code Coverage)

### Generar Reporte de Cobertura 🎯

**Comando único y simple:**

```bash
# Ejecutar desde el directorio raíz del proyecto
coverage.bat
```

**¿Qué hace el script?**
- ✅ Limpia archivos anteriores de cobertura
- ✅ Ejecuta todas las pruebas unitarias e integración
- ✅ Genera cobertura excluyendo Models/DTOs/Migrations
- ✅ Crea reporte HTML profesional  
- ✅ Abre automáticamente el reporte en el navegador

### Configuración de Exclusiones ⚙️

El proyecto está configurado para excluir del reporte de cobertura:
- **Models**: Clases de entidades (POCOs) sin lógica de negocio
- **DTOs**: Data Transfer Objects (solo propiedades)
- **Migrations**: Scripts de migración de base de datos generados automáticamente
- **Archivos de test**: Proyectos `*.Test` y `*.Tests`
- **Código generado**: Clases marcadas con atributos de código generado

### Archivos de Configuración 📋

- **`.csproj`**: Configuración automática de exclusiones de cobertura
- **`coverage.bat`**: Script único para generar reporte de cobertura
- **`.gitignore`**: Configurado para NO subir archivos de cobertura al repositorio

### Comandos Alternativos

#### Ver cobertura rápida en consola:
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

#### Comandos manuales:
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
reportgenerator -reports:"TestResults\*\coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html
start CoverageReport\index.html
```

Los archivos de cobertura **NO se suben al repositorio** gracias a la configuración en `.gitignore`.

## Arquitectura de Testing

### Principios Implementados
- **Clean Code**: Código de pruebas limpio, legible y mantenible
- **TDD**: Desarrollo guiado por pruebas con ciclos Red-Green-Refactor
- **SOLID Principles**: Aplicación de principios SOLID en el diseño de pruebas
- **DevOps Best Practices**: Integración continua y automatización de pruebas

## Patrones y Técnicas

### 1. Arrange-Act-Assert (AAA Pattern)
```csharp
// Arrange
var createDto = new CreatePropertyDto { /* datos */ };

// Act  
var result = await service.CreatePropertyAsync(createDto);

// Assert
result.Should().NotBeNull();
```

### 2. Dependency Injection con Mocking
```csharp
private Mock<IUnitOfWork> _mockUnitOfWork;
private Mock<IPropertyRepository> _mockPropertyRepository;
private PropertyService _propertyService;
```

### 3. Tipos de Pruebas Implementadas
- **Unit Tests**: Pruebas unitarias con mocks para aislamiento
- **Integration Tests**: Pruebas end-to-end con WebApplicationFactory
- **Repository Tests**: Pruebas de acceso a datos con base de datos en memoria
- **Controller Tests**: Validación de endpoints HTTP y respuestas

## Comandos para Ejecutar Pruebas

### Todas las pruebas
```bash
dotnet test
```

### Solo pruebas unitarias
```bash
dotnet test --filter "ClassName!~IntegrationTests"
```

### Con verbosidad detallada
```bash
dotnet test --verbosity normal
```