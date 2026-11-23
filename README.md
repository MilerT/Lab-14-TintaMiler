# Laboratorio 14 — CI/CD, GitHub Actions y Despliegue en Render
## Proyecto: LAB10-TINTAMILER (Ticketera API — Clean Architecture & CQRS MediatR)

[![.NET](https://img.shields.io/badge/.NET-10.0%20%2F%208.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue)](https://learn.microsoft.com/dotnet/architecture/)
[![MediatR CQRS](https://img.shields.io/badge/Pattern-CQRS%20MediatR-purple)](https://github.com/jbogard/MediatR)
[![Windows 11](https://img.shields.io/badge/OS-Windows%2011%20Enterprise-0078D4?logo=windows11&logoColor=white)](https://www.microsoft.com/windows)
[![GitHub Actions CI](https://img.shields.io/badge/GitHub_Actions-Passed-success?logo=githubactions&logoColor=white)](https://github.com/MilerT/Lab-14-TintaMiler/actions)
[![Docker Multi-Stage](https://img.shields.io/badge/Docker-Multi--stage-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![Render](https://img.shields.io/badge/Render-Live-46E3B7?logo=render&logoColor=black)](https://lab14-tintamiler.onrender.com)
[![TECSUP](https://img.shields.io/badge/TECSUP-2026-0284C7)](https://www.tecsup.edu.pe/)

Repositorio oficial del **Laboratorio 14** del curso **Desarrollo de Aplicaciones Empresariales Avanzadas**, implementando la publicación manual, pipeline automatizado de integración continua con **GitHub Actions**, contenedorización con **Docker multi-stage** y puesta en producción en la plataforma en la nube **Render** de la solución empresarial **LAB10-TINTAMILER** en entorno **Windows 11**.

---

## 👨‍💻 Información Académica y del Entorno
* **Institución:** TECSUP
* **Departamento:** Tecnología Digital — C24 Desarrollo de Software
* **Curso:** Desarrollo de Aplicaciones Empresariales Avanzadas
* **Laboratorio:** Laboratorio 14 — Publicación en .NET, CI/CD y Despliegue en Render
* **Proyecto Implementado:** `LAB10-TINTAMILER` (Ticketera API)
* **Estudiante:** Miler Andre Tinta Quispe
* **Sección:** 6C24-B (6to Ciclo)
* **Docente:** Ing. Jhordan Leonardo Mayhua Ubillas
* **Sistema Operativo del Desarrollador:** Windows 11 Enterprise (x64)
* **Terminal:** Windows PowerShell
* **Repositorio:** [https://github.com/MilerT/Lab-14-TintaMiler.git](https://github.com/MilerT/Lab-14-TintaMiler.git)
* **URL Pública en Render:** [https://lab14-tintamiler.onrender.com](https://lab14-tintamiler.onrender.com)
* **Endpoint de Salud / Telemetría:** [https://lab14-tintamiler.onrender.com/health](https://lab14-tintamiler.onrender.com/health)

---

## 📁 Estructura del Repositorio

```
LAB10-TINTAMILER/
├── .github/
│   └── workflows/
│       └── dotnet.yml               # Pipeline CI/CD en GitHub Actions
├── LAB10-TINTAMILER.API/            # Capa de Presentación / Controladores / Program.cs
│   └── Properties/
│       ├── launchSettings.json      # Configuración de puerto local (5088)
│       └── PublishProfiles/
│           └── FolderProfile.pubxml # Perfil de publicación Folder
├── LAB10-TINTAMILER.APPLICATION/    # Casos de uso, CQRS (Commands & Queries), MediatR
├── LAB10-TINTAMILER.DOMAIN/         # Entidades de negocio, Value Objects, Interfaces
├── LAB10-TINTAMILER.INFRASTRUCTURE/ # Persistencia PostgreSQL (Npgsql), ClosedXML
├── docs/
│   ├── evidencias/
│   │   ├── parte-1-publicacion/     # 12 capturas reales en Windows 11
│   │   ├── parte-2-github-actions/  # 13 capturas de GitHub Actions
│   │   └── parte-3-render/          # 12 capturas de despliegue en Render
│   ├── INFORME-LABORATORIO-14.md    # Informe académico completo en Markdown
│   ├── INFORME-LABORATORIO-14.docx  # Informe oficial en Word con 37 imágenes Windows
│   ├── OBSERVACIONES.md             # 6 observaciones técnicas verificadas
│   └── CONCLUSIONES.md              # 6 conclusiones académicas verificadas
├── scripts/                         # Scripts de automatización y evidencias
├── Dockerfile                       # Construcción multi-stage .NET (SDK + Runtime)
├── .dockerignore                    # Exclusión de binarios y temporales
├── .gitignore                       # Reglas de exclusión para .NET y Windows
├── render.yaml                      # Blueprint de despliegue para Render
├── LAB10-TINTAMILER.sln             # Solución de Visual Studio / Rider
└── README.md
```

---

## 🛠️ Tecnologías y Arquitectura

* **Patrón de Arquitectura:** Clean Architecture (API, Application, Domain, Infrastructure).
* **Patrón de Diseño:** CQRS (Command Query Responsibility Segregation) con MediatR.
* **Lenguaje:** C# 12.
* **Framework:** ASP.NET Core (.NET 10.0 / .NET 8.0).
* **Base de Datos:** PostgreSQL con `Npgsql.EntityFrameworkCore.PostgreSQL`.
* **Exportación y Reportes:** `ClosedXML`.
* **Seguridad:** Autenticación JWT Bearer Token y encriptación con `BCrypt.Net-Next`.
* **Documentación API:** Swagger / OpenAPI interactivo en la raíz (`/`).
* **Sistema Operativo del Desarrollador:** Windows 11 Enterprise con Windows PowerShell.
* **Integración Continua:** GitHub Actions (`actions/checkout@v4`, `actions/setup-dotnet@v4`, `actions/upload-artifact@v4`).
* **Cloud & Contenedores:** Docker multi-stage y Render PaaS.

---

## 🚀 Guía de Instalación y Ejecución en Windows 11

### Prerrequisitos
* Sistema Operativo: Windows 11 (o compatible).
* [.NET SDK](https://dotnet.microsoft.com/download) instalado.
* Git para Windows instalado.

### Pasos
1. **Clonar el repositorio:**
   ```powershell
   git clone https://github.com/MilerT/Lab-14-TintaMiler.git
   cd Lab-14-TintaMiler
   ```

2. **Restaurar dependencias de la solución:**
   ```powershell
   dotnet restore LAB10-TINTAMILER.sln
   ```

3. **Compilar en modo Release:**
   ```powershell
   dotnet build LAB10-TINTAMILER.sln --configuration Release
   ```

4. **Ejecutar la API localmente:**
   ```powershell
   dotnet run --project LAB10-TINTAMILER.API\LAB10-TINTAMILER.API.csproj
   ```

5. **Acceder a los endpoints:**
   * Swagger UI: `http://localhost:5088/`
   * Endpoint de Salud: `http://localhost:5088/health`

---

## 📦 Parte 1: Publicación Manual en Windows 11

Para empaquetar la API y sus 3 capas asociadas para producción:
```powershell
dotnet publish LAB10-TINTAMILER.API\LAB10-TINTAMILER.API.csproj --configuration Release --output ./published
```

Los artefactos resultantes se almacenan en la carpeta `./published` conteniendo:
* `LAB10-TINTAMILER.API.dll` y `LAB10-TINTAMILER.API.exe`: Host principal de la API.
* `LAB10-TINTAMILER.APPLICATION.dll`: Lógica de comandos y consultas MediatR.
* `LAB10-TINTAMILER.DOMAIN.dll`: Entidades de dominio.
* `LAB10-TINTAMILER.INFRASTRUCTURE.dll`: Repositorios y persistencia con PostgreSQL.
* `web.config`: Configuración para IIS o Reverse Proxy.

Para validar la ejecución standalone de los binarios:
```powershell
dotnet .\published\LAB10-TINTAMILER.API.dll --urls "http://localhost:5090"
```

---

## 🤖 Parte 2: Pipeline de GitHub Actions (CI/CD)

El archivo `.github/workflows/dotnet.yml` automatiza el ciclo de vida en cada push a la rama `main`:
1. **Check out code:** Descarga el repositorio usando `actions/checkout@v4`.
2. **Set up .NET:** Instala el entorno de SDK oficial con `actions/setup-dotnet@v4`.
3. **Restore dependencies:** Ejecuta `dotnet restore LAB10-TINTAMILER.sln`.
4. **Build application:** Compila la solución en modo Release con `dotnet build LAB10-TINTAMILER.sln --configuration Release --no-restore`.
5. **Publish application:** Publica la API hacia `./published`.
6. **Upload Artifacts:** Empaqueta y resguarda los binarios como artefacto descargable `dotnet-published-app` con `actions/upload-artifact@v4`.

---

## ☁️ Parte 3: Despliegue en Render con Docker Multi-Stage

El archivo `Dockerfile` multi-stage compila las 4 capas de forma óptima:
1. **Etapa 1 (base):** Imagen liviana de ejecución con puerto expuesto `8080`.
2. **Etapa 2 (build):** Imagen SDK que restaura selectivamente los `.csproj` y compila la API.
3. **Etapa 3 (publish):** Empaquetado final en modo Release.
4. **Etapa 4 (final):** Copia exclusiva de los binarios hacia la imagen base y ejecución con `dotnet LAB10-TINTAMILER.API.dll`.

### Características en Render:
* **Runtime:** Docker.
* **Región:** Oregon (US West).
* **Health Check Path:** `/health`.
* **Zero Downtime Deployment:** Despliegue automático sin tiempo de caída ante cada commit.

---

## 📸 Índice de Evidencias Capturadas en Windows 11 (37 Capturas)

### Parte 1: Publicación Manual (12 capturas)
| # | Nombre de Archivo | Descripción del Paso |
| :-: | :--- | :--- |
| 01 | `01-estructura-inicial-proyecto.png` | Estructura de LAB10-TINTAMILER en Windows PowerShell |
| 02 | `02-version-instalada-dotnet.png` | Verificación de .NET SDK y runtimes en Windows 11 |
| 03 | `03-proyecto-abierto-editor.png` | Inspección de Program.cs en el editor de código |
| 04 | `04-restauracion-dependencias.png` | Restauración exitosa de paquetes NuGet de la solución |
| 05 | `05-compilacion-exitosa.png` | Compilación Release de las 4 capas con 0 errores |
| 06 | `06-aplicacion-ejecutandose-localmente.png` | Servidor Kestrel en escucha en el puerto local 5088 |
| 07 | `07-pagina-principal-navegador.png` | Swagger UI de la Ticketera API en el navegador |
| 08 | `08-configuracion-publicacion-folder.png` | Perfil FolderProfile.pubxml en el editor |
| 09 | `09-publicacion-release.png` | Salida del comando dotnet publish en terminal |
| 10 | `10-carpeta-archivos-publicados.png` | Exploración de la carpeta local ./published |
| 11 | `11-contenido-carpeta-publicada.png` | Detalle de ensamblados DLL de las 4 capas y EXE |
| 12 | `12-verificacion-resultado-final.png` | Ejecución standalone de binarios y verificación en /health |

### Parte 2: GitHub Actions (13 capturas)
| # | Nombre de Archivo | Descripción del Paso |
| :-: | :--- | :--- |
| 13 | `13-repositorio-github.png` | Repositorio oficial sincronizado en GitHub |
| 14 | `14-estructura-subida.png` | Árbol de archivos de LAB10-TINTAMILER en GitHub |
| 15 | `15-archivo-dotnet-yml.png` | Código fuente del workflow en el editor |
| 16 | `16-configuracion-workflow.png` | Configuración de triggers y acciones oficiales v4 |
| 17 | `17-commit-realizado.png` | Registro formal del commit en Windows PowerShell |
| 18 | `18-push-exitoso.png` | Publicación de ramas hacia origin/main |
| 19 | `19-pestana-actions.png` | Vista general de la pestaña Actions |
| 20 | `20-ejecucion-workflow.png` | Ejecución en tiempo real del Workflow Run |
| 21 | `21-restauracion-dependencias.png` | Log de restauración de la solución en el runner |
| 22 | `22-compilacion-automatica.png` | Log de compilación en modo Release |
| 23 | `23-publicacion-automatica.png` | Log de empaquetado hacia ./published |
| 24 | `24-artifact-generado.png` | Paquete zip dotnet-published-app (2.15 MB) |
| 25 | `25-workflow-exitoso.png` | Workflow completado con éxito (Success) en verde |

### Parte 3: Render (12 capturas)
| # | Nombre de Archivo | Descripción del Paso |
| :-: | :--- | :--- |
| 26 | `26-dockerfile.png` | Definición de Dockerfile multi-stage en el editor |
| 27 | `27-configuracion-contenedor.png` | Archivo .dockerignore y puertos del contenedor |
| 28 | `28-repositorio-preparado-render.png` | Blueprint declarativo render.yaml |
| 29 | `29-creacion-servicio-render.png` | Creación de Web Service en Render |
| 30 | `30-conexion-repositorio.png` | Vinculación del repositorio en Render |
| 31 | `31-configuracion-servicio.png` | Configuración de Docker, región y /health |
| 32 | `32-inicio-despliegue.png` | Lanzamiento del proceso de despliegue |
| 33 | `33-logs-despliegue.png` | Consola de logs de Docker build en Render |
| 34 | `34-despliegue-exitoso.png` | Estado del servicio en vivo (Live) en verde |
| 35 | `35-url-publica.png` | URL pública asignada con certificado SSL |
| 36 | `36-aplicacion-funcionando-render.png` | Swagger UI de la Ticketera API respondiendo en la nube |
| 37 | `37-endpoint-health.png` | Telemetría JSON del endpoint /health |

---

## 📄 Documentación Adicional
* [Informe Académico Completo (Markdown)](docs/INFORME-LABORATORIO-14.md)
* [Informe Académico Oficial con Imágenes en Windows 11 (Word .docx)](docs/INFORME-LABORATORIO-14.docx)
* [Observaciones Técnicas Detalladas](docs/OBSERVACIONES.md)
* [Conclusiones Académicas Detalladas](docs/CONCLUSIONES.md)
