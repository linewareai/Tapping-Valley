# ValleyTapping: hoja de ruta móvil y online

## Decisiones de producto confirmadas

- **Plataformas prioritarias:** Android y iOS. La versión web actual se conserva como prototipo jugable y referencia de mecánicas.
- **Arte:** pixel art detallado, vista cenital (top-down), lectura clara en pantallas pequeñas y animaciones por spritesheet.
- **Multijugador:** visitas asíncronas primero; presencia y visitas simultáneas en tiempo real en una etapa posterior.
- **Social:** lista de amigos, permisos de visita, regalos opcionales, puestos de venta e intercambios controlados por servidor.
- **Identidad de mascotas:** nombres personalizados persistentes; arte individual, no sprites genéricos intercambiables.

## Arquitectura propuesta

### Cliente de juego
Godot 4.x, proyecto 2D con renderer compatible con móviles. Separar escenas y sistemas:
- `MainMenu`: iniciar sesión, invitado, continuar, opciones.
- `Farm`: mapa, personaje, parcelas, edificios, objetos interactivos.
- `Inventory`, `Crafting`, `Market`, `Friends`, `VisitFarm`.
- `Pet`: identidad, nombre, necesidades, vínculo y animaciones.
- `SaveSync`: cola de cambios locales, sincronización y resolución de conflictos.

Diseñar controles táctiles desde el inicio: joystick virtual opcional, toque para interactuar, objetivos táctiles generosos, escalado de UI y soporte para distintas relaciones de aspecto.

### Backend
Usar un **proyecto Supabase independiente para ValleyTapping**. No reutilizar el proyecto Supabase de otras aplicaciones. Supabase puede aportar Auth, PostgreSQL y Realtime; el cliente nunca debe tener acceso a claves secretas de servicio.

Principios:
- La cuenta autenticada es la propietaria de la granja.
- Las políticas RLS limitan lecturas y escrituras por propietario y permisos explícitos.
- El servidor es la autoridad para monedas, inventario, compras, ventas, regalos e intercambios.
- Las operaciones económicas se realizan mediante funciones transaccionales de servidor/RPC, con validación de cantidades, saldo, stock, propiedad e idempotencia.
- No confiar en hora, saldo o inventario enviados por el cliente.
- Guardar eventos de auditoría para movimientos de objetos y moneda.
- Modo invitado con guardado local; ofrecer vinculación posterior a Google, Apple o correo, sin perder la granja.
- Incluir eliminación de cuenta y de datos desde el producto si se habilita la creación de cuentas.

## Modelo de datos inicial

- `profiles`: ID de Auth, nombre visible, avatar, fecha de creación.
- `farms`: propietario, nombre, datos de diseño/versionado, última sincronización.
- `farm_visits`: visitante, granja visitada, permiso, fecha; sin conceder escritura sobre la granja ajena.
- `friendships`: solicitante, destinatario, estado y fechas.
- `farm_permissions`: propietario, visitante y capacidades permitidas.
- `pet_profiles`: granja, especie/identificador de arte, nombre, vínculo y atributos persistentes.
- `crop_instances`: parcela, tipo de cultivo, instante de plantación, instante de madurez, estado de riego.
- `inventory_items`: propietario, tipo de objeto, cantidad y versión.
- `market_listings`: vendedor, artículo, cantidad, precio, estado y vencimiento.
- `trade_sessions` / `trade_items`: ofertas entre jugadores, aceptación de ambas partes y estado final.
- `wallet_ledger`: libro mayor inmutable de entradas y salidas de moneda.
- `transaction_receipts`: claves idempotentes para impedir duplicar una operación al reintentar.

El esquema exacto debe migrarse y probarse en un proyecto de juego nuevo antes de guardar datos reales.

## Visitas asíncronas primero

1. Publicar una instantánea de granja versionada.
2. Cargarla como escena de solo lectura para el visitante.
3. Respetar opciones del dueño: pública, amigos o privada.
4. Mostrar nombre, decoración, cultivos y mascotas.
5. Permitir acciones limitadas explícitamente por el dueño (por ejemplo, dejar un regalo); nunca permitir editar inventario o parcelas ajenas directamente.
6. Registrar las visitas y aplicar límites antiabuso.

## Mercado e intercambios seguros

- Mercado global o por tablón, con comisión opcional configurable.
- La publicación reserva el stock en una transacción del servidor.
- La compra comprueba stock, precio y saldo y transfiere artículo y moneda atómicamente.
- Cancelaciones y vencimientos devuelven stock una sola vez.
- Intercambio directo: ambas partes bloquean sus artículos, ven el resumen y confirman; cualquier cambio invalida la confirmación previa.
- Sin transferencias de dinero real en la primera versión.
- Límites de frecuencia, detección de cuentas abusivas y registro de movimientos.

## Tiempo y guardado

Los cultivos usan marcas de tiempo de servidor (`planted_at`, `ready_at`) y estados persistentes, no un temporizador que dependa de mantener abierta la app. Al volver, el cliente consulta el estado actual y calcula la etapa visual. Definir límites razonables de progreso offline y validar el tiempo en servidor.

## Plan por fases

### Fase 0: base técnica
- Crear proyecto Godot móvil separado en este repositorio.
- Definir resolución lógica, cámara cenital, sistema de tiles, entradas táctiles y convenciones de spritesheets.
- Preparar proyecto Supabase independiente, migraciones, entornos y políticas RLS.
- Mantener la web actual disponible sin afirmar que ya es una app nativa.

### Fase 1: granja individual jugable
- Movimiento, colisiones, parcelas, sembrar, regar y cosechar.
- Crecimiento por tiempo real y progreso offline.
- Inventario, economía obtenida por cosechas/encargos, casa y guardado.
- Mascotas con nombres, animaciones y estados persistentes.
- Guardado local primero y sincronización autenticada después.

### Fase 2: cuenta y nube
- Invitado y vinculación de cuenta; Google/Apple/correo según configuración de plataforma.
- Sincronización, recuperación de cuenta, manejo de desconexiones y conflictos.
- Pruebas de restauración y de cambios simultáneos en dos dispositivos.

### Fase 3: amigos y visitas asíncronas
- Búsqueda por nombre/ID, solicitudes, aceptar/rechazar/bloquear.
- Privacidad de granja.
- Visitas de solo lectura y registro de actividad.
- Regalos limitados con operaciones validadas por servidor.

### Fase 4: mercado e intercambios
- Puestos, listados, compra/venta, historial y recibos idempotentes.
- Intercambios directos con doble confirmación.
- Pruebas contra duplicación, saldos negativos y solicitudes repetidas.

### Fase 5: multijugador en tiempo real
- Presencia online, instancias de visita y movimiento sincronizado.
- Sincronizar solo eventos necesarios; mantener inventario y economía bajo autoridad del servidor.
- Reconexión, límites de concurrencia y pruebas de latencia.

### Fase 6: publicación
- Pruebas internas en Android y TestFlight en iOS.
- Icono, capturas, política de privacidad, soporte, borrado de cuenta y revisión de tiendas.
- Firma y exportación de builds; las cuentas de desarrollador y el acceso a macOS/Xcode serán necesarios para la publicación correspondiente.

## Criterios de aceptación antes de abrir el mercado

- Repetir una petición de compra no duplica ni el artículo ni el cargo.
- Dos compradores no pueden comprar la última unidad a la vez.
- No es posible leer o modificar granjas privadas sin permiso.
- Una visita nunca puede modificar directamente la granja del anfitrión.
- Una desconexión durante una operación no deja monedas u objetos a medias.
- Cultivos y progreso sobreviven al cierre forzado y al cambio de dispositivo.
- Se prueban cuentas bloqueadas, nombres inválidos, cantidades extremas y reintentos.

## Estado honesto

Este documento registra las decisiones y la arquitectura propuesta. **No significa que ya existan** el proyecto Godot, las tablas de producción, el sistema de amigos, el mercado ni las visitas en tiempo real. La implementación debe avanzar por fases y probarse en dispositivos reales.
