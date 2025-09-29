@echo off
cls
echo =============================================
echo          REPORTE DE COBERTURA DE CODIGO
echo =============================================
echo.
echo Limpiando archivos anteriores...
if exist "TestResults" rmdir /s /q "TestResults"
if exist "CoverageReport" rmdir /s /q "CoverageReport"
echo.

echo Ejecutando pruebas con cobertura...
echo (Excluyendo: Models, DTOs, Migrations, ModelValidationMiddleware, DataSeeder, ErrorResponse)
echo.
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults --logger "console;verbosity=normal"

if errorlevel 1 (
    echo.
    echo  Error al ejecutar las pruebas
    pause
    exit /b 1
)

echo.
echo Verificando ReportGenerator...
where reportgenerator >nul 2>&1
if errorlevel 1 (
    echo Instalando ReportGenerator...
    dotnet tool install --global dotnet-reportgenerator-globaltool
)

echo.
echo Generando reporte HTML...
for /r TestResults %%f in (coverage.cobertura.xml) do (
    reportgenerator -reports:"%%f" -targetdir:"CoverageReport" -reporttypes:Html -classfilters:"-MillionRealEstatecompany.API.Models.*;-MillionRealEstatecompany.API.DTOs.*;-MillionRealEstatecompany.API.Middleware.ModelValidationMiddleware;-MillionRealEstatecompany.API.Services.DataSeeder;-MillionRealEstatecompany.API.Middleware.ErrorResponse;-Program" -filefilters:"-**/Migrations/**;-**/Models/**;-**/DTOs/**"
)

if exist "CoverageReport\index.html" (
    echo.
    echo  Reporte generado exitosamente!
    echo  Ubicaci?n: CoverageReport\index.html
    echo.
    echo Exclusiones aplicadas:
    echo    Models (clases de entidad)
    echo    DTOs (objetos de transferencia)  
    echo    Migrations (scripts de DB)
    echo    ModelValidationMiddleware (infraestructura)
    echo    DataSeeder (datos iniciales)
    echo    ErrorResponse (modelo de error)
    echo.
    echo  Abriendo reporte...
    start "" "CoverageReport\index.html"
) else (
    echo  No se pudo generar el reporte
    pause
)
echo.
echo Presiona cualquier tecla para salir...
pause >nul
