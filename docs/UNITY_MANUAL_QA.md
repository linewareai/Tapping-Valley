# ValleyTapping Unity: checklist de pruebas manuales

No marques un punto como aprobado sin ejecutarlo en Unity. Adjunta versión de Unity, plataforma y resultado en cada ronda.

## Editor: prototipo y guardado
- [ ] Crear proyecto 2D en Unity Hub con una versión LTS compatible con Android/iOS.
- [ ] Copiar `Unity/Assets` a ese proyecto y esperar a que termine la importación.
- [ ] Crear escena `Main`, añadir EventSystem y Canvas con botones/textos.
- [ ] Añadir GameSession o GameWorldController a un GameObject y asignar todas las referencias serializadas.
- [ ] Pulsar el botón de toque: monedas y total de toques aumentan una vez por pulsación.
- [ ] Comprar una mejora con saldo suficiente y confirmar que aumenta el nivel.
- [ ] Intentar comprar sin saldo: el saldo no cambia y aparece un mensaje.
- [ ] Cerrar y abrir la aplicación: monedas, niveles y parcelas persisten.
- [ ] Simular archivo de guardado ausente, JSON inválido y versión de esquema anterior.
- [ ] Verificar que una escritura fallida no elimina el archivo de respaldo.
- [ ] Probar saldo muy alto y recompensa de cosecha/misión para detectar desbordamientos.

## Granja
- [ ] Intentar plantar en parcela inválida, parcela ocupada y con saldo insuficiente.
- [ ] Plantar cultivo válido, reiniciar la app y comprobar el tiempo restante.
- [ ] Intentar cosechar antes de tiempo y confirmar que no se obtiene recompensa.
- [ ] Cosechar cuando está listo y confirmar recompensa una sola vez.
- [ ] Verificar que una definición de cultivo ausente no borra la parcela.

## Mascotas, misiones y logros
- [ ] Adoptar con saldo suficiente e insuficiente.
- [ ] Impedir adoptar dos veces la misma mascota.
- [ ] Cambiar mascota activa solo a una mascota poseída.
- [ ] Cuidar mascota y confirmar que se conserva el progreso.
- [ ] Avanzar misión, impedir reclamar antes de completarla y evitar doble reclamación.
- [ ] Desbloquear cada logro una sola vez.

## Dispositivos
- [ ] Android: controles táctiles, suspensión/reanudación, guardado y orientación.
- [ ] iPhone: controles táctiles, safe areas, suspensión/reanudación y guardado.
- [ ] Probar varias proporciones de pantalla y texto escalado.
- [ ] Revisar consumo de batería, rendimiento y tamaño de build.

## Estado actual
Checklist creado, todavía sin ejecutar. No se ha confirmado compilación Unity ni build instalable.
