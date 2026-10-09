# ValleyTapping: reconstrucción desde cero en Unity + C#

## Objetivo
Reconstruir la experiencia como juego instalable para Android e iOS. La versión HTML en `main` se conserva como referencia y respaldo; el trabajo Unity está aislado en `unity-rebuild`.

## Inventario preliminar verificado
La raíz contiene `index.html`, `assets/`, `docs/`, `iconmimi.png`, `manifest.webmanifest`, `release-notes.json` y `sw.js`. El código web incluye economía, mejoras, cultivos/parcelas, inventario, mascotas, cuidado, adopción, misiones, logros, mapa, navegación, modales, progreso offline, guardado automático, menú de inicio, ajustes, novedades y herramientas de depuración. El detalle de funciones detectadas está en `docs/HTML_FEATURE_INVENTORY.md`.

## Trabajo C# ya añadido en `unity-rebuild`
- Modelo de guardado versionado, con campos para economía, gemas, toques, nivel, parcelas, inventario, mejoras, mascotas, misiones y logros.
- Guardado local JSON con ruta persistente de Unity, migración de esquema y reemplazo con respaldo temporal.
- Ciclo inicial de clic y compra de mejora.
- Definiciones y reglas base de cultivos, plantación y cosecha.
- Definiciones y reglas base de adopción/cuidado de mascotas.
- Definiciones y reglas base de mejoras.
- Progreso de misiones y reclamación de recompensas.
- Desbloqueo de logros.
- Cálculo limitado de ganancias offline.

Estos módulos son el comienzo de la migración, no equivalen todavía a paridad funcional con la versión HTML. No se ha ejecutado Unity ni compilado el proyecto.

## Matriz de migración
| Sistema de la versión web | Destino C# | Estado |
| --- | --- | --- |
| Guardado y carga | `Core/SaveSystem.cs`, `Core/SaveData.cs` | Base implementada; falta prueba en Unity y conversión de la partida web |
| Monedas y clics | `Core/GameSession.cs` | Prototipo básico |
| Mejoras y economía | `Economy/UpgradeService.cs` | Reglas base; falta importar todas las definiciones y comprobar fórmulas exactas |
| Cultivos y parcelas | `Farm/FarmService.cs` | Reglas base; falta replicar todas las reglas de la web e integrar la UI |
| Mascotas y cuidado | `Pets/PetService.cs` | Base parcial; faltan necesidades, estados y reglas exactas |
| Inventario | `Core/SaveData.cs`, `Core/GameInventoryData.cs` | Modelo inicial; falta servicio y UI |
| Misiones | `Quests/QuestService.cs` | Base parcial; falta migrar catálogo y eventos |
| Logros | `Achievements/AchievementService.cs` | Base parcial; falta migrar catálogo y eventos completos |
| Progreso offline | `Core/OfflineProgressService.cs` | Cálculo base; falta conectar al ciclo de sesión y necesidades de mascota |
| Mapa y desbloqueos de zonas | Pendiente | No implementado |
| Navegación, páginas, modales y menú de título | UI Unity pendiente | No implementado |
| Notificaciones y efectos de toque | UI/audio Unity pendiente | No implementado |
| Ajustes y créditos | UI Unity pendiente | No implementado |
| Novedades de versión | Flujo de distribución pendiente | No implementado en la app Unity |
| Panel de depuración | Herramientas de desarrollo pendiente | No implementado |
| Recursos visuales y animaciones | Importación/adaptación pendiente | Assets originales conservados en `main` |
| Audio | Inventario pendiente | No afirmar que existen recursos hasta auditarlos |
| Online, cuentas, visitas e intercambios | Backend y cliente pendientes | No implementado; requiere diseño de seguridad |

## Recursos que se deben conservar
- `iconmimi.png`
- `assets/sprites/environment/farm-scene.svg`
- `assets/sprites/pets/mishi.svg`, `lillia.svg`
- `assets/sprites/crops/carrot.svg`, `catalog.json`
- Otras carpetas de sprites en `assets/sprites/`.

No se han convertido ni importado aún todos los SVG a formatos Unity. Debe verificarse cada archivo y su apariencia antes de decidir si se rasteriza o se adapta.

## Reglas de seguridad y calidad
- No borrar ni reemplazar los archivos originales de la web.
- No copiar assets a ciegas ni declarar la migración completa por haber creado modelos.
- Mantener reglas del juego separadas de la UI.
- Versionar los datos guardados y no descartar partidas por errores de lectura.
- Probar límites de economía, compras sin saldo, guardado/carga, timestamps y pantallas pequeñas.
- No prometer builds móviles hasta ejecutarlas y verificarlas en dispositivos reales.

## Próximos hitos
1. Completar el inventario del estado y las reglas reales en el JavaScript.
2. Terminar el modelo de datos y servicios de dominio, incluyendo todas las mejoras, recursos, necesidades de mascotas, ciclos de cultivos, misiones, logros y zonas.
3. Crear la escena Unity, importar arte original y conectar UI.
4. Implementar conversión de guardado web si resulta técnicamente viable y probarla con copias de datos.
5. Compilar y probar Android; configurar iOS en un entorno macOS con toolchain compatible.
6. Añadir servicios online solo después de definir identidad, autoridad del servidor, sincronización y protección de transacciones.


## Actualización de avance
Se añadieron módulos de integración de escena:
- `Core/GameWorldController.cs`: conecta toque, economía, parcelas, mascotas, misiones, logros y ganancias offline a referencias de UI asignadas desde el Inspector.
- `Core/InventoryService.cs`: operaciones de añadir, consultar y retirar artículos.
- `Core/SceneNavigator.cs`: navegación entre paneles y apertura/cierre de modal.
- `Core/SettingsData.cs`: modelo de preferencias locales.

Importante: estos scripts aún requieren una escena configurada en Unity, definiciones ScriptableObject y pruebas en el Editor. No se ha ejecutado compilador Unity en este entorno. El proyecto no debe considerarse compilado o listo para instalar hasta validar todas las referencias, el flujo de guardado y los botones en Unity.
