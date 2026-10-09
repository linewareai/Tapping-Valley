# Inventario funcional de la versión web

Fuente revisada: `index.html` de `main`. Este documento es una primera extracción estática de nombres y funciones; cada comportamiento debe verificarse durante la migración.

## Sistemas detectados en el código
- Mascotas: `activePet`, `setPetImages`, `petReact`, `adoptPet`, `renderPet`, `care`.
- Economía y progresión: `totalUpgrades`, `bonus`, `tapValue`, `earn`, `buyUpgrade`.
- Cultivos y parcelas: `harvestAt`, `renderPlots`.
- Interfaz y colecciones: `renderUpgrades`, `renderInventory`, `renderQuests`, `renderMap`.
- Navegación y modales: `switchPage`, `openModal`, `closeModal`.
- Guardado y recuperación: `save`, `hasSavedGame`, `offlineProgress`, `resetGame`.
- Metas y comunicación: `checkAchievements`, `toast`, `pop`.
- Menú de inicio y depuración: `syncTitleMenu`, `showTitlePanel`, `enterGame`, `refreshDebugPanel`, `debugAction`, `openDebugPanel`, `closeDebugPanel`.
- Novedades: `showReleaseNotice`.

## Áreas que debe cubrir la migración
1. Núcleo y guardado versionado.
2. Monedas, recompensas, mejoras y fórmulas de coste.
3. Cultivos, parcelas, tiempos de crecimiento y cosecha.
4. Mascotas, adopción, selección y cuidado.
5. Inventario y recursos.
6. Misiones, logros y recompensas.
7. Mapa, navegación, páginas, modales, menús y notificaciones.
8. Progreso offline y recuperación de sesión.
9. Novedades y ajustes.
10. Herramientas de depuración solo para desarrollo.
11. Integración de arte, iconos, sonidos y animaciones.

## Recursos que ya existen
- `iconmimi.png`
- `assets/sprites/environment/farm-scene.svg`
- `assets/sprites/pets/mishi.svg`
- `assets/sprites/pets/lillia.svg`
- `assets/sprites/crops/carrot.svg` y `assets/sprites/crops/catalog.json`
- Categorías de sprites para personajes, mascotas, cultivos, entorno e interfaz.

## No asumir
- La lista de funciones no describe por sí sola todas las reglas o estados.
- Los README indican que parte del arte aún está pendiente.
- No se ha ejecutado el juego web ni se ha comparado cada pantalla en runtime.
- No se ha verificado una conversión de partidas guardadas.
