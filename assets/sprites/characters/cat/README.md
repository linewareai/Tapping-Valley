# Mascotas: gato gris y blanco

Primera mascota de referencia para ValleyTapping, inspirada en la gatita de la foto compartida por el usuario. **“Campi” es solo el nombre interno de referencia**: el nombre visible debe poder personalizarse por jugador y no debe quedar codificado en el arte ni en la lógica.

## Archivos

- `idle-01.svg`: pose neutral.
- `idle-02-blink.svg`: variante de parpadeo.
- `walk-01.svg` a `walk-04.svg`: ciclo de caminar, en orden.
- `click-01.svg`: reacción alegre al clic.
- `sleep-01.svg`: pose de descanso.
- `celebrate-01.svg`: celebración con destellos.

## Especificaciones

- Cada archivo es un sprite individual SVG de 64 × 64.
- Fondo transparente; bordes duros para conservar el aspecto pixel-art.
- Paleta basada en la foto: pelaje gris pizarra, zonas blancas en cara y pecho, ojos oscuros y collar morado con aro metálico.
- Los archivos son recursos fuente separados, no una sprite sheet ni una imagen de presentación.
- El sistema de mascotas debe admitir futuros tipos de animal y un nombre personalizado independiente del identificador interno.

Estos sprites son una primera versión vectorial de estilo pixel-art. Conviene probarlos a tamaño real en el juego y ajustar las poses/colisiones antes de integrarlos como animaciones definitivas.
