# Sistema extensible de contenido y sprites

## Cultivos
- El catálogo de referencia está en `catalog.json`.
- Los sprites por cultivo viven en `final/<spriteFolder>/`, con nombres de etapa `01-seed.png` a `09-ripe_3.png`.
- Conserva el `id` de inventario una vez publicado para no romper partidas guardadas.
- Para añadir un cultivo: añade una entrada al catálogo, registra su producto en el inventario del juego, asigna una parcela mediante `plotCropIds` y sube sus nueve PNG transparentes.
- Usa una carpeta por cultivo y nombres de etapa estables. Evita codificar rutas por especie dentro del renderizador.
- Los PNG finales se intentan cargar primero; si faltan o fallan, el juego usa el SVG de respaldo existente.

## Expandir el universo
Usa IDs estables y datos declarativos para futuras zonas, NPC, recetas, edificios y misiones. Separa definición de contenido (datos) de lógica de interacción. No reutilices IDs de partidas guardadas y define los requisitos de desbloqueo explícitamente.

## Estado de arte
El ZIP de referencia incluye los sprites recortados. Antes de considerarlos arte final, revisar visualmente transparencia, márgenes y consistencia de las etapas. Mantener el master sheet como fuente, no usarlo directamente como sprite en el juego.
