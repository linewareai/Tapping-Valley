# ValleyTapping · Unity + C#

Este directorio contiene la base C# de la migración móvil. La versión HTML original se conserva en `main`; los cambios de Unity están aislados en `unity-rebuild`.

## Requisitos y arranque

- Unity LTS compatible con Android/iOS.
- Paquete **Unity UI (UGUI)** habilitado.
- **Antes de generar la escena**, instala **Vector Graphics** desde Package Manager (paquete oficial de Unity) para que los SVG originales se importen como recursos visuales. Los SVG originales se guardan en `Assets/OriginalArt_*.svg` para preservar la fuente. El generador busca los sprites importados y muestra el escenario de granja y Mishi en la escena cuando el importador los expone como `Sprite`.
- Android: Android Build Support, SDK/NDK y OpenJDK instalados desde Unity Hub.
- iOS: requiere macOS, Xcode y el módulo iOS Build Support.

## Crear la escena de prototipo

1. Crea un proyecto 2D en Unity LTS.
2. Copia el contenido de `Unity/Assets/` a la carpeta `Assets/` del proyecto.
3. Espera a que Unity compile los scripts.
4. Selecciona **ValleyTapping > Create Prototype Scene**.
5. Abre `Assets/ValleyTappingGenerated/Scenes/ValleyTappingPrototype.unity` y pulsa **Play**.
6. Prueba toques, compras, ocho parcelas, mascotas, alimentación/descanso, misiones y persistencia local.

El generador configura una columna desplazable para pantallas móviles, crea las definiciones de seis cultivos y las catorce familias de mejoras de la versión web, además de una mascota inicial y misiones de prueba. También intenta conectar el SVG original del escenario y el arte de Mishi a imágenes visibles de la escena; si el paquete SVG no está instalado o Unity no los expone como sprites, se muestran paneles de reserva en lugar de fallar.

## Qué está implementado y qué falta

### Base implementada
- Guardado JSON local con versión de esquema y progreso offline acotado.
- Economía, niveles, compras, parcelas con crecimiento, adopción y necesidades básicas de mascotas.
- Inventario, misiones, logros y ajustes como servicios/modelos.
- Generador de escena para probar los sistemas sin configurar cada referencia manualmente.
- Fuentes SVG originales copiadas al proyecto de Unity como material de importación.
- Escena generada con espacios visuales que intentan mostrar el escenario original y la mascota desde esos SVG.

### Pendiente antes de considerar la migración completa
- Verificar en Unity que los SVG se importen y se vean correctamente; conectar más sprites (cultivos, iconos de monedas/gemas y botones), asignar animaciones y recrear con precisión la composición, tarjetas, barra de recursos, navegación inferior, modales y efectos de la web.
- Replicar las fórmulas y todos los eventos del juego web. El catálogo de mejoras del prototipo ya está creado, pero algunas familias se aproximan a los tipos de efecto C# disponibles.
- Completar mapa/desbloqueos, recompensas y reclamación de misiones, inventario visual, ajustes, menú inicial, novedades y migración de partidas web.
- Compilar y probar en Unity Editor, Android real y una build iOS en macOS. Esos pasos todavía no se han verificado.

**Importante:** el generador crea una escena funcional para validar la base de sistemas, no una reproducción visual terminada del mockup ni un APK/IPA listo para instalar.
