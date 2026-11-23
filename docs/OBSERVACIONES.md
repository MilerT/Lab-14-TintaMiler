# Observaciones del Laboratorio 14

1. El SDK seleccionado fue 8.0.424 aunque la máquina también tenía SDK 10.0. El manifiesto publicado solicita Microsoft.NETCore.App y Microsoft.AspNetCore.App de la familia 8.0.
2. La publicación colocó en una sola carpeta la DLL de la API, los tres ensamblados de las demás capas, las dependencias y el esquema SQL. El arranque desde esa carpeta permitió probar la distribución independientemente de Rider.
3. El log de compilación del runner registró cero advertencias y cero errores. El paso funcional distinguió una petición válida a /health (200) de una solicitud anónima a /api/tickets (401).
4. GitHub identificó el artefacto de la ejecución con el número 14788289219 y registró su tamaño en bytes. El workflow lo conserva siete días.
5. El proceso de Render alcanzó el estado live con el mismo commit evaluado por GitHub. La URL pública abrió Swagger por HTTPS y mostró los endpoints de la API de tickets.
6. El endpoint de salud identifica el proceso. La descarga y apertura del reporte de usuarios y del reporte de tickets permitió verificar, además, el acceso a PostgreSQL y la generación de Excel.
