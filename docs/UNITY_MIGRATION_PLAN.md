# ValleyTapping: reconstrucción desde cero en Unity + C#

## Objetivo
Crear un juego instalable para Android y iOS. La versión web actual queda intacta en `main`; el trabajo de Unity vive en la rama `unity-rebuild`.

## Inventario preliminar verificado
La raíz actual incluye `index.html`, `assets/`, `docs/`, `iconmimi.png`, `manifest.webmanifest`, `release-notes.json` y `sw.js`. `index.html` es una aplicación web de aproximadamente 85 KB y `iconmimi.png` es un recurso de imagen de aproximadamente 1.4 MB. Todavía hay que inspeccionar recursivamente `assets/` y `docs/` antes de afirmar que conocemos todos los recursos.

## Principios
- No borrar ni reemplazar archivos de la versión web.
- No copiar assets a ciegas: primero catalogar formato, tamaño, uso y licencias.
- Mantener reglas de juego separadas de la interfaz.
- Versionar el modelo de guardado desde la primera versión.
- No marcar una función como migrada hasta probarla.
- No prometer builds móviles hasta ejecutarlas y verificarlas.

## Fases
1. Inventario de pantallas, mecánicas, economía, progresión, guardado, assets y dependencias.
2. Elegir una versión estable de Unity con soporte vigente para Android/iOS y usar el C# que esa versión soporte.
3. Prototipo: toque/clic para monedas, una mejora, interfaz táctil y guardado local persistente.
4. Pruebas de guardar/cargar, datos ausentes/corruptos, compra sin saldo suficiente y reinicio de aplicación.
5. Añadir mascotas, granja y progresión como módulos separados.
6. Diseñar online/multijugador solo cuando estén definidos identidad, backend, sincronización y protección contra trampas.
7. Preparar builds firmadas y pruebas en dispositivos reales para publicación.

## Arquitectura inicial
- `Core`: ciclo de vida y coordinación de la sesión.
- `Economy`: saldo, recompensas y costes.
- `Upgrades`: mejoras y fórmulas de progresión.
- `SaveSystem`: modelo versionado y almacenamiento local.
- `UI`: presentación y entrada táctil.
- `Pets` / `Farm`: módulos posteriores.

## Primer hito verificable
Poder tocar para ganar monedas, comprar una mejora, cerrar/reabrir la app y recuperar el saldo y el coste del siguiente nivel. El prototipo debe funcionar sin conexión.

## Pendientes
- Enumerar recursivamente todos los archivos de `assets/` y `docs/`.
- Leer el sistema actual de guardado en `index.html` y diseñar una conversión si procede.
- Seleccionar versión de Unity tras revisar requisitos del entorno de compilación.
- Importar y verificar recursos originales en Unity sin modificarlos.
