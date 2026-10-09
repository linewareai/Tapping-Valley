# Versiones y novedades de ValleyTapping

## Requisito de producto

Cada despliegue publicado debe tener una versión identificable y una lista de novedades visible al abrir el juego. La ventana debe mostrar solo novedades posteriores a la última versión que cada jugador haya visto, permitir cerrar el aviso y ofrecer acceso a un historial de versiones anteriores.

## Fuente única de novedades

Mantener un manifiesto versionado, por ejemplo `release-notes.json`, con este formato:

```json
{
  "version": "0.2.0",
  "releasedAt": "2026-10-09",
  "title": "Tu granjita sigue creciendo",
  "changes": [
    { "type": "added", "text": "Se añadió una nueva interacción." },
    { "type": "improved", "text": "Se mejoró la experiencia en pantallas pequeñas." },
    { "type": "fixed", "text": "Se corrigió un detalle visual." }
  ]
}
```

Los ejemplos anteriores son ilustrativos, no afirman que esas funciones estén implementadas. El manifiesto real debe enumerar todos los cambios incluidos en ese lanzamiento, incluso los pequeños. No registrar funcionalidades futuras como terminadas.

## Comportamiento de la interfaz

- En cada arranque, consultar la versión publicada y compararla con la última versión vista.
- Si hay una versión nueva, mostrar una ventana accesible y adaptada a móviles con número de versión, fecha y lista de cambios.
- Guardar la versión vista por usuario; para visitantes, usar almacenamiento local como mínimo. Cuando haya cuentas y backend, guardar la preferencia en el perfil del usuario para sincronizarla entre dispositivos.
- Incluir una entrada permanente «Novedades» para consultar el historial completo.
- Si falla la red o el manifiesto no está disponible, el juego debe seguir abriendo y conservar el último historial válido.
- Evitar mostrar repetidamente la misma versión después de que el jugador la haya marcado como vista, salvo que exista una actualización crítica que se explique explícitamente.

## Automatización de publicación

La versión y sus notas deben formar parte del proceso de release. Cada cambio que vaya a producción debe incluir una entrada revisada en el manifiesto. Idealmente, el pipeline valida el esquema y rechaza un release si falta la versión o las notas. No mostrar como publicada una versión hasta que el despliegue de producción haya terminado correctamente.

## Estado de implementación

Este archivo define el requisito y el diseño. No significa que la ventana esté implementada, que exista ya el manifiesto consumido por la app ni que el pipeline de despliegue esté conectado. La integración funcional debe realizarse en la interfaz de inicio y en el flujo real de publicación.