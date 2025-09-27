# Million Real Estate Company API

## 📋 Descripción

API REST para la gestión de propiedades inmobiliarias desarrollada con ASP.NET Core 8.0. Implementa un sistema completo de CRUD para propiedades, propietarios, imágenes y trazabilidad de precios, siguiendo principios SOLID y patrones de diseño modernos.

## 🏗️ Arquitectura

### Patrones Implementados
- **Repository Pattern**: Abstracción de la capa de datos
- **Unit of Work**: Gestión de transacciones
- **Service Layer**: Lógica de negocio
- **Dependency Injection**: Inversión de control
- **DTO Pattern**: Separación entre modelos de dominio y API

### Tecnologías
- ASP.NET Core 8.0
- Entity Framework Core con Npgsql
- AutoMapper para mapeo de DTOs
- PostgreSQL como base de datos
- Docker & Docker Compose
- Swagger/OpenAPI para documentación

## 🚀 Inicio Rápido

### Prerrequisitos
- **Docker Desktop** (instalado y ejecutándose)
- **Git** (para clonar el repositorio)
- **PowerShell** (para scripts opcionales)

### 1. Descargar el Proyecto

```bash
# Clonar desde GitHub
git clone https://github.com/DiegoAGalindo/MillionRealEstatecompany.API.git
cd MillionRealEstatecompany.API

# O descargar ZIP y extraer
# Navegar a la carpeta extraída
```

### 2. Ejecutar con Docker (¡TODO AUTOMÁTICO!)

```bash
# UN SOLO COMANDO - ¡Hace todo automáticamente!
docker-compose up --build -d
```

**⚡ Qué sucede automáticamente:**
1. 🐳 Construye contenedor de la API 
2. 🗄️ Inicia PostgreSQL
3. 📊 Aplica migraciones (crea tablas)
4. 🌱 Carga 75 registros de datos de prueba
5. 🚀 API lista en http://localhost:8080

### 3. Verificar que Todo Funciona

```bash
# Ver estado de contenedores
docker-compose ps

# Verificar datos cargados (PowerShell)
Invoke-RestMethod -Uri "http://localhost:8080/api/dataseeder/status"
```

### 4. Acceder a la API

- 🌐 **API Base**: http://localhost:8080/api
- 📚 **Swagger UI**: http://localhost:8080/swagger
- ✅ **Health Check**: http://localhost:8080/api/dataseeder/status

## 📡 Endpoints Principales

### Propietarios (Owners)
```
GET    /api/owners           # Listar todos los propietarios
GET    /api/owners/{id}      # Obtener propietario por ID
POST   /api/owners           # Crear nuevo propietario
PUT    /api/owners/{id}      # Actualizar propietario
DELETE /api/owners/{id}      # Eliminar propietario
```

### Propiedades (Properties)
```
GET    /api/properties       # Listar todas las propiedades
GET    /api/properties/{id}  # Obtener propiedad por ID
POST   /api/properties       # Crear nueva propiedad
PUT    /api/properties/{id}  # Actualizar propiedad
DELETE /api/properties/{id}  # Eliminar propiedad
```

### Imágenes de Propiedades
```
GET    /api/propertyimages                    # Listar todas las imágenes
GET    /api/propertyimages/{id}               # Obtener imagen por ID
GET    /api/propertyimages/property/{propertyId} # Imágenes por propiedad
POST   /api/propertyimages                    # Subir nueva imagen
PUT    /api/propertyimages/{id}               # Actualizar imagen
DELETE /api/propertyimages/{id}               # Eliminar imagen
```

### Trazabilidad de Precios
```
GET    /api/propertytraces                    # Listar todas las trazas
GET    /api/propertytraces/{id}               # Obtener traza por ID
GET    /api/propertytraces/property/{propertyId} # Trazas por propiedad
POST   /api/propertytraces                    # Crear nueva traza
PUT    /api/propertytraces/{id}               # Actualizar traza
DELETE /api/propertytraces/{id}               # Eliminar traza
```

### Gestión de Datos
```
GET    /api/dataseeder/status  # Verificar estado de la BD
POST   /api/dataseeder/seed    # Cargar datos iniciales
```

## 🗂️ Estructura del Proyecto

```
├── Controllers/           # Controladores de API
├── Models/               # Modelos de dominio
├── DTOs/                 # Data Transfer Objects
├── Data/                 # Contexto EF Core y configuración
├── Interfaces/           # Contratos y abstracciones
├── Repositories/         # Implementación de repositorios
├── Services/            # Lógica de negocio
├── Migrations/          # Migraciones de EF Core
├── scripts/             # Scripts SQL y PowerShell
├── Properties/          # Configuración de launch settings
├── appsettings*.json    # Configuración por ambiente
├── Dockerfile           # Configuración de contenedor
├── docker-compose.yml   # Orquestación de servicios
└── README.md           # Este archivo
```

## ⚙️ Configuración por Ambiente

La aplicación soporta múltiples ambientes:

- **Development**: `appsettings.Development.json`
- **Docker**: `appsettings.Docker.json`
- **Production**: `appsettings.Production.json`

### Variables de Entorno

```bash
# Para Docker
ASPNETCORE_ENVIRONMENT=Docker
ASPNETCORE_URLS=http://+:8080
ConnectionStrings__DefaultConnection=...

# Para Producción
ASPNETCORE_ENVIRONMENT=Production
```

## 🔧 Desarrollo Local (Sin Docker)

### Ejecutar en Entorno Local

```bash
# 1. Restaurar paquetes
dotnet restore

# 2. Configurar base de datos local en appsettings.Development.json
# (Necesitas PostgreSQL local instalado)

# 3. Las migraciones y datos se aplican automáticamente al ejecutar:
dotnet run

# La aplicación estará en: https://localhost:7001 o http://localhost:5000
```

**⚠️ Nota:** El desarrollo con Docker es **más fácil** porque no requiere instalar PostgreSQL localmente.

### Crear Nueva Migración (Solo Desarrolladores)

```bash
# Crear nueva migración
dotnet ef migrations add <NombreMigracion>

# Nota: Las migraciones se aplican automáticamente al iniciar la app
```

### Comandos Avanzados

```bash
# Ejecutar tests (cuando se implementen)
dotnet test

# Ver migraciones aplicadas
dotnet ef migrations list

# Revertir a migración específica (¡CUIDADO!)
dotnet ef database update <NombreMigracion>
```

## 🎯 **¿Por Qué Es Automático?**

El sistema está diseñado para **desarrollo ágil**. En `Program.cs`:

```csharp
// Se ejecuta automáticamente al iniciar
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    context.Database.Migrate();           // 1. Crear/actualizar tablas
    if (!await dataSeeder.HasDataAsync()) // 2. Solo si está vacía
    {
        await dataSeeder.SeedDataAsync(); // 3. Cargar datos
    }
}
```

**Beneficios:**
- ✅ **Zero Configuration**: Un comando y todo funciona
- ✅ **Consistencia**: Mismo setup para todo el equipo  
- ✅ **Desarrollo Rápido**: Datos listos inmediatamente
- ✅ **Sin Errores**: No hay que recordar pasos manuales

## 📊 Datos de Ejemplo (Carga Automática)

La base de datos se **puebla automáticamente** con datos realistas:
- ✅ **15 Propietarios** con información completa
- ✅ **15 Propiedades** de diferentes ciudades colombianas
- ✅ **30 Imágenes** distribuidas entre las propiedades
- ✅ **30 Trazas de Precio** con historial de cambios

**Total: 75 registros** cargados automáticamente al iniciar Docker

## �️ Comandos Útiles

### Scripts de Automatización
```powershell
# Inicio rápido (limpia y reconstruye todo)
.\scripts\start-dev.ps1

# Reset completo (limpia volúmenes y contenedores)
.\scripts\reset-dev.ps1
```

### Comandos Docker Manuales
```bash
# Ver logs en tiempo real
docker-compose logs -f api
docker-compose logs -f db

# Parar servicios
docker-compose down

# Parar y limpiar todo (incluye datos)
docker-compose down -v

# Estado de contenedores
docker-compose ps
```

### Verificación de Datos
```powershell
# Verificar que datos estén cargados
Invoke-RestMethod -Uri "http://localhost:8080/api/dataseeder/status"

# Contar registros por entidad
(Invoke-RestMethod -Uri "http://localhost:8080/api/owners").Count
(Invoke-RestMethod -Uri "http://localhost:8080/api/properties").Count
```

## �🐛 Troubleshooting

### Problema: Puerto 8080 en uso
```bash
# Cambiar puerto en docker-compose.yml
ports:
  - "8081:8080"  # Usar puerto 8081 localmente
```

### Problema: Error de conexión a base de datos
```bash
# Verificar estado de contenedores
docker-compose ps

# Revisar logs
docker-compose logs api
docker-compose logs db
```

### Problema: Migraciones pendientes
```bash
# Dentro del contenedor API
docker-compose exec api dotnet ef database update
```

## 📝 Contribución

1. Fork del proyecto
2. Crear rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para detalles.

## 👥 Autores

- **Equipo Million Real Estate** - *Desarrollo inicial*

## 🙏 Agradecimientos

- Entity Framework Core team
- ASP.NET Core community
- Docker community