# Database Migrations

## Historia de Migraciones

### InitialMigration (2025-09-29)
**Migración inicial unificada** que incluye toda la estructura de base de datos optimizada.

#### Características incluidas:
- ✅ Todas las tablas principales (Owners, Properties, PropertyImages, PropertyTraces)
- ✅ Índices optimizados para performance:
  - `DocumentNumber` (único) para propietarios
  - `Email` para búsquedas rápidas de propietarios
  - `CodeInternal` (único) para propiedades
  - `IdOwner` para consultas por propietario
  - `Price` para filtros y ordenamientos por precio
- ✅ Relaciones con políticas de eliminación apropiadas:
  - Properties → Owners (Restrict)
  - PropertyImages → Properties (Cascade)
  - PropertyTraces → Properties (Cascade)
- ✅ Tipos de datos optimizados (decimal(18,2), date, varchar con límites)

#### Notas:
- Esta migración reemplaza las migraciones anteriores que fueron unificadas
- El proyecto no estaba en producción, por lo que era seguro unificar
- Incluye todas las mejoras de índices y optimizaciones identificadas en la revisión

## Comandos útiles

```bash
# Aplicar migraciones
dotnet ef database update

# Crear nueva migración
dotnet ef migrations add "NombreMigracion"

# Remover última migración
dotnet ef migrations remove

# Ver script SQL de migración
dotnet ef migrations script
```