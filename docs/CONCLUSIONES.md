# Conclusiones Académicas — Laboratorio 14
## Proyecto: LAB10-TINTAMILER (Clean Architecture & CQRS MediatR)

**Curso:** Desarrollo de Aplicaciones Empresariales Avanzadas  
**Estudiante:** Miler Andre Tinta Quispe  
**Sección:** 6C24-B  
**Docente:** Ing. Jhordan Leonardo Mayhua Ubillas  
**Institución:** TECSUP — 2026  
**Sistema Operativo del Estudiante:** Windows 11 Enterprise (x64)  
**Repositorio Oficial:** [https://github.com/MilerT/Lab-14-TintaMiler](https://github.com/MilerT/Lab-14-TintaMiler)

---

### 1. Robustez de la Publicación en Arquitecturas Empresariales Multicapa
El proceso de publicación manual con `dotnet publish` sobre la solución `LAB10-TINTAMILER.sln` ratifica la eficacia del empaquetado de .NET. Todas las dependencias inter-proyecto entre `API`, `APPLICATION`, `DOMAIN` e `INFRASTRUCTURE` se consolidan de manera coherente en `./published`, garantizando que la solución completa de tickets pueda operar de forma aislada sin requerir el código fuente original.

### 2. Validez de la Experiencia Nativa en Windows 11 para Proyectos Empresariales
El desarrollo en Windows 11 haciendo uso de la terminal de Windows PowerShell y herramientas como JetBrains Rider y Visual Studio ofrece una experiencia de ingeniería de primer nivel. El uso de comandos nativos de .NET CLI permite simular fielmente el comportamiento de servidores antes de subir los cambios a repositorios remotos.

### 3. Automatización de Integración Continua sin Fricciones con GitHub Actions
La implementación del workflow de CI en GitHub Actions demostró que una solución multicapa compleja con MediatR y Entity Framework Core se puede restaurar, compilar y verificar automáticamente en menos de un minuto. La generación de artefactos versionados (`dotnet-published-app`) establece un mecanismo confiable de auditoría y respaldo.

### 4. Portabilidad Absoluta Mediante Contenedores Docker
La definición del Dockerfile multi-stage adaptado para la estructura Clean Architecture de `LAB10-TINTAMILER` garantiza que el proyecto sea completamente agnóstico al sistema operativo. Pese a haber sido desarrollado en Windows 11, se ejecuta de manera idéntica en contenedores Linux gestionados en la nube.

### 5. Eficiencia de Despliegue en la Nube con Render (GitOps)
La integración directa de Render con la rama `main` del repositorio `https://github.com/MilerT/Lab-14-TintaMiler` proporciona un flujo continuo de entregas. Cada commit validado genera una reconstrucción de la imagen en Render y su despliegue con cero tiempo de caída (Zero Downtime).

### 6. Observabilidad Empresarial y Contratos de Salud
La implementación del endpoint `/health` y Swagger UI en producción confirma la necesidad de dotar a las APIs empresariales de interfaces de diagnóstico. Permite que tanto los desarrolladores como los balanceadores de carga monitoreen en tiempo real la salud de la aplicación, el framework y los metadatos institucionales.
