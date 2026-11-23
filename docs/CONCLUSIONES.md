# Conclusiones del Laboratorio 14

1. Fijar el destino y la selección del SDK hizo reproducible la publicación de una solución que antes dependía de .NET 10. El resultado se ejecutó tanto en Windows como en Linux.
2. Arrancar el paquete Release desde su carpeta de salida fue una comprobación más completa que la compilación aislada. Permitió verificar la carga de la aplicación y el comportamiento del middleware de autenticación.
3. El runner de GitHub reprodujo la compilación en un entorno independiente. Incorporar PostgreSQL y peticiones HTTP permitió comprobar el funcionamiento de la API aun sin una suite de pruebas unitarias.
4. El artefacto vincula un conjunto concreto de binarios con el commit que los produjo y permite recuperarlos durante la retención configurada. Esta distribución evita depender del servidor SCP de ejemplo.
5. El contenedor y las variables de entorno permitieron desplegar las cuatro capas con una base privada propia. La configuración de Render mantuvo las credenciales fuera del código publicado.
6. La evidencia conjunta de Git, los logs del runner, el estado de Render y los archivos Excel descargados permite rastrear el resultado desde el cambio de código hasta el comportamiento que observa una persona usuaria.
