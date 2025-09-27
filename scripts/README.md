# Million Real Estate Compa## 🚀 INSTRUCCIONES DE EJECUCIÓN

### 🆕 PRIMERA VEZ - Setup Inicial

**Si es tu primera vez con este proyecto:**

```powershell
# 1. Clona o descarga el proyecto
git clone <URL_DEL_REPOSITORIO>
# O descomprime el ZIP descargado

# 2. Navega a la carpeta del proyecto
cd "RUTA_DONDE_DESCARGASTE\MillionRealEstatecompany.API"

# 3. Verifica que tienes los archivos correctos
dir | findstr "docker-compose.yml"
# Debe aparecer: docker-compose.yml

# 4. Configura PowerShell (solo UNA vez)
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
# Escribir: Y [Enter]

# 5. Ejecuta el script
.\scripts\start-dev.ps1
```

---

### ⚡ INICIO RÁPIDO - `start-dev.ps1`- Development Scripts

Scripts de automatización para el desarrollo del proyecto.

## � UBICACIÓN DEL PROYECTO

**ANTES DE EJECUTAR SCRIPTS:** Debes estar en la carpeta raíz del proyecto.

### **¿Cómo identificar la carpeta correcta?**
La carpeta raíz debe contener estos archivos:
- ✅ `docker-compose.yml`
- ✅ `Program.cs`
- ✅ `MillionRealEstatecompany.API.csproj`
- ✅ Carpeta `scripts/`

### **Ejemplos de rutas típicas:**
```powershell
# Windows:
cd "C:\Users\TU_USUARIO\Desktop\MillionRealEstatecompany.API"
cd "C:\Projects\MillionRealEstatecompany.API"
cd "D:\Desarrollo\MillionRealEstatecompany.API"

# Verificar que estás en el lugar correcto:
dir | findstr "docker-compose.yml"
# Si aparece el archivo, estás en el lugar correcto ✅
```

## �🚀 INSTRUCCIONES DE EJECUCIÓN

### ⚡ INICIO RÁPIDO - `start-dev.ps1`

**📋 PRE-REQUISITOS:**
1. **Docker Desktop** instalado y ejecutándose
2. **PowerShell** abierto como Administrador
3. **Proyecto descargado** en tu computadora
4. **Terminal ubicado** en la carpeta raíz del proyecto

**📍 PASOS EXACTOS:**
```powershell
# 1. Navegar a la carpeta raíz del proyecto
# (donde está el archivo docker-compose.yml)
cd "RUTA_A_TU_PROYECTO\MillionRealEstatecompany.API"

# 2. Ejecutar el script
.\scripts\start-dev.ps1

# 3. Esperar mensaje: "Listo! El entorno de desarrollo esta funcionando."
```

**⏱️ TIEMPO ESTIMADO:** 2-3 minutos

**✅ RESULTADO ESPERADO:**
- API funcionando en: http://localhost:8080
- Swagger UI en: http://localhost:8080/swagger
- Base de datos con 75 registros cargados automáticamente

---

### 🔄 RESET COMPLETO - `reset-dev.ps1`

**📋 CUÁNDO USAR:**
- Problemas con contenedores
- Errores de base de datos
- Cambios importantes en configuración
- Limpieza completa del entorno

**📍 PASOS EXACTOS:**
```powershell
# 1. Navegar a la carpeta raíz del proyecto
# (donde está el archivo docker-compose.yml)
cd "RUTA_A_TU_PROYECTO\MillionRealEstatecompany.API"

# 2. Ejecutar el script
.\scripts\reset-dev.ps1

# 3. Esperar mensaje de finalización exitosa
```

**⏱️ TIEMPO ESTIMADO:** 3-5 minutos

**⚠️ ADVERTENCIA:** Eliminará todos los datos y contenedores existentes

## � VERIFICACIÓN DE FUNCIONAMIENTO

### **Paso 1: Verificar que los contenedores están corriendo**
```powershell
docker-compose ps
```
**Resultado esperado:** 2 contenedores (api y db) con status "Up"

### **Paso 2: Verificar que la API responde**
```powershell
Invoke-RestMethod -Uri "http://localhost:8080/api/dataseeder/status"
```
**Resultado esperado:** `hasData: True, message: "Database contains data"`

### **Paso 3: Verificar datos cargados**
```powershell
# Contar propietarios (debe ser 15)
(Invoke-RestMethod -Uri "http://localhost:8080/api/owners").Count

# Contar propiedades (debe ser 15)  
(Invoke-RestMethod -Uri "http://localhost:8080/api/properties").Count
```

### **Paso 4: Abrir Swagger UI**
1. Abrir navegador
2. Ir a: http://localhost:8080/swagger
3. Probar endpoint GET `/api/owners`

## �️ COMANDOS MANUALES DE EMERGENCIA

### **Parar todo manualmente:**
```powershell
docker-compose down
```

### **Ver logs si hay errores:**
```powershell
# Logs de la API
docker-compose logs api

# Logs de la base de datos
docker-compose logs db

# Logs en tiempo real
docker-compose logs -f api
```

### **Limpiar todo y empezar de cero:**
```powershell
docker-compose down -v
docker system prune -f
.\scripts\start-dev.ps1
```

## ⚙️ CONFIGURACIÓN DE POWERSHELL

### **Si aparece error de "Execution Policy":**
```powershell
# Ejecutar UNA VEZ como Administrador:
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Luego ejecutar el script normalmente:
.\scripts\start-dev.ps1
```

### **Si PowerShell no encuentra el script:**
```powershell
# Verificar que estás en la carpeta correcta:
Get-Location

# Debe mostrar la ruta que termine en: ...\MillionRealEstatecompany.API

# Verificar que el script existe:
Test-Path ".\scripts\start-dev.ps1"
# Debe retornar: True
```

## 🚨 SOLUCIÓN DE PROBLEMAS COMUNES

### ❌ **ERROR: "Puerto 8080 está en uso"**
**SOLUCIÓN:**
1. Abrir `docker-compose.yml`
2. Cambiar línea: `"8080:80"` por `"8081:80"`
3. Ejecutar: `.\scripts\start-dev.ps1`
4. Usar nueva URL: http://localhost:8081

### ❌ **ERROR: "Docker no está ejecutándose"**
**SOLUCIÓN:**
1. Abrir Docker Desktop
2. Esperar a que aparezca "Docker Desktop is running"
3. Ejecutar: `.\scripts\start-dev.ps1`

### ❌ **ERROR: "No se puede ejecutar scripts"**
**SOLUCIÓN:**
```powershell
# Ejecutar como Administrador:
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
# Escribir: Y [Enter]
# Luego ejecutar el script normalmente
```

### ❌ **ERROR: "API no responde después de 5 minutos"**
**SOLUCIÓN PASO A PASO:**
```powershell
# 1. Ver logs de errores
docker-compose logs api

# 2. Si hay errores de base de datos:
.\scripts\reset-dev.ps1

# 3. Si persiste el problema:
docker system prune -f
.\scripts\start-dev.ps1
```

### ❌ **ERROR: "Base de datos sin datos"**
**VERIFICAR:**
```powershell
# Ver si el seeder se ejecutó
docker-compose logs api | Select-String "Seed"

# Debe mostrar:
# "Seeded 15 owners"
# "Seeded 15 properties" 
# "Data seeding completed successfully"
```

## 📞 FLUJO DE AYUDA RÁPIDA

**Si algo no funciona:**
1. ✅ Docker Desktop corriendo → `.\scripts\reset-dev.ps1`
2. ✅ Error de puerto → Cambiar puerto en `docker-compose.yml`
3. ✅ Error de PowerShell → `Set-ExecutionPolicy RemoteSigned`
4. ✅ Todo falla → Reiniciar Docker Desktop y ejecutar `.\scripts\start-dev.ps1`