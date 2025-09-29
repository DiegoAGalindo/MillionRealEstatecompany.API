# Perfiles de Lanzamiento - Million Real Estate API

## 🚀 Perfiles disponibles en Visual Studio 2022

### 1. **Million API (Swagger)** ⭐ Recomendado para desarrollo
- **Inicia con:** Documentación Swagger UI
- **URL:** `http://localhost:5218/swagger`
- **Uso:** Desarrollo y prueba de APIs
- **Navegador:** Se abre automáticamente

### 2. **Million API (Health Check)**
- **Inicia con:** Endpoint de salud
- **URL:** `http://localhost:5218/health`
- **Uso:** Verificar estado de la aplicación
- **Navegador:** Se abre automáticamente

### 3. **Million API (No Browser)**
- **Inicia con:** Solo servidor
- **URL:** `http://localhost:5218`
- **Uso:** Para desarrollo con cliente externo o tests
- **Navegador:** No se abre automáticamente

### 4. **IIS Express**
- **Inicia con:** IIS Express + Swagger
- **URL:** `http://localhost:64769/swagger`
- **Uso:** Simulación de entorno de producción
- **Navegador:** Se abre automáticamente

## 🔧 Cómo cambiar el perfil de lanzamiento

### En Visual Studio 2022:
1. Busca el dropdown al lado del botón ▶️ (Play)
2. Selecciona el perfil deseado
3. Presiona F5 o el botón ▶️

### Endpoints útiles una vez iniciada la aplicación:
- **Swagger UI:** `/swagger`
- **Health Check:** `/health`
- **Health Check (Ready):** `/health/ready`
- **API Base:** `/api/`

## 📝 Notas importantes:
- El puerto `5218` es fijo para consistencia en desarrollo
- Todos los perfiles usan el ambiente `Development`
- El perfil por defecto ahora abre Swagger en lugar de WeatherForecast
- Para cambiar el puerto, modifica `applicationUrl` en `launchSettings.json`