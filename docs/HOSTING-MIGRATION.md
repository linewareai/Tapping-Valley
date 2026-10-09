# Alojamiento de ValleyTapping

## Decisión recomendada

Separar el alojamiento del prototipo web del backend y del futuro cliente nativo:

- **Prototipo web:** Cloudflare Pages, conectado al repositorio de GitHub.
- **Juego móvil:** Godot para Android e iOS; la ejecución local del juego no dependerá del hosting web.
- **Funciones online:** proyecto Supabase exclusivo de ValleyTapping para autenticación, datos de granjas y operaciones sociales/económicas.

Cloudflare Pages es una buena primera opción para archivos estáticos. La documentación oficial indica que las solicitudes a recursos estáticos son gratuitas y sin límite de cantidad en Pages, mientras que Pages Functions consumen cuota de Workers. Límites y condiciones pueden cambiar, por lo que deben revisarse antes de un lanzamiento grande:
- https://developers.cloudflare.com/pages/platform/limits/
- https://developers.cloudflare.com/pages/functions/pricing/

GitHub Pages puede servir para demos pequeñas, pero no lo elegiría como destino principal de un juego con mercado entre jugadores. GitHub publica un límite de ancho de banda suave de 100 GB/mes y advierte que Pages no está pensado para ciertos usos de comercio electrónico o SaaS:
- https://docs.github.com/en/pages/getting-started-with-github-pages/github-pages-limits

## Pasos de migración

1. Crear/iniciar sesión en una cuenta Cloudflare.
2. Abrir Workers & Pages y crear un proyecto Pages conectado a `linewareai/ValleyTapping---Clicker`.
3. Para la versión actual HTML/JS estática, configurar la raíz del proyecto según los archivos reales del repositorio y no ejecutar un build si no hay paso de compilación.
4. Publicar primero una URL de prueba de Cloudflare Pages.
5. Probar en Android y iPhone: menú, guardado local, carga de sprites, PWA/service worker, modo offline y rutas.
6. Solo después de verificar el sitio, enlazar el dominio personalizado si existe y cambiar los enlaces compartidos.
7. Mantener Vercel disponible como fallback durante la comprobación; no borrar el proyecto ni cambiar DNS antes de validar el reemplazo.

## Importante

- La conexión del repositorio a Cloudflare requiere autorización del propietario desde su panel; no puede completarse solo mediante un commit de GitHub.
- No almacenar secretos de Supabase en el cliente.
- No usar el hosting estático para ejecutar lógica de saldo, inventario o comercio. Las transacciones deben validarse en el backend.
- La versión web actual es el prototipo. La versión nativa Godot y las funciones online todavía necesitan implementación.
