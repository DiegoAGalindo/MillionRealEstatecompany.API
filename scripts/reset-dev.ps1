# Script para resetear completamente el entorno de desarrollo
# Million Rea} catch {
Write-Host \"`n⚠ Error durante el reset: $($_.Exception.Message)\" -ForegroundColor Red
exit 1
}tate Company API

param(
    [switch]$SkipBuild,
    [switch]$Quiet
)

$ErrorActionPreference = "Stop"

if (-not $Quiet) {
    Write-Host "Million Real Estate Company - Reset Development Environment" -ForegroundColor Green
    Write-Host "================================================================" -ForegroundColor Green
}

try {
    # Parar y limpiar contenedores existentes
    if (-not $Quiet) { Write-Host "\n1. Deteniendo contenedores existentes..." -ForegroundColor Cyan }
    docker-compose down -v --remove-orphans 2>$null
    
    # Limpiar imágenes huérfanas del proyecto
    if (-not $Quiet) { Write-Host "2. Limpiando imágenes..." -ForegroundColor Cyan }
    docker system prune -f --filter "label=com.docker.compose.project=millionrealestatecompanyapi" 2>$null
    
    # Reconstruir y ejecutar
    if (-not $SkipBuild) {
        if (-not $Quiet) { Write-Host "3. Reconstruyendo y ejecutando contenedores..." -ForegroundColor Cyan }
        docker-compose up --build -d
    } else {
        if (-not $Quiet) { Write-Host "3. Ejecutando contenedores (sin rebuild)..." -ForegroundColor Cyan }
        docker-compose up -d
    }
    
    # Esperar a que los servicios estén listos
    if (-not $Quiet) { Write-Host "4. Esperando servicios..." -ForegroundColor Cyan }
    
    $maxAttempts = 30
    $attempt = 1
    
    do {
        Start-Sleep -Seconds 2
        $apiStatus = docker-compose ps --services --filter "status=running" | Where-Object { $_ -eq "api" }
        $dbStatus = docker-compose ps --services --filter "status=running" | Where-Object { $_ -eq "db" }
        
        if (-not $Quiet) {
            Write-Host "  Intento $attempt/$maxAttempts - API: $(if($apiStatus){'OK'}else{'Esperando'}) | DB: $(if($dbStatus){'OK'}else{'Esperando'})" -ForegroundColor Yellow
        }
        
        $attempt++
    } while ((-not $apiStatus -or -not $dbStatus) -and $attempt -le $maxAttempts)
    
    if ($apiStatus -and $dbStatus) {
        if (-not $Quiet) {
            Write-Host "\n✓ Entorno de desarrollo listo!" -ForegroundColor Green
            Write-Host "\n=== URLs Disponibles ===" -ForegroundColor Green
            Write-Host "API: http://localhost:8080" -ForegroundColor Cyan
            Write-Host "Swagger: http://localhost:8080/swagger" -ForegroundColor Cyan
            Write-Host "\n=== Estado de Servicios ===" -ForegroundColor Green
        }
        
        docker-compose ps
        
        if (-not $Quiet) {
            Write-Host "\n=== Datos Cargados Automáticamente ===" -ForegroundColor Green
            Write-Host "Los datos de prueba se cargan automáticamente al iniciar." -ForegroundColor Yellow
            Write-Host "Revisa los logs con: docker-compose logs api" -ForegroundColor Yellow
        }
        
    } else {
        Write-Host "\n✗ Error: Los servicios no iniciaron correctamente" -ForegroundColor Red
        Write-Host "\nRevisar logs:" -ForegroundColor Yellow
        Write-Host "docker-compose logs api" -ForegroundColor White
        Write-Host "docker-compose logs db" -ForegroundColor White
        exit 1
    }
    
} catch {
    Write-Host "\n✗ Error durante el reset: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}