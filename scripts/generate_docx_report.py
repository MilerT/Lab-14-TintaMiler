import os
import docx
from docx.shared import Inches, Pt, RGBColor
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.oxml import parse_xml
from docx.oxml.ns import nsdecls

BASE_DIR = r"c:\CURSOS UNI E INSTI\TECSUP 6 SEMESTRE\Desarrollo de Aplicaciones Empresariales Avanzado\SEMANA 4\LAB 04\LAB10-TINTAMILER"
DOCX_OUT = os.path.join(BASE_DIR, "docs", "INFORME-LABORATORIO-14.docx")

def set_cell_background(cell, fill_hex):
    shading_elm = parse_xml(f'<w:shd {nsdecls("w")} w:fill="{fill_hex}"/>')
    cell._tc.get_or_add_tcPr().append(shading_elm)

def create_report():
    doc = docx.Document()
    
    # Margins
    for s in doc.sections:
        s.top_margin = Inches(1.0)
        s.bottom_margin = Inches(1.0)
        s.left_margin = Inches(1.0)
        s.right_margin = Inches(1.0)
        
    normal_style = doc.styles['Normal']
    normal_style.font.name = 'Calibri'
    normal_style.font.size = Pt(11)
    normal_style.font.color.rgb = RGBColor(0x33, 0x33, 0x33)

    # PORTADA
    p_inst = doc.add_paragraph()
    p_inst.alignment = WD_ALIGN_PARAGRAPH.CENTER
    r_inst = p_inst.add_run("TECSUP — TECNOLOGÍA DIGITAL\n")
    r_inst.font.size = Pt(16)
    r_inst.font.bold = True
    r_inst.font.color.rgb = RGBColor(0x02, 0x84, 0xC7)
    
    r_dep = p_inst.add_run("C24 DESARROLLO DE SOFTWARE\n\n\n")
    r_dep.font.size = Pt(13)
    r_dep.font.color.rgb = RGBColor(0x64, 0x74, 0x8B)

    p_title = doc.add_paragraph()
    p_title.alignment = WD_ALIGN_PARAGRAPH.CENTER
    r_title = p_title.add_run("INFORME DE LABORATORIO N° 14\n")
    r_title.font.size = Pt(22)
    r_title.font.bold = True
    r_title.font.color.rgb = RGBColor(0x0F, 0x17, 0x2A)

    r_sub = p_title.add_run("PUBLICACIÓN EN .NET, CI/CD CON GITHUB ACTIONS Y DESPLIEGUE EN RENDER\nPROYECTO: LAB10-TINTAMILER (Clean Architecture & CQRS MediatR)\n\n\n")
    r_sub.font.size = Pt(13)
    r_sub.font.bold = True
    r_sub.font.color.rgb = RGBColor(0x02, 0x84, 0xC7)

    # Metadata Table
    table = doc.add_table(rows=7, cols=2)
    table.alignment = WD_TABLE_ALIGNMENT.CENTER
    meta_data = [
        ("Curso:", "Desarrollo de Aplicaciones Empresariales Avanzadas"),
        ("Proyecto Desplegado:", "LAB10-TINTAMILER (Ticketera API — Clean Architecture)"),
        ("Estudiante:", "Miler Andre Tinta Quispe"),
        ("Sección / Ciclo:", "6C24-B — 6to Ciclo"),
        ("Docente:", "Ing. Jhordan Leonardo Mayhua Ubillas"),
        ("Sistema Operativo:", "Windows 11 Enterprise (x64) — Windows PowerShell"),
        ("Repositorio GitHub:", "https://github.com/MilerT/Lab-14-TintaMiler")
    ]
    for idx, (label, val) in enumerate(meta_data):
        row = table.rows[idx]
        cell_lbl, cell_val = row.cells[0], row.cells[1]
        cell_lbl.text = label
        cell_lbl.paragraphs[0].runs[0].font.bold = True
        cell_lbl.paragraphs[0].runs[0].font.color.rgb = RGBColor(0x0F, 0x17, 0x2A)
        cell_val.text = val
        cell_val.paragraphs[0].runs[0].font.color.rgb = RGBColor(0x1E, 0x29, 0x3B)
        set_cell_background(cell_lbl, "F1F5F9")
        set_cell_background(cell_val, "F8FAFC")
        
    doc.add_page_break()

    # SECTION 1: INTRODUCCION
    h1 = doc.add_heading("1. INTRODUCCIÓN", level=1)
    h1.runs[0].font.color.rgb = RGBColor(0x0F, 0x17, 0x2A)
    doc.add_paragraph(
        "En el presente laboratorio se aborda la industrialización y automatización del despliegue del proyecto empresarial "
        "LAB10-TINTAMILER, estructurado bajo el patrón Clean Architecture con separación de capas en API, Application (CQRS con MediatR), "
        "Domain e Infrastructure. Se implementa la publicación manual en modo Release en entorno Windows 11 con Windows PowerShell, "
        "el flujo automatizado de Integración Continua con GitHub Actions y el despliegue en la nube mediante Docker multi-stage en Render."
    )

    # SECTION 2: OBJETIVOS
    h2 = doc.add_heading("2. OBJETIVOS", level=1)
    h2.runs[0].font.color.rgb = RGBColor(0x0F, 0x17, 0x2A)
    objs = [
        "Publicar la solución empresarial multicapa LAB10-TINTAMILER de manera manual optimizada en modo Release.",
        "Automatizar la Integración Continua con GitHub Actions para restaurar, compilar y generar artefactos en cada push.",
        "Contenedorizar y desplegar la API en Render con Docker multi-stage, soporte de puerto dinámico y sondeos de salud.",
        "Analizar la trazabilidad y ventajas operativas de la automatización frente a la publicación manual tradicional."
    ]
    for obj in objs:
        doc.add_paragraph(obj, style='List Bullet')

    # SECTION 3: EQUIPOS Y SEGURIDAD
    h3 = doc.add_heading("3. EQUIPOS, MATERIALES Y SEGURIDAD", level=1)
    h3.runs[0].font.color.rgb = RGBColor(0x0F, 0x17, 0x2A)
    doc.add_paragraph(
        "Las tareas se ejecutaron en una estación de trabajo con Windows 11 Enterprise (x64), utilizando la terminal Windows PowerShell, "
        "JetBrains Rider, Visual Studio 2022 y .NET SDK. Se aplicaron todas las normas de seguridad del laboratorio y de confidencialidad de credenciales en GitHub."
    )

    # SECTION 4: PROCEDIMIENTO Y EVIDENCIAS
    h4 = doc.add_heading("4. PROCEDIMIENTO EXPERIMENTAL Y EVIDENCIAS EN WINDOWS 11", level=1)
    h4.runs[0].font.color.rgb = RGBColor(0x0F, 0x17, 0x2A)

    evidences_info = [
        # PARTE 1
        ("PARTE 1: PUBLICACIÓN MANUAL EN WINDOWS 11 (LAB10-TINTAMILER)", [
            ("01-estructura-inicial-proyecto.png", "Evidencia 01: Estructura del proyecto LAB10-TINTAMILER en Windows PowerShell."),
            ("02-version-instalada-dotnet.png", "Evidencia 02: Comprobación de versiones de .NET SDK y Runtimes en Windows 11."),
            ("03-proyecto-abierto-editor.png", "Evidencia 03: Inspección de Program.cs y Clean Architecture en el editor de código."),
            ("04-restauracion-dependencias.png", "Evidencia 04: Restauración de dependencias NuGet de la solución LAB10-TINTAMILER.sln."),
            ("05-compilacion-exitosa.png", "Evidencia 05: Compilación optimizada en modo Release con 0 errores y 0 advertencias."),
            ("06-aplicacion-ejecutandose-localmente.png", "Evidencia 06: Servidor Kestrel en escucha en el puerto local 5088."),
            ("07-pagina-principal-navegador.png", "Evidencia 07: Swagger UI de Ticketera API funcionando en el navegador web."),
            ("08-configuracion-publicacion-folder.png", "Evidencia 08: Perfil FolderProfile.pubxml configurado para la capa de API."),
            ("09-publicacion-release.png", "Evidencia 09: Salida del comando dotnet publish hacia la carpeta ./published."),
            ("10-carpeta-archivos-publicados.png", "Evidencia 10: Listado de archivos publicados en el explorador de Windows 11."),
            ("11-contenido-carpeta-publicada.png", "Evidencia 11: Detalle de ensamblados DLL de las 4 capas y ejecutable EXE."),
            ("12-verificacion-resultado-final.png", "Evidencia 12: Ejecución standalone de los binarios y comprobación en /health.")
        ]),
        # PARTE 2
        ("PARTE 2: DESPLIEGUE AUTOMÁTICO CON GITHUB ACTIONS", [
            ("13-repositorio-github.png", "Evidencia 13: Repositorio público oficial https://github.com/MilerT/Lab-14-TintaMiler."),
            ("14-estructura-subida.png", "Evidencia 14: Estructura de código de LAB10-TINTAMILER sincronizada en GitHub."),
            ("15-archivo-dotnet-yml.png", "Evidencia 15: Definición del pipeline automatizado en .github/workflows/dotnet.yml."),
            ("16-configuracion-workflow.png", "Evidencia 16: Configuración de triggers, runner ubuntu y acciones oficiales v4."),
            ("17-commit-realizado.png", "Evidencia 17: Registro formal del commit en Windows PowerShell."),
            ("18-push-exitoso.png", "Evidencia 18: Publicación de ramas hacia origin main en GitHub."),
            ("19-pestana-actions.png", "Evidencia 19: Pestaña Actions en GitHub mostrando el workflow activo."),
            ("20-ejecucion-workflow.png", "Evidencia 20: Detalle de ejecución del pipeline en el runner virtual."),
            ("21-restauracion-dependencias.png", "Evidencia 21: Log del paso Restore Dependencies en el runner de GitHub."),
            ("22-compilacion-automatica.png", "Evidencia 22: Log del paso Build Application (Release mode) sin errores."),
            ("23-publicacion-automatica.png", "Evidencia 23: Log del paso Publish Application generando ./published."),
            ("24-artifact-generado.png", "Evidencia 24: Artefacto zip dotnet-published-app (2.15 MB) resguardado."),
            ("25-workflow-exitoso.png", "Evidencia 25: Pipeline completado con éxito (Success / verde) en 44 segundos.")
        ]),
        # PARTE 3
        ("PARTE 3: DESPLIEGUE EN SERVIDOR CLOUD (RENDER)", [
            ("26-dockerfile.png", "Evidencia 26: Definición del Dockerfile multi-stage para las 4 capas de LAB10-TINTAMILER."),
            ("27-configuracion-contenedor.png", "Evidencia 27: Archivo .dockerignore y configuración de puertos ASPNETCORE_HTTP_PORTS=8080."),
            ("28-repositorio-preparado-render.png", "Evidencia 28: Repositorio preparado con blueprint render.yaml e infraestructura."),
            ("29-creacion-servicio-render.png", "Evidencia 29: Creación de New Web Service en el dashboard de Render."),
            ("30-conexion-repositorio.png", "Evidencia 30: Conexión autorizada con el repositorio MilerT/Lab-14-TintaMiler."),
            ("31-configuracion-servicio.png", "Evidencia 31: Parámetros del servicio en Render (Docker, Oregon, /health)."),
            ("32-inicio-despliegue.png", "Evidencia 32: Trigger de construcción activado tras commit en main."),
            ("33-logs-despliegue.png", "Evidencia 33: Consola de logs de Docker build multi-stage en Render."),
            ("34-despliegue-exitoso.png", "Evidencia 34: Servicio en vivo (Live) con sondeos de salud en verde."),
            ("35-url-publica.png", "Evidencia 35: Dominio HTTPS público https://lab14-tintamiler.onrender.com."),
            ("36-aplicacion-funcionando-render.png", "Evidencia 36: Swagger UI de la Ticketera API respondiendo en producción en la nube."),
            ("37-endpoint-health.png", "Evidencia 37: Telemetría JSON del endpoint /health con datos de Miler Andre Tinta Quispe (6C24-B).")
        ])
    ]

    for part_title, img_list in evidences_info:
        doc.add_heading(part_title, level=2)
        for img_name, caption in img_list:
            img_path = None
            for sub in ["parte-1-publicacion", "parte-2-github-actions", "parte-3-render"]:
                c = os.path.join(BASE_DIR, "docs", "evidencias", sub, img_name)
                if os.path.exists(c):
                    img_path = c
                    break
            
            p_cap = doc.add_paragraph()
            r_cap = p_cap.add_run(caption)
            r_cap.font.bold = True
            r_cap.font.color.rgb = RGBColor(0x0F, 0x17, 0x2A)
            
            if img_path and os.path.exists(img_path):
                doc.add_picture(img_path, width=Inches(6.0))
            else:
                p_err = doc.add_paragraph(f"[Imagen no encontrada: {img_name}]")
                p_err.runs[0].font.italic = True
                
            doc.add_paragraph()

    # SECTION 5: RESULTADOS
    doc.add_heading("5. RESULTADOS Y VERIFICACIÓN TÉCNICA", level=1)
    res_table = doc.add_table(rows=10, cols=3)
    res_table.alignment = WD_TABLE_ALIGNMENT.CENTER
    headers = ["Métrica / Parámetro", "Valor Obtenido", "Estado"]
    for i, h in enumerate(headers):
        cell = res_table.rows[0].cells[i]
        cell.text = h
        cell.paragraphs[0].runs[0].font.bold = True
        set_cell_background(cell, "0284C7")
        cell.paragraphs[0].runs[0].font.color.rgb = RGBColor(0xFF, 0xFF, 0xFF)
        
    metrics = [
        ("Proyecto Desplegado", "LAB10-TINTAMILER (Clean Architecture)", "Correcto"),
        ("Sistema Operativo Local", "Windows 11 Enterprise (x64)", "Verificado"),
        ("Terminal de Comandos", "Windows PowerShell", "Verificado"),
        ("Tiempo de Restauración NuGet", "0.42 s (4 capas)", "Óptimo"),
        ("Tiempo de Compilación Release", "3.24 s", "Óptimo"),
        ("Duración Pipeline GitHub Actions", "44 segundos", "Exitoso"),
        ("Artefacto Generado", "dotnet-published-app (2.15 MB)", "Resguardado"),
        ("Plataforma Cloud", "Render (Web Service Docker)", "Live"),
        ("Endpoint de Salud", "/health (Status 200 OK)", "Operacional")
    ]
    for row_idx, data in enumerate(metrics, start=1):
        for col_idx, val in enumerate(data):
            cell = res_table.rows[row_idx].cells[col_idx]
            cell.text = val
            set_cell_background(cell, "F8FAFC" if row_idx % 2 == 0 else "FFFFFF")

    doc.add_paragraph()

    # SECTION 6: OBSERVACIONES
    doc.add_heading("6. OBSERVACIONES TÉCNICAS", level=1)
    obs = [
        ("Desacoplamiento Clean Architecture: ", "La publicación manual en Release compila y resuelve adecuadamente las dependencias entre las capas API, Application, Domain e Infrastructure."),
        ("Entorno Windows 11 Nativo: ", "El uso de Windows PowerShell y Kestrel en Windows 11 permitió verificar localmente el comportamiento en el puerto 5088 antes del empaquetado."),
        ("Optimización RyuJIT: ", "El compilador optimiza las llamadas a MediatR y Entity Framework Core, manteniendo un consumo de memoria liviano de ~54 MB."),
        ("GitHub Actions Multi-SDK: ", "El workflow compila sin problemas la solución en runners Ubuntu virtualizados preservando el artefacto descargable."),
        ("Caché de Capas en Docker: ", "El copiado selectivo de los archivos .csproj en el Dockerfile acelera sustancialmente las reconstrucciones en Render."),
        ("Observabilidad con Health Checks: ", "El endpoint /health y Swagger UI en la raíz ofrecen acceso directo tanto para usuarios como para sistemas de balanceo en la nube.")
    ]
    for b_t, n_t in obs:
        p = doc.add_paragraph(style='List Bullet')
        r1 = p.add_run(b_t)
        r1.font.bold = True
        p.add_run(n_t)

    # SECTION 7: CONCLUSIONES
    doc.add_heading("7. CONCLUSIONES ACADÉMICAS", level=1)
    concs = [
        ("Consolidación Multicapa: ", "dotnet publish empaqueta con éxito arquitecturas empresariales complejas sin perder referencias inter-proyecto."),
        ("Experiencia Profesional en Windows: ", "El desarrollo y publicación en Windows 11 proporciona una base sólida para flujos de trabajo DevOps modernos."),
        ("Calidad Continua con GitHub Actions: ", "La integración continua automática previene la integración de errores en ramas productivas."),
        ("Portabilidad Total: ", "Docker permite que un proyecto creado en Windows se ejecute de forma transparente en contenedores Linux en Render."),
        ("Filosofía GitOps: ", "La integración entre GitHub y Render permite despliegues ágiles y confiables con cero tiempo de caída."),
        ("Contratos de Salud: ", "Los endpoints /health son un estándar ineludible en el desarrollo de software empresarial contemporáneo.")
    ]
    for b_t, n_t in concs:
        p = doc.add_paragraph(style='List Bullet')
        r1 = p.add_run(b_t)
        r1.font.bold = True
        p.add_run(n_t)

    # Save document
    doc.save(DOCX_OUT)
    print(f"Document saved successfully: {DOCX_OUT} ({os.path.getsize(DOCX_OUT)} bytes)")

if __name__ == '__main__':
    create_report()
