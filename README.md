# Million Real Estate Company API

## 📋 Descripción

API REST para la gestión de propiedades inmobiliarias desarrollada con ASP.NET Core 8.0, Prueba Técnica para Million.

## 🏗️ Arquitectura y Tecnologías

### Patrones Implementados

- **Repository Pattern**: Abstracción de la capa de datos
- **Service Layer**: Lógica de negocio
- **Dependency Injection**: Inversión de control
- **DTO Pattern**: Separación entre modelos de dominio y API

### Stack Tecnológico

- **Backend**: ASP.NET Core 8.0
- **Base de Datos**: MongoDB con MongoDB.Driver
- **Mapeo**: AutoMapper para DTOs
- **Contenedores**: Docker & Docker Compose
- **Documentación**: Swagger/OpenAPI
- **Testing**: NUnit con cobertura del 86.8%

## � Descarga e Instalación

### Prerrequisitos

- **Docker Desktop** (recomendado - instalado y ejecutándose)
- **.NET 8.0 SDK** (solo para desarrollo local sin Docker)
- **Git** (para clonar el repositorio)

### 1. Obtener el Proyecto

```bash
# Opción 1: Clonar desde GitHub
git clone https://github.com/DiegoAGalindo/MillionRealEstatecompany.API.git
cd MillionRealEstatecompany.API

# Opción 2: Descargar ZIP
# 1. Descargar desde GitHub como ZIP
# 2. Extraer en la carpeta deseada
# 3. Navegar a la carpeta extraída
```

### 2. Configurar Variables de Entorno

**⚠️ IMPORTANTE**: Este proyecto usa variables de entorno para credenciales seguras. Sin embargo, para facilitar el ejercicio, el archivo `.env` ya viene preconfigurado para desarrollo.

```bash
# Copiar plantilla de configuración
cp .env.example .env

# El archivo .env ya viene configurado para desarrollo
# Opcionalmente puedes editarlo según tus necesidades
```


## 🐳 Ejecución con Docker (Recomendado)
### Inicio Rápido - Un Solo Comando

```bash
# ⚡ COMANDO ÚNICO - ¡Hace todo automáticamente!
docker-compose up --build -d
```

**🎯 Qué sucede automáticamente:**

1. 🐳 Construye contenedor de la API
2. 🗄️ Inicia MongoDB con credenciales del .env
3. 📊 Crea base de datos MongoDB automáticamente
4. 🌱 Carga 75 registros de datos de prueba
5. 🚀 API lista en http://localhost:8080

### Comandos Docker Útiles

```bash
# 🔄 Construir y ejecutar
docker-compose up --build              # En primer plano (ver logs)
docker-compose up --build -d           # En segundo plano

# 📊 Gestión de contenedores
docker-compose ps                       # Estado de contenedores
docker-compose logs -f                  # Ver logs en tiempo real
docker-compose logs api                 # Logs solo de la API
docker-compose logs db                  # Logs solo de la base de datos

# ⛔ Parar y limpiar
docker-compose down                     # Parar contenedores
docker-compose down -v                  # Parar y eliminar volúmenes (⚠️ elimina datos)

# 🔄 Reiniciar desde cero
docker-compose down -v
docker system prune -f
docker-compose up --build -d
```
### 🌐 Acceso a Servicios

- **API Base**: http://localhost:8080/api
- **Swagger UI**: http://localhost:8080/swagger/index.html
- **MongoDB**: localhost:27017 (milliondb/mongo/mongopass)

## 🔐 Autenticación JWT

Esta API utiliza autenticación JWT (JSON Web Token) para proteger todos los endpoints. **Todos los controladores requieren autenticación**.
**⚠️ IMPORTANTE**: Si desea omitir la autenticación para pruebas rápidas, puede comentar la línea `app.UseAuthentication();` en `Program.cs`, O la anotacion `[Authorize]` que tienen los `[Controllers]`, pero no es recomendable para entornos reales.

### 📝 Credenciales de Acceso

Para obtener un token JWT, utiliza las siguientes credenciales:

```json
{
  "username": "testmillion",
  "password": "TestMillionPass"
}
```

### 🚀 Cómo Usar la Autenticación

#### Paso 1: Obtener Token JWT
```http
POST /api/Auth/login
Content-Type: application/json

{
  "username": "testmillion",
  "password": "TestMillionPass"
}
```

**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login exitoso"
}
```

#### Paso 2: Usar el Token en Requests
Incluye el token en el header `Authorization` de todas las peticiones:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### Paso 3: Usar Swagger UI
1. Ve a http://localhost:8080/swagger/index.html
2. Haz clic en el botón **"Authorize"** en la parte superior
3. Ingresa: `Bearer {tu-token-jwt}`
4. Haz clic en **"Authorize"**
5. ¡Ya puedes probar todos los endpoints protegidos!

### ⚙️ Configuración JWT

Los tokens tienen las siguientes características:
- **Algoritmo**: HMAC SHA256
- **Expiración**: 60 minutos
- **Emisor**: MillionRealEstate API
- **Audiencia**: MillionRealEstate Users

> **⚠️ Importante**: Sin autenticación, recibirás un error 401 (Unauthorized) en todos los endpoints excepto en `/api/Auth/login`.


## 📡 API Endpoints

### 👥 Propietarios (Owners)

```http
GET    /api/owners           # Listar todos los propietarios
GET    /api/owners/{id}      # Obtener propietario por ID
POST   /api/owners           # Crear nuevo propietario
PUT    /api/owners/{id}      # Actualizar propietario
DELETE /api/owners/{id}      # Eliminar propietario
```

### 🏠 Propiedades (Properties)

```http
GET    /api/properties       # Listar todas las propiedades
GET    /api/properties/{id}  # Obtener propiedad por ID
POST   /api/properties       # Crear nueva propiedad
PUT    /api/properties/{id}  # Actualizar propiedad
DELETE /api/properties/{id}  # Eliminar propiedad
```

### 📸 Imágenes de Propiedades (PropertyImages)

```http
GET    /api/propertyimages                        # Listar todas las imágenes
GET    /api/propertyimages/{id}                   # Obtener imagen por ID
GET    /api/propertyimages/property/{propertyId}  # Imágenes por propiedad
POST   /api/propertyimages                        # Subir nueva imagen
PUT    /api/propertyimages/{id}                   # Actualizar imagen
DELETE /api/propertyimages/{id}                   # Eliminar imagen
```

### 📈 Trazabilidad de Precios (PropertyTraces)

```http
GET    /api/propertytraces                        # Listar todas las trazas
GET    /api/propertytraces/{id}                   # Obtener traza por ID
GET    /api/propertytraces/property/{propertyId}  # Trazas por propiedad
POST   /api/propertytraces                        # Crear nueva traza
PUT    /api/propertytraces/{id}                   # Actualizar traza
DELETE /api/propertytraces/{id}                   # Eliminar traza
```

### 🛠️ Gestión de Datos (DataSeeder)

```http
GET    /api/dataseeder/status  # Verificar estado de la BD
POST   /api/dataseeder/seed    # Cargar datos iniciales
```

## 🗂️ Estructura del Proyecto

```
MillionRealEstatecompany.API/
├── Controllers/           # Controladores de API REST
│   ├── OwnersController.cs
│   ├── PropertiesController.cs
│   ├── PropertyImagesController.cs
│   └── PropertyTracesController.cs
├── Models/               # Modelos de dominio
│   ├── Owner.cs
│   ├── Property.cs
│   ├── PropertyImage.cs
│   └── PropertyTrace.cs
├── DTOs/                 # Data Transfer Objects
├── Data/                 # Contexto MongoDB y configuración
│   ├── MongoDbContext.cs
│   ├── MappingProfile.cs
│   └── DataSeeder.cs
├── Interfaces/           # Contratos y abstracciones
├── Repositories/         # Implementación de repositorios
├── Services/            # Lógica de negocio
├── Middleware/          # Middleware personalizado
├── .env                 # Variables de entorno
├── appsettings*.json    # Configuración por ambiente
├── Dockerfile           # Configuración de contenedor
├── docker-compose.yml   # Orquestación de servicios
└── README.md           # Este archivo
```

## � Desarrollo Local (Sin Docker)

### Prerrequisitos para Desarrollo Local

- **.NET 8.0 SDK** (instalado)
- **MongoDB** (instalado y ejecutándose localmente)
- **Visual Studio** o **VS Code** (recomendado)

### Configuración de Base de Datos Local

1. **Instalar MongoDB** localmente
2. **Crear base de datos** `milliondb`
3. **Configurar conexión** en `appsettings.Development.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "milliondb"
  }
}
```

### Ejecutar la Aplicación

```bash
# 1. Navegar al proyecto principal
cd MillionRealEstatecompany.API

# 2. Restaurar paquetes NuGet
dotnet restore

# 3. Ejecutar la aplicación (se conecta automáticamente a MongoDB)
dotnet run
```

**⚠️ Nota:** El desarrollo con Docker es **más fácil** porque no requiere instalar MongoDB localmente.

## 🎯 Automatización y Beneficios

### ¿Por Qué Es Automático?

El sistema está diseñado para **desarrollo ágil**. En `Program.cs`:

```csharp
// Se ejecuta automáticamente al iniciar
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    // MongoDB se conecta automáticamente      // 1. Conectar a MongoDB
    if (!await dataSeeder.HasDataAsync())     // 2. Solo si está vacía
    {
        await dataSeeder.SeedDataAsync();    // 3. Cargar datos
    }
}
```

**Beneficios:**

- ✅ **Zero Configuration**: Un comando y todo funciona
- ✅ **Consistencia**: Mismo setup para todo el equipo
- ✅ **Desarrollo Rápido**: Datos listos inmediatamente
- ✅ **Sin Errores**: No hay que recordar pasos manuales

## 📊 Datos de Ejemplo (Carga Automática)

La base de datos se **puebla automáticamente** con datos realistas al iniciar en modo desarrollo o Docker

## 🧪 Testing y Calidad

### Ejecutar Pruebas

```bash
# Ejecutar todas las pruebas
dotnet test

# Generar reporte de cobertura
.\coverage.bat

# Ver reporte HTML
# Abrir: CoverageReport\index.html
```


## 👥 Autor

- **Diego A. Galindo** - *Desarrollo inicial* - [DiegoAGalindo](https://github.com/DiegoAGalindo)

