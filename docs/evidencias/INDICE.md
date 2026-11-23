# Índice de evidencias del Laboratorio 14

Miler Andre Tinta Quispe · 6C24-B · 22 de noviembre de 2026, Lima.

38 capturas reales. El orden corresponde al procedimiento de la guía. Se repitieron algunas verificaciones locales tras la compilación final. Las imágenes de terminal muestran PowerShell en ejecución, los registros remotos proceden de GitHub y Render.

## Publicación manual

| Paso | Evidencia PNG | Qué se comprobó | Estado |
|---:|---|---|---|
| 01 | [01-solucion-y-capas.png](parte-1-publicacion/01-solucion-y-capas.png) | Estructura de la solución. dotnet sln enumeró los cuatro proyectos que componen la Ticketera. | Capturada |
| 02 | [02-sdk-y-runtime.png](parte-1-publicacion/02-sdk-y-runtime.png) | SDK y runtimes instalados. La terminal muestra el SDK seleccionado y las versiones de ASP.NET Core y .NET disponibles. | Capturada |
| 03 | [03-solucion-en-rider.png](parte-1-publicacion/03-solucion-en-rider.png) | Solución abierta en Rider. El editor muestra ExcelReportService y el árbol de la solución original. | Capturada |
| 04 | [04-restaurar-dependencias.png](parte-1-publicacion/04-restaurar-dependencias.png) | Restauración de NuGet. La restauración forzada resolvió las dependencias de las cuatro capas. | Capturada |
| 05 | [05-compilar-release.png](parte-1-publicacion/05-compilar-release.png) | Compilación en Release. MSBuild produjo los cuatro ensamblados y terminó con cero advertencias y cero errores. | Capturada |
| 06 | [06-kestrel-local.png](parte-1-publicacion/06-kestrel-local.png) | Kestrel en el puerto 5088. El puerto en escucha y el JSON de salud identifican la instancia local. | Capturada |
| 07 | [07-swagger-local.png](parte-1-publicacion/07-swagger-local.png) | Swagger en el navegador local. La página de inicio de la API presenta la documentación interactiva de la Ticketera. | Capturada |
| 08 | [08-perfil-folder.png](parte-1-publicacion/08-perfil-folder.png) | Perfil de publicación en carpeta. FolderProfile configura Release, net8.0 y la carpeta de destino del proyecto API. | Capturada |
| 09 | [09-publicar-folder.png](parte-1-publicacion/09-publicar-folder.png) | Publicación del paquete. dotnet publish aplicó el perfil y escribió los archivos en published/net8.0. | Capturada |
| 10 | [10-ensamblados-publicados.png](parte-1-publicacion/10-ensamblados-publicados.png) | Archivos de distribución. La salida incluye las cuatro DLL del proyecto, los archivos del host y el esquema SQL. | Capturada |
| 11 | [11-runtime-publicado.png](parte-1-publicacion/11-runtime-publicado.png) | Manifiesto de runtime. El archivo runtimeconfig.json identifica el TFM y los dos frameworks compartidos requeridos. | Capturada |
| 12 | [12-api-publicada.png](parte-1-publicacion/12-api-publicada.png) | Ejecución desde la publicación. La instancia en el puerto 5090 responde al sondeo de salud y exige credenciales para consultar tickets. | Capturada |

## Integración continua con GitHub Actions

| Paso | Evidencia PNG | Qué se comprobó | Estado |
|---:|---|---|---|
| 13 | [13-repositorio-github.png](parte-2-github-actions/13-repositorio-github.png) | Repositorio del laboratorio. GitHub muestra el repositorio que contiene el código de la Ticketera. | Capturada |
| 14 | [14-archivos-del-workflow.png](parte-2-github-actions/14-archivos-del-workflow.png) | Cambios preparados para integración. El diff del índice identifica el workflow, el SDK fijado y los archivos de despliegue. | Capturada |
| 15 | [15-workflow-integracion.png](parte-2-github-actions/15-workflow-integracion.png) | Configuración del workflow. El workflow define el disparador en main, el runner Ubuntu, PostgreSQL y la compilación. | Capturada |
| 16 | [16-pruebas-y-artefacto.png](parte-2-github-actions/16-pruebas-y-artefacto.png) | Pruebas y distribución. La segunda mitad del workflow comprueba rutas HTTP y sube el paquete Release. | Capturada |
| 17 | [17-commit-aplicacion.png](parte-2-github-actions/17-commit-aplicacion.png) | Commit de la aplicación. Git registró la revisión d589200 con los cambios de publicación en .NET 8. | Capturada |
| 18 | [18-push-a-main.png](parte-2-github-actions/18-push-a-main.png) | Push de la rama principal. Git confirmó el avance de main en el remoto MilerT/Lab-14-TintaMiler. | Capturada |
| 19 | [19-acciones-en-github.png](parte-2-github-actions/19-acciones-en-github.png) | Pestaña Actions. La lista de workflows en GitHub permite localizar la ejecución correspondiente al push. | Capturada |
| 20 | [20-ejecucion-en-github.png](parte-2-github-actions/20-ejecucion-en-github.png) | Resumen de ejecución. La página oficial de la ejecución 45109846828 muestra el trabajo y el artefacto asociados. | Capturada |
| 21 | [21-restauracion-runner.png](parte-2-github-actions/21-restauracion-runner.png) | Restauración en Ubuntu. El registro descargado de GitHub documenta la restauración de los cuatro proyectos en el runner. | Capturada |
| 22 | [22-compilacion-runner.png](parte-2-github-actions/22-compilacion-runner.png) | Compilación automática. El log del runner presenta los ensamblados y el resultado de la compilación Release. | Capturada |
| 23 | [23-publicacion-runner.png](parte-2-github-actions/23-publicacion-runner.png) | Publicación automática. GitHub registró el comando de publicación y la ruta de salida en Linux. | Capturada |
| 24 | [24-artefacto-github.png](parte-2-github-actions/24-artefacto-github.png) | Artefacto descargable. La API de GitHub confirma el nombre del artefacto, su identificador y su tamaño. | Capturada |
| 25 | [25-pruebas-http-del-runner.png](parte-2-github-actions/25-pruebas-http-del-runner.png) | Pruebas HTTP y archivos Excel. El log muestra 200 en salud y reportes, 401 en tickets y la comprobación de integridad de los libros. | Capturada |

## Despliegue en Render

| Paso | Evidencia PNG | Qué se comprobó | Estado |
|---:|---|---|---|
| 26 | [26-dockerfile.png](parte-3-render/26-dockerfile.png) | Dockerfile de la API. La etapa SDK publica la aplicación y la imagen de runtime ejecuta la DLL con el usuario app. | Capturada |
| 27 | [27-imagen-docker.png](parte-3-render/27-imagen-docker.png) | Imagen Linux construida. Docker inspeccionó la imagen local y confirmó el sistema operativo, el usuario y el puerto 8080. | Capturada |
| 28 | [28-blueprint-validado.png](parte-3-render/28-blueprint-validado.png) | Validación del Blueprint. El CLI de Render validó render.yaml para el espacio de trabajo del laboratorio. | Capturada |
| 29 | [29-postgres-en-render.png](parte-3-render/29-postgres-en-render.png) | Base PostgreSQL privada. Render identifica la base de datos gratuita en Oregón y su estado disponible. | Capturada |
| 30 | [30-servicio-y-repositorio.png](parte-3-render/30-servicio-y-repositorio.png) | Origen del servicio web. La configuración remota vincula el servicio con el repositorio de GitHub y la rama main. | Capturada |
| 31 | [31-configuracion-del-servicio.png](parte-3-render/31-configuracion-del-servicio.png) | Configuración en Render. El servicio identifica el runtime Docker, el plan gratuito, la región, la URL y el sondeo de salud. | Capturada |
| 32 | [32-despliegue-inicial.png](parte-3-render/32-despliegue-inicial.png) | Despliegue en curso. El registro de despliegues captura la actualización del servicio y el commit de origen. | Capturada |
| 33 | [33-build-remoto.png](parte-3-render/33-build-remoto.png) | Registro de construcción remota. Los logs de Render muestran la exportación de capas y de la caché de construcción de Docker. | Capturada |
| 34 | [34-render-en-vivo.png](parte-3-render/34-render-en-vivo.png) | Despliegue activo. Render marca live y devuelve el identificador del commit desplegado. | Capturada |
| 35 | [35-verificacion-https.png](parte-3-render/35-verificacion-https.png) | Respuesta HTTPS pública. curl siguió la redirección a index.html y verificó el certificado TLS y la respuesta HTTP 200. | Capturada |
| 36 | [36-swagger-publico.png](parte-3-render/36-swagger-publico.png) | Swagger en producción. En la URL pública, Swagger despliega la operación de reportes de tickets con respuestas. | Capturada |
| 37 | [37-health-publico.png](parte-3-render/37-health-publico.png) | Salud del servicio público. El navegador muestra el JSON obtenido directamente de /health en Render. | Capturada |
| 38 | [38-reportes-desde-postgres.png](parte-3-render/38-reportes-desde-postgres.png) | Lectura de los tres reportes. La comprobación externa descargó cada XLSX, verificó su estructura ZIP y leyó los nombres de sus hojas. | Capturada |

La publicación se realizó con el perfil Folder desde la CLI. La configuración y el despliegue remotos se documentaron mediante las respuestas oficiales del CLI y la API de Render. El informe incluye la captura de Rider, la navegación por GitHub y Swagger en ambos entornos.
