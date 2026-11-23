# Laboratorio 14 · Publicación de la Ticketera

Aplicación empresarial de gestión de tickets de Miler Andre Tinta Quispe, sección 6C24-B, TECSUP. El laboratorio publica la solución existente **LAB10-TINTAMILER**. La API expone autenticación JWT, tickets y reportes Excel. Application contiene los casos de uso y CQRS con MediatR, Domain define las entidades e Infrastructure conecta EF Core con PostgreSQL y genera los libros con ClosedXML.

[Swagger público](https://lab10-tintamiler-api.onrender.com/) · [Salud de la API](https://lab10-tintamiler-api.onrender.com/health) · [Ejecución verificada en GitHub Actions](https://github.com/MilerT/Lab-14-TintaMiler/actions/runs/45109846828)

## Entrega

| Material | Contenido |
|---|---|
| [Informe PDF](docs/INFORME-LABORATORIO-14.pdf) · [Informe Word](docs/INFORME-LABORATORIO-14-VERIFICADO.docx) · [Informe Markdown](docs/INFORME-LABORATORIO-14.md) | Procedimiento, resultados y capturas de las tres partes |
| [Carpeta de capturas](docs/evidencias/) · [Índice](docs/evidencias/INDICE.md) | 38 PNG con su paso, explicación y estado |
| [Observaciones](docs/OBSERVACIONES.md) · [Conclusiones](docs/CONCLUSIONES.md) | Seis observaciones y seis conclusiones |

## Ejecutar localmente

Se requiere Git, un SDK .NET 8 y PostgreSQL. La solución selecciona el SDK mediante `global.json`. La comprobación en Windows utilizó 8.0.424. En una nueva instalación, cree una base para la Ticketera, configure su conexión local y guarde una clave aleatoria de firma mediante Secret Manager. Las variables `ConnectionStrings__DefaultConnection` y `JwtSettings__SecretKey` también permiten configurar el proceso.

```powershell
git clone https://github.com/MilerT/Lab-14-TintaMiler.git
cd Lab-14-TintaMiler
dotnet restore LAB10-TINTAMILER.sln
dotnet build LAB10-TINTAMILER.sln -c Release --no-restore

# Para una instalación nueva, escriba la cadena Npgsql en la entrada protegida.
$conexion = Read-Host 'Cadena de conexión a PostgreSQL' -MaskInput
dotnet user-secrets set 'ConnectionStrings:DefaultConnection' $conexion --project LAB10-TINTAMILER.API
$clave = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
dotnet user-secrets set 'JwtSettings:SecretKey' $clave --project LAB10-TINTAMILER.API

# Aplica TicketeraBD.sql a una base dedicada existente.
$env:Ticketera__InitializeSchema = 'true'
dotnet run --project LAB10-TINTAMILER.API -c Release --launch-profile http
```

Los comandos de configuración utilizan PowerShell 7. El perfil HTTP abre Kestrel en el puerto 5088. Swagger se encuentra en la raíz y el estado del proceso está en `/health`. La consulta anónima a `/api/tickets` debe responder 401. Los reportes de lectura corresponden a `/api/reports/users-with-tickets` y `/api/reports/tickets-with-responses`.

## Publicar en una carpeta

El perfil FolderProfile selecciona Release y net8.0. La salida contiene el host, los ensamblados de las cuatro capas, los manifiestos, las dependencias y el esquema SQL. Para repetir la prueba local del paquete, detenga primero la instancia que usa los archivos y ejecute lo siguiente desde la raíz.

```powershell
dotnet publish LAB10-TINTAMILER.API -c Release -f net8.0 -p:PublishProfile=FolderProfile
Push-Location published/net8.0
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet LAB10-TINTAMILER.API.dll --urls http://127.0.0.1:5090
# Tras detener el proceso con Ctrl+C
Pop-Location
```

## GitHub Actions

Cada push a `main` activa el [workflow de publicación](.github/workflows/dotnet.yml). En un runner Ubuntu se restaura y compila la solución, se publica la API y se ejecutan pruebas HTTP con una base PostgreSQL 18 temporal. La validación incluye salud, OpenAPI, autorización y la integridad de dos reportes Excel. La solución carece de proyectos de pruebas unitarias. GitHub conserva el artefacto `ticketera-net8-release` durante siete días.

## Render y Docker

El [Dockerfile](Dockerfile) publica la API con el SDK 8 y ejecuta la salida con el runtime ASP.NET Core 8 y el usuario `app`. La aplicación acepta el puerto `PORT` de Render. El [Blueprint](render.yaml) describe el servicio Docker, una base privada, la clave JWT generada y las variables PG de conexión.

```powershell
docker build -t lab10-tintamiler:lab14 .
render blueprints validate ./render.yaml --output text
```

El servicio desplegado, `lab10-tintamiler-api`, toma la rama `main` y usa `/health`. El despliegue de código comprobado fue `dep-dc1rpabl550s73fc4kog`, correspondiente a `d589200`. El servicio web y la base `ticketera-lab14` usan el plan gratuito en Oregón. Render informa que la base vence el **22 de diciembre de 2026 a las 23:09, hora de Lima**.

## Resultados

La publicación Release arrancó en Windows y el runner Linux completó la compilación sin advertencias ni errores. El endpoint de salud confirmó que el proceso estaba activo. La descarga de los reportes de usuarios y tickets comprobó también la conexión con PostgreSQL y la generación de archivos XLSX en Render.
