# Sprite assets

Los recursos visuales del juego viven aquí, separados por función para que se puedan sustituir o ampliar sin mezclar arte con código.

## Estructura

- `environment/`: escenarios y fondos del mundo.
- `pets/`: sprites de Mishi y futuras mascotas.
- `crops/`: cultivos, semillas y cosechas (pendiente de incorporar recursos individuales).
- `buildings/`: edificios y elementos construibles (pendiente).
- `ui/`: marcos, iconos y elementos de interfaz (pendiente).
- `characters/`: jugador y personajes no jugables (pendiente).

## Recursos integrados

- `environment/farm-scene.svg`: escenario principal de la granja.
- `pets/mishi.svg`: sprite actual de Mishi.

## Convenciones

- Mantener cada recurso en su categoría, con nombres descriptivos en kebab-case.
- Preferir sprites individuales o spritesheets con una hoja de referencia cuando corresponda.
- Al cambiar un recurso cacheado, actualizar la versión de URL y el caché del service worker.
- No sustituir arte final por emojis o formas CSS cuando exista un recurso gráfico específico.
