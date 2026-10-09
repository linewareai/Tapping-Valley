# ValleyTapping: hoja de ruta móvil y online

## Dirección del producto
- **Plataformas prioritarias:** Android y iPhone.
- **Arte:** pixel art detallado, vista desde arriba (top-down), con personajes y mascotas reconocibles y animaciones cuidadas.
- **Multijugador:** comenzar con visitas asíncronas a granjas guardadas; diseñar la arquitectura para habilitar visitas en tiempo real más adelante.
- **Juego:** simulador acogedor de granja. La agricultura, la exploración, las mascotas y la comunidad deben ser el centro, no el clic repetitivo.

## Arquitectura propuesta
### Cliente de juego
- **Godot 4** para el juego móvil 2D y las animaciones pixel art.
- Mantener la versión web actual como prototipo mientras se construye la versión móvil en un proyecto separado. El prototipo HTML no se convierte automáticamente en una app nativa.
- Usar arte original por capas y spritesheets con animaciones consistentes. Mantener el arte de cada mascota como archivo individual y validar su parecido con las referencias antes de darlo por terminado.

### Backend online
- **Proyecto Supabase independiente para ValleyTapping**, separado de cualquier proyecto existente que pertenezca a otra aplicación.
- Supabase Auth para Google, correo electrónico y modo invitado, con un camino claro para vincular una cuenta invitada.
- PostgreSQL para perfiles, amistades, granjas, inventarios, anuncios del mercado y transacciones.
- Storage para avatares y contenido permitido por el juego, no para guardar el estado crítico de la economía como archivos editables por el cliente.
- Políticas RLS y funciones del lado servidor para autorizar visitas, intercambios y ventas. El cliente nunca debe poder adjudicarse monedas o artículos directamente.

## Sistemas y reglas de producto
### 1. Granja y cultivos
- Guardar hora de plantación y hora de cosecha (planted_at, ready_at), además del estado del cultivo.
- Calcular el crecimiento con hora del servidor para que avance mientras el jugador está desconectado.
- Validar en el servidor las cosechas y recompensas importantes para reducir trampas y duplicación.
- La economía debe premiar cultivar, cuidar, completar pedidos y comerciar; no depender de producción pasiva de un clicker.

### 2. Mascotas
- Guardar nombre elegido, especie, apariencia, vínculo, necesidades y progreso.
- Las acciones de cuidado deben tener límites y resultados validados.
- Las mascotas visibles en una visita son una representación de la granja del anfitrión; los visitantes no pueden modificar su inventario ni sus estadísticas sin permiso explícito.

### 3. Amigos y visitas asíncronas (primera versión online)
- Buscar jugadores por nombre público o código de amigo, no por correo electrónico.
- Solicitudes de amistad, aceptar/rechazar, bloquear y eliminar.
- Visitar una instantánea actualizada de una granja aunque el dueño esté desconectado.
- Permisos por granja: pública, solo amigos o privada.
- Las visitas son inicialmente de solo lectura; más adelante se pueden permitir ayudas o regalos limitados y registrados.
- Limitar frecuencia de visitas y proteger la privacidad del jugador.

### 4. Mercado y comercio
- **Puestos del mercado:** anuncios con artículo, cantidad, precio, fecha de vencimiento y propietario.
- **Intercambio directo:** ambos jugadores confirman los artículos y monedas; la operación se completa como una transacción atómica.
- El servidor comprueba propiedad, cantidades, límites de precio, saldo y vigencia antes de mover nada.
- Registrar un historial de transacciones y permitir caducar/cancelar anuncios.
- Evitar duplicaciones: nunca confiar en que el cliente envíe el saldo final; calcularlo y modificarlo en una transacción del servidor.
- Añadir límites antiabuso, registro de eventos y herramientas para resolver errores. No lanzar comercio entre jugadores antes de probar bien estas protecciones.

### 5. Multijugador en tiempo real (fase posterior)
- No es requisito para la primera versión social.
- Diseñar la API para que después se puedan añadir salas, presencia y movimiento sincronizado sin rehacer cuentas, granjas ni inventarios.
- Separar estado persistente (granja, inventario, economía) del estado efímero (posición, emotes, presencia).
- Si se habilita, el servidor controla las acciones sensibles; la sincronización de movimiento nunca debe dar autoridad al cliente sobre la economía.

## Modelo de datos inicial (borrador)
- profiles: user_id, nombre público, código de amigo, avatar, fecha de creación.
- friendships: solicitante, destinatario, estado, marcas de tiempo.
- farms: owner_id, nombre, configuración de privacidad, versión/fecha de actualización.
- farm_tiles: granja, coordenada, tipo de terreno, estado.
- crops: parcela, especie, planted_at, ready_at, estado.
- inventory_items: propietario, artículo, cantidad.
- pets: propietario, nombre elegido, especie, apariencia y estadísticas permitidas.
- market_listings: vendedor, artículo, cantidad, precio, estado, vencimiento.
- trade_sessions: participantes, estado y vencimiento.
- trade_items: sesión, propietario original, artículo, cantidad.
- transactions: tipo, participantes, resumen, fecha y clave de idempotencia.
- farm_visits: visitante, granja visitada, fecha, solo para métricas limitadas y respetuosas con la privacidad.

El esquema definitivo debe incorporar claves foráneas, restricciones, índices, políticas RLS y funciones transaccionales. Este documento es un plan, no significa que el backend o el multijugador ya estén implementados.

## Hoja de ruta
### Fase 0: base y decisiones
- Mantener la versión web como prototipo funcional.
- Crear un proyecto Supabase separado para ValleyTapping cuando se autorice.
- Crear el proyecto Godot y documentar cómo ejecutar/exportar.
- Definir resolución base, escala de píxel, paleta, cuadrícula, formato de spritesheets y convenciones de animación.

### Fase 1: núcleo jugable móvil
- Movimiento y cámara top-down.
- Terreno, herramientas, plantar, regar, crecimiento por tiempo y cosechar.
- Inventario, economía básica, pedidos y progreso offline.
- Mascotas con arte y animaciones originales.
- Guardado local robusto, migraciones y pruebas en Android.

### Fase 2: cuentas y nube
- Inicio de sesión Google/correo e invitado con vinculación.
- Sincronización de guardado y resolución de conflictos.
- Recuperación de cuenta, cierre de sesión y eliminación de cuenta dentro de la app.
- Reglas de seguridad y pruebas de restauración de partidas.

### Fase 3: amigos y visitas asíncronas
- Perfiles y códigos de amigo.
- Solicitudes, bloqueo y privacidad.
- Carga de granjas ajenas en modo visita.
- Regalos/ayudas opcionales, con límites diarios y registro.

### Fase 4: mercado seguro
- Puestos de venta, compra y cancelación.
- Intercambio directo con confirmación mutua.
- Transacciones atómicas, historial, controles antiabuso y pruebas de duplicación.
- Prueba cerrada con un grupo pequeño antes de abrirlo a todos.

### Fase 5: beta móvil
- Pruebas de rendimiento, accesibilidad, batería, red lenta y modo desconectado.
- Android primero para validar el flujo de publicación; preparar iOS en paralelo cuando haya acceso a macOS/Xcode y las cuentas necesarias.
- Pruebas cerradas, telemetría mínima respetuosa con la privacidad y proceso de soporte.

### Fase 6: visitas en tiempo real
- Presencia y salas privadas para amigos.
- Movimiento y emotes sincronizados.
- Límites de jugadores por sala y reconexión.
- Mantener economía e inventario bajo autoridad del servidor.

## Criterios de calidad antes de publicar
- Ningún jugador puede leer o modificar datos privados de otro fuera de las reglas autorizadas.
- La misma compra/intercambio repetido no puede duplicar artículos ni monedas.
- Las cosechas usan una referencia de tiempo fiable y no dependen del reloj local.
- Las partidas se pueden restaurar en otro dispositivo.
- Los jugadores pueden bloquear a otros y controlar quién visita su granja.
- Hay una forma clara de cerrar sesión y eliminar una cuenta.
- Arte y animaciones se revisan en el dispositivo real; no marcar sprites de referencia como arte final.

## Alcance y costes
Primero validar el bucle de juego y las visitas asíncronas con una beta pequeña. Medir uso real antes de pagar infraestructura escalable o añadir servidores de tiempo real. Supabase, las cuentas de las tiendas, el acceso a macOS/Xcode para iOS y el mantenimiento tendrán requisitos y costes propios; confirmarlos antes del lanzamiento.