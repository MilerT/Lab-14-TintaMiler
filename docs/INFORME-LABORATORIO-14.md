# Laboratorio 14 · Publicación de la API de tickets LAB10-TINTAMILER

**TECSUP · Tecnología Digital · Desarrollo de Software**

Curso de Desarrollo de Aplicaciones Empresariales Avanzadas. Estudiante Miler Andre Tinta Quispe, sección 6C24-B. Docente Ing. Jhordan Leonardo Mayhua Ubillas. Lima, 22 de noviembre de 2026.

[Informe en PDF](INFORME-LABORATORIO-14.pdf) · [Informe en Word](INFORME-LABORATORIO-14-VERIFICADO.docx) · [Índice de las 38 capturas](evidencias/INDICE.md)

## Introducción

La Ticketera reúne autenticación, gestión de tickets y reportes Excel en una API de ASP.NET Core. El laboratorio siguió esa misma solución desde la compilación en Windows hasta su ejecución pública en Render. El identificador de Git permitió relacionar el código local con las pruebas de GitHub y el contenedor desplegado.

## Objetivos

1. Publicar en una carpeta local una versión Release que cargue las cuatro capas de la Ticketera en .NET 8.
2. Automatizar la compilación y comprobar el comportamiento HTTP del paquete publicado frente a PostgreSQL.
3. Desplegar la imagen en Render y verificar por HTTPS la documentación, la disponibilidad y la descarga de reportes.

## Herramientas y evidencias

Windows, Rider, PowerShell, SDK .NET 8.0.424, NuGet, Git, GitHub Actions, Docker Desktop, Render, PostgreSQL 18 y Chrome. Las capturas muestran Rider, páginas reales del navegador y sesiones de PowerShell ejecutadas en una terminal web local. Los registros de CI proceden del runner de GitHub y los del despliegue se consultaron con el CLI oficial de Render.

## Parte 1 · Publicación manual

La solución abierta en Rider contiene API, Application, Domain e Infrastructure. Los cuatro proyectos se ajustaron a net8.0 y global.json seleccionó el SDK 8.0.424 instalado en Windows. Las dependencias de autenticación y el proveedor de PostgreSQL se alinearon con ese destino.

Se restauraron los paquetes NuGet y se compiló Release. Kestrel atendió en el puerto 5088 y Swagger presentó los controladores de autenticación, tickets y reportes. El perfil FolderProfile se aplicó desde la CLI y escribió en published/net8.0.

Se revisaron los ensamblados y el manifiesto de runtime. Una segunda instancia arrancó desde la carpeta publicada en el puerto 5090. El sondeo de salud devolvió Healthy y una solicitud sin credenciales a los tickets recibió HTTP 401.

```text
dotnet restore LAB10-TINTAMILER.sln
dotnet build LAB10-TINTAMILER.sln -c Release --no-restore
dotnet run -c Release --no-build --project LAB10-TINTAMILER.API --launch-profile http
dotnet publish LAB10-TINTAMILER.API -c Release -f net8.0 -p:PublishProfile=FolderProfile
```

### 01. Estructura de la solución

![Estructura de la solución](evidencias/parte-1-publicacion/01-solucion-y-capas.png)

dotnet sln enumeró los cuatro proyectos que componen la Ticketera.

### 02. SDK y runtimes instalados

![SDK y runtimes instalados](evidencias/parte-1-publicacion/02-sdk-y-runtime.png)

La terminal muestra el SDK seleccionado y las versiones de ASP.NET Core y .NET disponibles.

### 03. Solución abierta en Rider

![Solución abierta en Rider](evidencias/parte-1-publicacion/03-solucion-en-rider.png)

El editor muestra ExcelReportService y el árbol de la solución original.

### 04. Restauración de NuGet

![Restauración de NuGet](evidencias/parte-1-publicacion/04-restaurar-dependencias.png)

La restauración forzada resolvió las dependencias de las cuatro capas.

### 05. Compilación en Release

![Compilación en Release](evidencias/parte-1-publicacion/05-compilar-release.png)

MSBuild produjo los cuatro ensamblados y terminó con cero advertencias y cero errores.

### 06. Kestrel en el puerto 5088

![Kestrel en el puerto 5088](evidencias/parte-1-publicacion/06-kestrel-local.png)

El puerto en escucha y el JSON de salud identifican la instancia local.

### 07. Swagger en el navegador local

![Swagger en el navegador local](evidencias/parte-1-publicacion/07-swagger-local.png)

La página de inicio de la API presenta la documentación interactiva de la Ticketera.

### 08. Perfil de publicación en carpeta

![Perfil de publicación en carpeta](evidencias/parte-1-publicacion/08-perfil-folder.png)

FolderProfile configura Release, net8.0 y la carpeta de destino del proyecto API.

### 09. Publicación del paquete

![Publicación del paquete](evidencias/parte-1-publicacion/09-publicar-folder.png)

dotnet publish aplicó el perfil y escribió los archivos en published/net8.0.

### 10. Archivos de distribución

![Archivos de distribución](evidencias/parte-1-publicacion/10-ensamblados-publicados.png)

La salida incluye las cuatro DLL del proyecto, los archivos del host y el esquema SQL.

### 11. Manifiesto de runtime

![Manifiesto de runtime](evidencias/parte-1-publicacion/11-runtime-publicado.png)

El archivo runtimeconfig.json identifica el TFM y los dos frameworks compartidos requeridos.

### 12. Ejecución desde la publicación

![Ejecución desde la publicación](evidencias/parte-1-publicacion/12-api-publicada.png)

La instancia en el puerto 5090 responde al sondeo de salud y exige credenciales para consultar tickets.

## Parte 2 · Integración continua con GitHub Actions

El push a main activó la ejecución 45109846828 para el commit d589200. Un runner Ubuntu restauró las dependencias, compiló las cuatro capas y publicó el proyecto API. La solución carece de proyectos de pruebas unitarias. La comprobación funcional se implementó con solicitudes HTTP al paquete publicado.

El runner inició PostgreSQL 18, aplicó TicketeraBD.sql y levantó la API. Comprobó el identificador de la aplicación, el documento OpenAPI y el rechazo de una petición sin token. También descargó dos libros Excel y validó la integridad ZIP de ambos.

GitHub conservó la salida en el artefacto ticketera-net8-release, de 7 035 336 bytes. La transferencia SCP del ejemplo de la guía se sustituyó por el artefacto descargable. Render obtiene el código directamente del repositorio.

```text
git push origin main
dotnet publish LAB10-TINTAMILER.API -c Release -f net8.0 --no-build -o published/net8.0
```

### 13. Repositorio del laboratorio

![Repositorio del laboratorio](evidencias/parte-2-github-actions/13-repositorio-github.png)

GitHub muestra el repositorio que contiene el código de la Ticketera.

### 14. Cambios preparados para integración

![Cambios preparados para integración](evidencias/parte-2-github-actions/14-archivos-del-workflow.png)

El diff del índice identifica el workflow, el SDK fijado y los archivos de despliegue.

### 15. Configuración del workflow

![Configuración del workflow](evidencias/parte-2-github-actions/15-workflow-integracion.png)

El workflow define el disparador en main, el runner Ubuntu, PostgreSQL y la compilación.

### 16. Pruebas y distribución

![Pruebas y distribución](evidencias/parte-2-github-actions/16-pruebas-y-artefacto.png)

La segunda mitad del workflow comprueba rutas HTTP y sube el paquete Release.

### 17. Commit de la aplicación

![Commit de la aplicación](evidencias/parte-2-github-actions/17-commit-aplicacion.png)

Git registró la revisión d589200 con los cambios de publicación en .NET 8.

### 18. Push de la rama principal

![Push de la rama principal](evidencias/parte-2-github-actions/18-push-a-main.png)

Git confirmó el avance de main en el remoto MilerT/Lab-14-TintaMiler.

### 19. Pestaña Actions

![Pestaña Actions](evidencias/parte-2-github-actions/19-acciones-en-github.png)

La lista de workflows en GitHub permite localizar la ejecución correspondiente al push.

### 20. Resumen de ejecución

![Resumen de ejecución](evidencias/parte-2-github-actions/20-ejecucion-en-github.png)

La página oficial de la ejecución 45109846828 muestra el trabajo y el artefacto asociados.

### 21. Restauración en Ubuntu

![Restauración en Ubuntu](evidencias/parte-2-github-actions/21-restauracion-runner.png)

El registro descargado de GitHub documenta la restauración de los cuatro proyectos en el runner.

### 22. Compilación automática

![Compilación automática](evidencias/parte-2-github-actions/22-compilacion-runner.png)

El log del runner presenta los ensamblados y el resultado de la compilación Release.

### 23. Publicación automática

![Publicación automática](evidencias/parte-2-github-actions/23-publicacion-runner.png)

GitHub registró el comando de publicación y la ruta de salida en Linux.

### 24. Artefacto descargable

![Artefacto descargable](evidencias/parte-2-github-actions/24-artefacto-github.png)

La API de GitHub confirma el nombre del artefacto, su identificador y su tamaño.

### 25. Pruebas HTTP y archivos Excel

![Pruebas HTTP y archivos Excel](evidencias/parte-2-github-actions/25-pruebas-http-del-runner.png)

El log muestra 200 en salud y reportes, 401 en tickets y la comprobación de integridad de los libros.

## Parte 3 · Despliegue en Render

El Dockerfile usa una etapa SDK para restaurar y publicar, seguida de una etapa ASP.NET Core que ejecuta la DLL con el usuario app. Docker Desktop construyó la imagen Linux. Render validó el Blueprint y los recursos se crearon mediante su API oficial en el espacio MilerT.

Se configuraron el servicio Docker lab10-tintamiler-api y una base PostgreSQL 18 privada, ambos en el plan gratuito de Oregón. El servicio toma la rama main, escucha en el puerto PORT asignado y usa /health. La inicialización opcional aplica las cinco tablas de TicketeraBD.sql en una transacción.

Render informó live para el despliegue dep-dc1rpabl550s73fc4kog. La verificación externa siguió la redirección a Swagger, leyó el JSON de salud y abrió los tres libros entregados por los endpoints públicos. El reporte de tickets incluyó las hojas Resumen de Tickets y Detalle de Respuestas.

```text
docker build -t lab10-tintamiler:lab14 .
render blueprints validate ./render.yaml -o text
render deploys list srv-dc1rpa3l550s73fc4jk0 -o json
```

### 26. Dockerfile de la API

![Dockerfile de la API](evidencias/parte-3-render/26-dockerfile.png)

La etapa SDK publica la aplicación y la imagen de runtime ejecuta la DLL con el usuario app.

### 27. Imagen Linux construida

![Imagen Linux construida](evidencias/parte-3-render/27-imagen-docker.png)

Docker inspeccionó la imagen local y confirmó el sistema operativo, el usuario y el puerto 8080.

### 28. Validación del Blueprint

![Validación del Blueprint](evidencias/parte-3-render/28-blueprint-validado.png)

El CLI de Render validó render.yaml para el espacio de trabajo del laboratorio.

### 29. Base PostgreSQL privada

![Base PostgreSQL privada](evidencias/parte-3-render/29-postgres-en-render.png)

Render identifica la base de datos gratuita en Oregón y su estado disponible.

### 30. Origen del servicio web

![Origen del servicio web](evidencias/parte-3-render/30-servicio-y-repositorio.png)

La configuración remota vincula el servicio con el repositorio de GitHub y la rama main.

### 31. Configuración en Render

![Configuración en Render](evidencias/parte-3-render/31-configuracion-del-servicio.png)

El servicio identifica el runtime Docker, el plan gratuito, la región, la URL y el sondeo de salud.

### 32. Despliegue en curso

![Despliegue en curso](evidencias/parte-3-render/32-despliegue-inicial.png)

El registro de despliegues captura la actualización del servicio y el commit de origen.

### 33. Registro de construcción remota

![Registro de construcción remota](evidencias/parte-3-render/33-build-remoto.png)

Los logs de Render muestran la exportación de capas y de la caché de construcción de Docker.

### 34. Despliegue activo

![Despliegue activo](evidencias/parte-3-render/34-render-en-vivo.png)

Render marca live y devuelve el identificador del commit desplegado.

### 35. Respuesta HTTPS pública

![Respuesta HTTPS pública](evidencias/parte-3-render/35-verificacion-https.png)

curl siguió la redirección a index.html y verificó el certificado TLS y la respuesta HTTP 200.

### 36. Swagger en producción

![Swagger en producción](evidencias/parte-3-render/36-swagger-publico.png)

En la URL pública, Swagger despliega la operación de reportes de tickets con respuestas.

### 37. Salud del servicio público

![Salud del servicio público](evidencias/parte-3-render/37-health-publico.png)

El navegador muestra el JSON obtenido directamente de /health en Render.

### 38. Lectura de los tres reportes

![Lectura de los tres reportes](evidencias/parte-3-render/38-reportes-desde-postgres.png)

La comprobación externa descargó cada XLSX, verificó su estructura ZIP y leyó los nombres de sus hojas.

## Resultados comprobados

| Entorno | Resultado |
|---|---|
| Windows | SDK 8.0.424, compilación limpia y ejecución del paquete en 5090 |
| GitHub | [Ejecución 45109846828](https://github.com/MilerT/Lab-14-TintaMiler/actions/runs/45109846828) completada con éxito, artefacto de 7 035 336 bytes |
| Render | [Swagger](https://lab10-tintamiler-api.onrender.com/) y [salud](https://lab10-tintamiler-api.onrender.com/health) públicos, despliegue activo |
| PostgreSQL y Excel | Los dos reportes de consulta y el ejemplo devolvieron XLSX válidos |

La base gratuita tiene vencimiento el 22 de diciembre de 2026 a las 23:09 (Lima), según el valor expiresAt informado por Render.

## Problemas y soluciones

**Versión del proyecto.** La solución original apuntaba a net10.0. Se ajustaron los proyectos, el perfil de publicación, el workflow y las imágenes Docker para que el mismo código se ejecute con .NET 8.

**Archivos en uso.** Durante una recompilación, Windows bloqueó DLL que Kestrel tenía abiertas. Se detuvo la instancia de prueba, se recompiló y se volvió a publicar. La compilación definitiva terminó con cero advertencias y cero errores.

**Configuración sensible.** La configuración anterior incluía una clave JWT y una contraseña de PostgreSQL. Se retiraron del código y de appsettings.json. El desarrollo usa secretos de usuario y el servicio remoto recibe valores propios mediante variables de entorno.

**Verificación de la nube.** El control visual del navegador se interrumpió. La creación de recursos se completó con la API de Render y su estado se consultó con el CLI. Las comprobaciones de Swagger, HTTPS y /health se hicieron contra la URL pública.

## Observaciones

1. El SDK seleccionado fue 8.0.424 aunque la máquina también tenía SDK 10.0. El manifiesto publicado solicita Microsoft.NETCore.App y Microsoft.AspNetCore.App de la familia 8.0.
2. La publicación colocó en una sola carpeta la DLL de la API, los tres ensamblados de las demás capas, las dependencias y el esquema SQL. El arranque desde esa carpeta permitió probar la distribución independientemente de Rider.
3. El log de compilación del runner registró cero advertencias y cero errores. El paso funcional distinguió una petición válida a /health (200) de una solicitud anónima a /api/tickets (401).
4. GitHub identificó el artefacto de la ejecución con el número 14788289219 y registró su tamaño en bytes. El workflow lo conserva siete días.
5. El proceso de Render alcanzó el estado live con el mismo commit evaluado por GitHub. La URL pública abrió Swagger por HTTPS y mostró los endpoints de la API de tickets.
6. El endpoint de salud identifica el proceso. La descarga y apertura del reporte de usuarios y del reporte de tickets permitió verificar, además, el acceso a PostgreSQL y la generación de Excel.

## Conclusiones

1. Fijar el destino y la selección del SDK hizo reproducible la publicación de una solución que antes dependía de .NET 10. El resultado se ejecutó tanto en Windows como en Linux.
2. Arrancar el paquete Release desde su carpeta de salida fue una comprobación más completa que la compilación aislada. Permitió verificar la carga de la aplicación y el comportamiento del middleware de autenticación.
3. El runner de GitHub reprodujo la compilación en un entorno independiente. Incorporar PostgreSQL y peticiones HTTP permitió comprobar el funcionamiento de la API aun sin una suite de pruebas unitarias.
4. El artefacto vincula un conjunto concreto de binarios con el commit que los produjo y permite recuperarlos durante la retención configurada. Esta distribución evita depender del servidor SCP de ejemplo.
5. El contenedor y las variables de entorno permitieron desplegar las cuatro capas con una base privada propia. La configuración de Render mantuvo las credenciales fuera del código publicado.
6. La evidencia conjunta de Git, los logs del runner, el estado de Render y los archivos Excel descargados permite rastrear el resultado desde el cambio de código hasta el comportamiento que observa una persona usuaria.

## Referencias

Guía de trabajo de TECSUP, PLAB-S14-JMAYHUA-2026-01, Laboratorio 14. [Repositorio de la Ticketera](https://github.com/MilerT/Lab-14-TintaMiler) · [Ejecución de GitHub Actions](https://github.com/MilerT/Lab-14-TintaMiler/actions/runs/45109846828) · [Servicio público de Render](https://lab10-tintamiler-api.onrender.com) · [Especificación de Blueprints](https://render.com/docs/blueprint-spec).
