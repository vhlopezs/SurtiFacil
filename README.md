# SurtiFácil - Módulo de Pedido Sugerido (CU-01)

Proyecto académico desarrollado en C# (.NET) y SQL Server Express estructurado en 3 capas.

## Requisitos de Entorno

1. SQL Server / SQL Express ejecutándose en localhost.
2. Servidor SQL accesible para la cadena de conexión del proyecto.

## Instrucciones para Restaurar la Base de Datos

Si tienes una versión antigua del proyecto, ejecuta esta sentencia en SQL Server Management Studio para borrar la base de datos previa:

```sql
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SurtiFacilDB')
BEGIN
    ALTER DATABASE SurtiFacilDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SurtiFacilDB;
END
GO
