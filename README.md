# ValleyTapping - Clicker 🌱🐈

Primera versión jugable de un clicker de granja para Android, hecha con HTML, CSS y JavaScript, sin frameworks ni Lovable.

> **Dirección del proyecto:** evolucionar hacia un simulador acogedor de granja para Android y iPhone, con pixel art detallado top-down, cuentas, guardado en la nube, visitas a granjas de amigos y comercio seguro entre jugadores. La hoja de ruta describe la arquitectura prevista; estas funciones online y la versión Godot todavía no están implementadas.

## Hoja de ruta
Consulta [docs/ROADMAP-MOBILE-ONLINE.md](docs/ROADMAP-MOBILE-ONLINE.md) para la arquitectura propuesta, el modelo de datos inicial, las etapas de desarrollo y los criterios de seguridad para amistades, visitas, mercado y multijugador en tiempo real.

## Incluye en el prototipo web actual
- Toques para cosechar monedas y mejoras de clic.
- Producción automática por segundo.
- Cultivos, gallinero, vaca y manzanos como mejoras.
- Gato Mishi con hambre, felicidad, energía, vínculo y niveles.
- Alimentar, jugar y acariciar.
- Bonus de producción por vínculo con la mascota.
- Guardado automático local.
- Progreso offline limitado a 2 horas.
- Diseño adaptable a móvil y configuración PWA con caché offline.

## Probar
Abre `index.html` en un navegador. Para instalación PWA y service worker, sirve la carpeta desde HTTPS o localhost.

## Android
Publica la carpeta en HTTPS (por ejemplo GitHub Pages), abre la URL en Chrome Android y usa Menú → Instalar aplicación o Añadir a pantalla principal.

Esta versión es una PWA, no un APK nativo. Para Google Play puede empaquetarse después con Capacitor. El guardado es local y no sincroniza entre dispositivos.