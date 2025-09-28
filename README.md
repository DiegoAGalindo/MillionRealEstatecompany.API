# Million Real Estate Company API

## 📋 Descripción

API REST para la gestión de propiedades inmobiliarias desarrollada con ASP.NET Core 8.0. Implementa un sistema completo de CRUD para propiedades, propietarios, imágenes y trazabilidad de precios, siguiendo principios SOLID y pa```

## 🐳 Docker & Configuración Avanzada

### Variables de Entorno

Este proyecto utiliza archivos `.env` para manejar configuraciones sensibles:

**Archivos de configuración:**

- `.env` - Variables reales (NO se commitea)
- `.env.example` - Plantilla con ejemplos (SÍ se commitea)

### Configuración para Diferentes Ambientes

```bash
# Desarrollo (usar credenciales por defecto)
cp .env.example .env

# Producción (cambiar credenciales)
cp .env.example .env.prod
# Editar .env.prod con credenciales seguras

# Usar archivo específico
docker-compose --env-file .env.prod up
```

### Comandos Docker Útiles

```bash
# Construir y ejecutar
docker-compose up --build
docker-compose up -d --build  # En segundo plano

# Gestión de contenedores
docker-compose ps              # Estado de contenedores
docker-compose logs -f         # Ver logs en tiempo real
docker-compose logs api        # Logs solo de la API
docker-compose logs db         # Logs solo de la base de datos

# Parar y limpiar
docker-compose down            # Parar contenedores
docker-compose down -v         # Parar y eliminar volúmenes (⚠️ elimina datos)

# Reiniciar desde cero
docker-compose down -v
docker system prune -f
docker-compose up --build
```

### Verificación de Configuración

```bash
# Ver configuración con variables resueltas
docker-compose config

# Verificar que variables se cargan correctamente
docker-compose config --services
```

### Seguridad

- ✅ **`.env`** está en `.gitignore` (no se commitea)
- ✅ **Credenciales** separadas del código fuente
- ✅ **Variables** organizadas y documentadas
- ✅ **Diferentes configuraciones** por ambiente
- ✅ **Plantilla documentada** en `.env.example`

### Acceso a Servicios

- 🌐 **API Base**: http://localhost:8080/api
- 📚 **Swagger UI**: http://localhost:8080/swagger
- 🗄️ **PostgreSQL**: localhost:5432
  - Usuario: `postgres` (o el configurado en `.env`)
  - Password: `postgres` (o el configurado en `.env`)
  - Base de datos: `milliondb`

## 🐛 Troubleshootingnes de diseño modernos.

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

### 1. Descargar el Proyecto

```bash
# Clonar desde GitHub
git clone https://github.com/DiegoAGalindo/MillionRealEstatecompany.API.git
cd MillionRealEstatecompany.API

# O descargar ZIP y extraer
# Navegar a la carpeta extraída
```

### 2. Configurar Variables de Entorno

**⚠️ IMPORTANTE**: Este proyecto usa variables de entorno para credenciales seguras.

```bash
# Copiar plantilla de configuración
cp .env.example .env

# Editar .env con tus credenciales (opcional para desarrollo)
# Por defecto funciona con: postgres/postgres/milliondb
```

**📁 Archivo `.env` (ya configurado para desarrollo):**

```env
POSTGRES_DB=milliondb
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
ASPNETCORE_ENVIRONMENT=Docker
ASPNETCORE_URLS=http://+:80
```

### 3. Ejecutar con Docker (¡TODO AUTOMÁTICO!)

```bash
# UN SOLO COMANDO - ¡Hace todo automáticamente!
docker-compose up --build -d
```

**⚡ Qué sucede automáticamente:**

1. 🐳 Construye contenedor de la API
2. 🗄️ Inicia PostgreSQL con credenciales del .env
3. 📊 Aplica migración inicial unificada (crea todas las tablas e índices)
4. 🌱 Carga 75 registros de datos de prueba
5. 🚀 API lista en http://localhost:8080

### 4. Verificar que Todo Funciona

```bash
# Ver estado de contenedores
docker-compose ps

# Verificar datos cargados (PowerShell)
Invoke-RestMethod -Uri "http://localhost:8080/api/dataseeder/status"
```

### 5. Acceder a la API

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
├── .env                 # Variables de entorno (no se commitea)
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

### Comandos Rápidos de Desarrollo

```bash
# ⚡ INICIO RÁPIDO - Un solo comando
docker-compose up --build -d

# 🔄 RESET COMPLETO - Limpia todo y reinicia
docker-compose down -v && docker-compose up --build -d

# 📊 ESTADO - Ver qué está corriendo
docker-compose ps

# 📋 LOGS - Ver qué está pasando
docker-compose logs -f
```

> **💡 Nota**: Este proyecto usa comandos Docker estándar (sin scripts personalizados) para mayor simplicidad y compatibilidad multiplataforma.

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

## 🐛 Troubleshooting

### Problema: Archivo .env no encontrado

```bash
# Error: docker-compose no encuentra variables
# Solución: Crear archivo .env
cp .env.example .env
```

### Problema: Variables de entorno no se cargan

```bash
# Verificar configuración
docker-compose config

# Si las variables aparecen vacías, verificar:
ls -la .env                    # ¿Existe el archivo?
cat .env                       # ¿Tiene contenido?
```

### Problema: Puerto 8080 en uso

```bash
# Cambiar puerto en docker-compose.yml
ports:
  - "8081:80"  # Usar puerto 8081 localmente
```

### Problema: Error de conexión a base de datos

```bash
# 1. Verificar estado de contenedores
docker-compose ps

# 2. Revisar logs
docker-compose logs api
docker-compose logs db

# 3. Verificar variables de conexión
docker-compose config | grep -A5 -B5 ConnectionStrings

# 4. Reiniciar servicios
docker-compose restart db
docker-compose restart api
```

### Problema: Credenciales de base de datos incorrectas

```bash
# Error: "password authentication failed"
# Solución: Verificar variables en .env
cat .env | grep POSTGRES

# Si cambiaste credenciales, recrear volumen
docker-compose down -v
docker-compose up --build
```

### Problema: Migraciones pendientes

```bash
# Dentro del contenedor API
docker-compose exec api dotnet ef database update

# O reiniciar completamente
docker-compose down -v
docker-compose up --build
```

### Problema: Contenedores no inician

```bash
# Limpiar Docker completamente
docker-compose down -v
docker system prune -f
docker-compose up --build

# Ver logs detallados
docker-compose up --build (sin -d para ver logs)
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

- **Equipo Million Real Estate** - _Desarrollo inicial_

## 🙏 Agradecimientos

- Entity Framework Core team
- ASP.NET Core community
- Docker community
