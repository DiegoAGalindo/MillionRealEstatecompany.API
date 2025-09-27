# Script simple para iniciar el entorno de desarrollo
# Million Real Estate Company API

Write-Host "Million Real Estate Company - Development Start" -ForegroundColor Green
Write-Host "==============================================" -ForegroundColor Green

# Parar contenedores existentes
Write-Host "`nParando contenedores existentes..." -ForegroundColor Yellow
docker-compose down 2>$null

# Iniciar con rebuild
Write-Host "Iniciando contenedores con rebuild..." -ForegroundColor Cyan
docker-compose up --build -d

# Esperar un poco
Write-Host "Esperando servicios..." -ForegroundColor Cyan
Start-Sleep -Seconds 10

# Mostrar estado
Write-Host "`n=== Estado de Contenedores ===" -ForegroundColor Green
docker-compose ps

Write-Host "`n=== URLs Disponibles ===" -ForegroundColor Green
Write-Host "API: http://localhost:8080" -ForegroundColor Cyan
Write-Host "Swagger: http://localhost:8080/swagger" -ForegroundColor Cyan

Write-Host "`n=== Datos de Prueba ===" -ForegroundColor Green
Write-Host "Los datos se cargan automaticamente al iniciar la API" -ForegroundColor Yellow
Write-Host "Verifica los logs: docker-compose logs api" -ForegroundColor Yellow

Write-Host "`nListo! El entorno de desarrollo esta funcionando." -ForegroundColor Green