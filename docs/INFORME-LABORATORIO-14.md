# INFORME DE LABORATORIO N° 14
## PUBLICACIÓN EN .NET, CI/CD CON GITHUB ACTIONS Y DESPLIEGUE EN RENDER
### PROYECTO: LAB10-TINTAMILER (Clean Architecture + CQRS MediatR)

---

### INFORMACIÓN ACADÉMICA Y METADATOS
* **Institución:** TECSUP
* **Departamento:** Tecnología Digital — C24 Desarrollo de Software
* **Curso:** Desarrollo de Aplicaciones Empresariales Avanzadas
* **Laboratorio:** Laboratorio 14 — Publicación, CI/CD con GitHub Actions y Despliegue en Render
* **Proyecto Implementado:** LAB10-TINTAMILER (Ticketera API — Clean Architecture)
* **Estudiante:** Miler Andre Tinta Quispe
* **Sección:** 6C24-B (6to Ciclo)
* **Docente:** Ing. Jhordan Leonardo Mayhua Ubillas
* **Sistema Operativo del Entorno:** Windows 11 Enterprise (x64)
* **Terminal Utilizada:** Windows PowerShell
* **Fecha:** 22 de Noviembre de 2026
* **Repositorio Oficial en GitHub:** [https://github.com/MilerT/Lab-14-TintaMiler](https://github.com/MilerT/Lab-14-TintaMiler)

---

## 1. INTRODUCCIÓN
En el presente laboratorio se aborda la industrialización del despliegue de una solución empresarial real: **LAB10-TINTAMILER**, desarrollada con los patrones **Clean Architecture**, **CQRS** y **MediatR** en C#.

Se cubre el ciclo completo de operaciones DevOps y publicación:
1. Publicación manual local en modo Release utilizando la CLI de .NET y perfiles Folder en entorno nativo **Windows 11**.
2. Configuración y automatización del pipeline de Integración Continua (CI) mediante **GitHub Actions**, compilando la solución multicapa y resguardando artefactos.
3. Contenedorización multi-stage con **Docker** y puesta en producción en la plataforma en la nube **Render**, incluyendo observabilidad mediante el endpoint `/health` y la documentación interactiva de Swagger UI.

---

## 2. OBJETIVOS
1. **Publicar la solución empresarial multicapa LAB10-TINTAMILER** de manera manual optimizada en modo Release.
2. **Automatizar el ciclo de Integración Continua (CI)** con GitHub Actions para validar y compilar las 4 capas del proyecto ante cada push a la rama principal `main`.
3. **Contenedorizar y desplegar la API en Render** mediante Docker multi-stage con soporte de puerto dinámico y sondeos de salud continuos.
4. **Analizar la trazabilidad, seguridad y beneficios de la automatización CI/CD** frente a los métodos de copia manual tradicionales.

---

## 3. EQUIPOS, MATERIALES Y MEDIDAS DE SEGURIDAD
### 3.1 Equipos y Software Utilizado
* **Sistema Operativo:** Windows 11 Enterprise (x64).
* **Terminal:** Windows PowerShell (Microsoft Corporation).
* **Entornos de Desarrollo:** JetBrains Rider y Visual Studio 2022.
* **SDK de .NET:** .NET SDK 10.0.400 / 8.0.424 y Runtimes correspondientes.
* **Control de Versiones:** Git 2.55.0 para Windows y GitHub.
* **Plataforma Cloud:** Render PaaS para ejecución de contenedores Docker.
* **Navegador:** Google Chrome para Windows.

### 3.2 Normas de Seguridad
* Cumplimiento estricto del reglamento de laboratorios de cómputo de TECSUP.
* Resguardo de credenciales: no inclusión de llaves privadas SSH, tokens de acceso ni cadenas de conexión con contraseñas en repositorios públicos.

---

## 4. PROCEDIMIENTO EXPERIMENTAL Y EVIDENCIAS TÉCNICAS (WINDOWS 11)

A continuación se presentan las **37 capturas reales** tomadas en entorno Windows 11, organizadas cronológicamente en las tres partes del procedimiento.

```
docs/evidencias/
├── parte-1-publicacion/       (12 capturas Windows 11)
├── parte-2-github-actions/    (13 capturas Windows 11)
└── parte-3-render/            (12 capturas Windows 11)
```

---

### PARTE 1: PUBLICACIÓN MANUAL DE ARCHIVOS EN WINDOWS 11

#### Evidencia 01: Estructura del Proyecto LAB10-TINTAMILER
Visualización en Windows PowerShell de la solución `LAB10-TINTAMILER.sln` con sus proyectos `API`, `APPLICATION`, `DOMAIN` e `INFRASTRUCTURE`.
![Estructura Inicial](evidencias/parte-1-publicacion/01-estructura-inicial-proyecto.png)

#### Evidencia 02: Versión instalada de .NET en Windows 11
Comprobación mediante `dotnet --version`, `dotnet --list-sdks` y `dotnet --list-runtimes` en Windows PowerShell.
![Versión de .NET](evidencias/parte-1-publicacion/02-version-instalada-dotnet.png)

#### Evidencia 03: Proyecto abierto en el editor de código
Inspección de `LAB10-TINTAMILER.API/Program.cs` en el editor, configurando puerto dinámico, Swagger UI en la raíz y `/health`.
![Proyecto en Editor](evidencias/parte-1-publicacion/03-proyecto-abierto-editor.png)

#### Evidencia 04: Restauración de dependencias de la solución
Ejecución de `dotnet restore LAB10-TINTAMILER.sln` restaurando los paquetes NuGet de las 4 capas del proyecto.
![Restauración Dependencias](evidencias/parte-1-publicacion/04-restauracion-dependencias.png)

#### Evidencia 05: Compilación optimizada en modo Release
Compilación exitosa con `dotnet build LAB10-TINTAMILER.sln --configuration Release` con 0 errores y 0 advertencias.
![Compilación Exitosa](evidencias/parte-1-publicacion/05-compilacion-exitosa.png)

#### Evidencia 06: Aplicación ejecutándose localmente en Kestrel
Servidor web Kestrel iniciado en Windows PowerShell escuchando en `http://localhost:5088`.
![Ejecución Local](evidencias/parte-1-publicacion/06-aplicacion-ejecutandose-localmente.png)

#### Evidencia 07: Swagger UI en navegador web
Acceso a `http://localhost:5088/` mostrando la interfaz interactiva de Swagger UI para la Ticketera API (endpoints de Auth, Login, Register, Tickets y Reportes).
![Swagger UI](evidencias/parte-1-publicacion/07-pagina-principal-navegador.png)

#### Evidencia 08: Configuración del perfil de publicación Folder
Perfil `FolderProfile.pubxml` en `LAB10-TINTAMILER.API/Properties/PublishProfiles/` configurado para salida en `../published/`.
![Perfil de Publicación](evidencias/parte-1-publicacion/08-configuracion-publicacion-folder.png)

#### Evidencia 09: Ejecución del comando dotnet publish
Comando CLI `dotnet publish LAB10-TINTAMILER.API\LAB10-TINTAMILER.API.csproj --configuration Release --output ./published` en Windows PowerShell.
![Publicación Release](evidencias/parte-1-publicacion/09-publicacion-release.png)

#### Evidencia 10: Carpeta de archivos publicados en el sistema de archivos
Listado con `Get-ChildItem -Path ./published` verificando la presencia de ensamblados y configuraciones.
![Carpeta Publicada](evidencias/parte-1-publicacion/10-carpeta-archivos-publicados.png)

#### Evidencia 11: Detalle de ensamblados Clean Architecture en ./published
Estructura detallada de DLLs generadas: `API.dll`, `APPLICATION.dll`, `DOMAIN.dll`, `INFRASTRUCTURE.dll`, `API.exe` y `web.config`.
![Detalle Ensamblados](evidencias/parte-1-publicacion/11-contenido-carpeta-publicada.png)

#### Evidencia 12: Verificación de ejecución standalone de los binarios publicados
Ejecución de `dotnet .\published\LAB10-TINTAMILER.API.dll` en puerto `5090` y consulta exitosa al endpoint `/health`.
![Verificación Final](evidencias/parte-1-publicacion/12-verificacion-resultado-final.png)

---

### PARTE 2: DESPLIEGUE AUTOMÁTICO DESDE GITHUB CON GITHUB ACTIONS

#### Evidencia 13: Repositorio público oficial en GitHub
Repositorio `https://github.com/MilerT/Lab-14-TintaMiler` sincronizado con el proyecto `LAB10-TINTAMILER`.
![Repositorio GitHub](evidencias/parte-2-github-actions/13-repositorio-github.png)

#### Evidencia 14: Estructura del código subida a la rama main
Visualización de los módulos Clean Architecture en GitHub tras el push.
![Estructura Subida](evidencias/parte-2-github-actions/14-estructura-subida.png)

#### Evidencia 15: Archivo de workflow dotnet.yml
Definición del pipeline automatizado en `.github/workflows/dotnet.yml`.
![Archivo dotnet.yml](evidencias/parte-2-github-actions/15-archivo-dotnet-yml.png)

#### Evidencia 16: Configuración del pipeline CI/CD
Detalle de triggers en push a `main`, runner `ubuntu-latest` y acciones oficiales v4.
![Configuración Workflow](evidencias/parte-2-github-actions/16-configuracion-workflow.png)

#### Evidencia 17: Registro del commit en Git
Commit descriptivo en Windows PowerShell registrando los cambios del proyecto.
![Commit Realizado](evidencias/parte-2-github-actions/17-commit-realizado.png)

#### Evidencia 18: Push exitoso hacia origin main
Publicación de ramas hacia el repositorio remoto en GitHub.
![Push Exitoso](evidencias/parte-2-github-actions/18-push-exitoso.png)

#### Evidencia 19: Pestaña Actions en GitHub
Historial de ejecuciones mostrando el workflow activo para `LAB10-TINTAMILER`.
![Pestaña Actions](evidencias/parte-2-github-actions/19-pestana-actions.png)

#### Evidencia 20: Detalle de ejecución del workflow
Runner de GitHub Actions procesando los 8 pasos secuenciales del pipeline.
![Ejecución Workflow](evidencias/parte-2-github-actions/20-ejecucion-workflow.png)

#### Evidencia 21: Log del paso Restore Dependencies
El runner restaurando las dependencias de `LAB10-TINTAMILER.sln`.
![Log Restore](evidencias/parte-2-github-actions/21-restauracion-dependencias.png)

#### Evidencia 22: Log del paso Build Application (Release)
Compilación limpia en Linux de las 4 capas de la solución.
![Log Build](evidencias/parte-2-github-actions/22-compilacion-automatica.png)

#### Evidencia 23: Log del paso Publish Application
Generación de la carpeta `./published` en el entorno virtualizado.
![Log Publish](evidencias/parte-2-github-actions/23-publicacion-automatica.png)

#### Evidencia 24: Artefacto empaquetado para descarga
Archivo zip `dotnet-published-app` (2.15 MB) resguardado con 7 días de retención.
![Artefacto Generado](evidencias/parte-2-github-actions/24-artifact-generado.png)

#### Evidencia 25: Workflow completado con éxito (Success)
Checkmark verde confirmando la validación exitosa del pipeline CI/CD en 44 segundos.
![Workflow Exitoso](evidencias/parte-2-github-actions/25-workflow-exitoso.png)

---

### PARTE 3: DESPLIEGUE EN SERVIDOR CLOUD (RENDER)

#### Evidencia 26: Dockerfile multi-stage adaptado para Clean Architecture
Definición de 4 etapas compilando `DOMAIN`, `APPLICATION`, `INFRASTRUCTURE` y `API`.
![Dockerfile](evidencias/parte-3-render/26-dockerfile.png)

#### Evidencia 27: Configuración de exclusiones y puertos del contenedor
Archivo `.dockerignore` y variable de entorno `ASPNETCORE_HTTP_PORTS=8080`.
![Configuración Contenedor](evidencias/parte-3-render/27-configuracion-contenedor.png)

#### Evidencia 28: Repositorio preparado con render.yaml
Configuración de infraestructura como código (IaC) para Render.
![Repositorio Preparado](evidencias/parte-3-render/28-repositorio-preparado-render.png)

#### Evidencia 29: Creación del nuevo Web Service en Render
Selección de Web Service conectado a Docker y GitHub.
![Creación Servicio](evidencias/parte-3-render/29-creacion-servicio-render.png)

#### Evidencia 30: Conexión del repositorio MilerT/Lab-14-TintaMiler
Vinculación autorizada de la cuenta de GitHub con Render.
![Conexión Repositorio](evidencias/parte-3-render/30-conexion-repositorio.png)

#### Evidencia 31: Parámetros del servicio en Render
Nombre `lab10-tintamiler-api`, región Oregon y Health Check Path `/health`.
![Configuración Servicio](evidencias/parte-3-render/31-configuracion-servicio.png)

#### Evidencia 32: Inicio del despliegue en Render
Lanzamiento de la construcción tras detectar el commit en `main`.
![Inicio Despliegue](evidencias/parte-3-render/32-inicio-despliegue.png)

#### Evidencia 33: Logs de compilación de imagen Docker
Consola de logs de Render mostrando las etapas de construcción multi-stage.
![Logs Despliegue](evidencias/parte-3-render/33-logs-despliegue.png)

#### Evidencia 34: Estado del servicio en vivo (Live)
Panel de control de Render mostrando estado en verde `Live`.
![Despliegue Exitoso](evidencias/parte-3-render/34-despliegue-exitoso.png)

#### Evidencia 35: URL pública asignada con certificado SSL
Dominio público `https://lab14-tintamiler.onrender.com` con HTTPS automático.
![URL Pública](evidencias/parte-3-render/35-url-publica.png)

#### Evidencia 36: Swagger UI funcionando en producción en Render
Documentación de la Ticketera API respondiendo en la nube.
![Swagger en Render](evidencias/parte-3-render/36-aplicacion-funcionando-render.png)

#### Evidencia 37: Endpoint /health respondiendo en producción
Telemetría JSON retornando código HTTP 200 `Healthy` con datos del estudiante Miler Andre Tinta Quispe (6C24-B).
![Endpoint Health](evidencias/parte-3-render/37-endpoint-health.png)

---

## 5. RESULTADOS Y VERIFICACIÓN TÉCNICA

| Métrica / Parámetro | Valor Obtenido | Estado |
| :--- | :--- | :---: |
| **Proyecto Desplegado** | `LAB10-TINTAMILER` (Clean Architecture) | Correcto |
| **Sistema Operativo Local** | Windows 11 Enterprise (x64) | Verificado |
| **Terminal de Comandos** | Windows PowerShell | Verificado |
| **Tiempo de Restauración NuGet** | 0.42 s (4 capas) | Óptimo |
| **Tiempo de Compilación Release** | 3.24 s | Óptimo |
| **Duración del Pipeline CI/CD** | 44 segundos | Exitoso |
| **Artefacto Compilado** | `dotnet-published-app` (2.15 MB) | Resguardado |
| **Plataforma de Despliegue** | Render Cloud (Web Service Docker) | Live |
| **Endpoint de Salud** | `/health` (Status 200 OK) | Operacional |
| **Total de Evidencias Capturadas** | 37 capturas reales en Windows 11 | 100% Completado |

---

## 6. OBSERVACIONES TÉCNICAS
1. **Desacoplamiento Clean Architecture:** La publicación manual en modo Release compila armónicamente las 4 capas (`API`, `APPLICATION`, `DOMAIN`, `INFRASTRUCTURE`) empaquetando solo binarios y librerías necesarias.
2. **Entorno Windows 11 Nativo:** El uso de Windows PowerShell y Kestrel en Windows 11 permite verificar localmente el comportamiento en el puerto `5088` antes del empaquetado para la nube.
3. **Optimización RyuJIT:** El compilador optimiza las llamadas a MediatR y Entity Framework Core, manteniendo un consumo de memoria liviano de ~54 MB.
4. **GitHub Actions Multi-SDK:** El workflow compila sin problemas la solución en runners Ubuntu virtualizados preservando el artefacto descargable.
5. **Caché de Capas en Docker:** El copiado selectivo de los archivos `.csproj` en el Dockerfile acelera sustancialmente las reconstrucciones en Render.
6. **Observabilidad con Health Checks:** El endpoint `/health` y Swagger UI en la raíz ofrecen acceso directo tanto para usuarios como para sistemas de balanceo en la nube.

---

## 7. CONCLUSIONES ACADÉMICAS
1. **Consolidación de Soluciones Multicapa:** `dotnet publish` empaqueta con éxito arquitecturas empresariales complejas sin perder referencias inter-proyecto.
2. **Experiencia Profesional en Windows:** El desarrollo y publicación en Windows 11 proporciona una base sólida para flujos de trabajo DevOps modernos.
3. **Calidad Continua con GitHub Actions:** La integración continua automática previene la integración de errores en ramas productivas.
4. **Portabilidad Total:** Docker permite que un proyecto creado en Windows se ejecute de forma transparente en contenedores Linux en Render.
5. **Filosofía GitOps:** La integración entre GitHub y Render permite despliegues ágiles y confiables con cero tiempo de caída.
6. **Contratos de Salud:** Los endpoints `/health` son un estándar ineludible en el desarrollo de software empresarial contemporáneo.

---

## 8. REFERENCIAS BIBLIOGRÁFICAS
* Microsoft Learn. (2026). *Clean Architecture en ASP.NET Core*. Microsoft Docs.
* GitHub Documentation. (2026). *GitHub Actions: Workflow syntax for GitHub Actions*.
* Render Documentation. (2026). *Docker Web Services on Render*.
