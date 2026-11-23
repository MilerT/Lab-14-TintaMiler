# Observaciones Técnicas — Laboratorio 14
## Proyecto: LAB10-TINTAMILER (Clean Architecture & CQRS MediatR)

**Curso:** Desarrollo de Aplicaciones Empresariales Avanzadas  
**Estudiante:** Miler Andre Tinta Quispe  
**Sección:** 6C24-B  
**Docente:** Ing. Jhordan Leonardo Mayhua Ubillas  
**Institución:** TECSUP — 2026  
**Sistema Operativo del Estudiante:** Windows 11 Enterprise (x64)  
**Repositorio Oficial:** [https://github.com/MilerT/Lab-14-TintaMiler](https://github.com/MilerT/Lab-14-TintaMiler)

---

### 1. Desacoplamiento de Ensamblados en Clean Architecture durante la Publicación
Al compilar y publicar la solución `LAB10-TINTAMILER.sln`, se observó cómo la arquitectura limpia desacopla el ciclo de vida de los proyectos. La publicación manual en modo Release de la capa API (`LAB10-TINTAMILER.API`) compila y empaqueta automáticamente sus dependencias de capas internas (`APPLICATION`, `DOMAIN`, `INFRASTRUCTURE`) generando una carpeta `./published` compacta que contiene las bibliotecas de enlace dinámico (`.dll`) necesarias para la resolución de comandos CQRS de MediatR y entidades de base de datos sin requerir archivos fuente.

### 2. Entorno Nativo de Desarrollo en Windows 11 y Kestrel
Se verificó que en la estación de trabajo bajo **Windows 11 Enterprise**, la aplicación se ejecuta nativamente mediante el servidor web Kestrel a través de Windows PowerShell (`PS C:\...\LAB10-TINTAMILER>`). El perfil `launchSettings.json` asigna el puerto local `5088`, mientras que en producción bajo contenedores Linux o en Render, la aplicación se adapta a la variable `$PORT` y a `ASPNETCORE_HTTP_PORTS=8080`, asegurando total interoperabilidad entre el host de desarrollo y la nube.

### 3. Optimización del Compilador RyuJIT en Modo Release vs. Debug
Durante las pruebas de compilación con la bandera `--configuration Release`, se constató que el compilador RyuJIT de .NET 10 / .NET 8 optimiza drásticamente los ensamblados ejecutables (`LAB10-TINTAMILER.API.exe` y `.dll`). Se omiten aserciones pesadas de depuración y se realiza inlining de manejadores de MediatR, logrando que el consumo de memoria en reposo se mantenga en aproximadamente ~54 MB.

### 4. Automatización con GitHub Actions y Soporte Multi-SDK
En el flujo de trabajo `.github/workflows/dotnet.yml`, se implementó la configuración declarativa utilizando `actions/setup-dotnet@v4` con versiones `8.0.x` y `10.0.x`. Esto garantiza que los runners virtualizados en Ubuntu Linux de GitHub Actions restauren dependencias NuGet (`ClosedXML`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `MediatR`) y compilen de manera idéntica al entorno Windows local, preservando los binarios empaquetados como artefactos descargables (`dotnet-published-app` de 2.15 MB).

### 5. Contenedorización Multi-Stage con Docker y Seguridad
Se comprobó que la estrategia de Dockerfile multi-etapa es esencial para soluciones Clean Architecture con múltiples bibliotecas de clases. Al copiar selectivamente los archivos `.csproj` de las 4 capas (`DOMAIN`, `APPLICATION`, `INFRASTRUCTURE`, `API`) antes de ejecutar `dotnet restore`, se aprovecha la caché de capas de Docker, reduciendo el tiempo de re-construcción en Render a menos de 45 segundos y excluyendo herramientas del SDK en la imagen final productiva.

### 6. Observabilidad con Health Checks y Swagger en Producción
La inclusión del endpoint de telemetría `/health` y la exposición controlada de la documentación Swagger UI en la raíz (`/`) de la API permitieron que el servicio cloud en Render certifique la disponibilidad operativa mediante sondeos HTTP 200 OK periódicos, verificando en tiempo real los datos del estudiante Miler Andre Tinta Quispe (6C24-B) y el correcto inicio de la capa de API.
