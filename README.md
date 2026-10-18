# SurtiFácil - Módulo de Pedido Sugerido (CU-01)

Proyecto académico desarrollado en C# (.NET) y SQL Server Express estructurado en 3 capas.

## Requisitos de Entorno
1. **SQL Server / SQL Express** ejecutándose en `localhost`.
2. Base de datos `SurtiFacilDB` creada.

## Instrucciones para Restaurar la Base de Datos
1. Ejecutar el script principal de datos de prueba (`SurtiFacilDB`).
2. Ejecutar el script de procedimientos almacenados y tablas de orden (`spObtenerDatosPedido`, `spListarCategorias`, `spInsertarOrdenCompra`, `spInsertarDetalleOrdenCompra`, `OrdenCompra`, `DetalleOrdenCompra`).

## Estructura del Diseño
El diseño de la interfaz visual se encuentra construido por código (sin diseñador drag-and-drop de VS) para permitir mantenibilidad y adaptabilidad modular:
- **Colores y Fuentes:** Modificar la clase `clsEstiloSF.cs` en la capa de presentación.
- **Estructura y Controles:** Modificar la clase parcial `frmPedidoSugeridoSF.Diseno.cs`.
- **Lógica de Interfaz y Eventos:** Modificar la clase parcial `frmPedidoSugeridoSF.cs`.

