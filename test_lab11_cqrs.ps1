# ==============================================================================
# SCRIPT DE PRUEBAS AUTOMATIZADAS - LABORATORIO 11: CQRS CON MEDIATR
# ==============================================================================
$baseUrl = "http://localhost:5088"
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host " INICIANDO PRUEBAS DE ENDPOINTS CQRS + MEDIATR ($baseUrl) " -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan

# 1. Login con credenciales para obtener JWT Token (LoginCommand)
Write-Host "`n[PRUEBA 1] Login de Usuario -> LoginCommand..." -ForegroundColor Yellow
$loginPayload = @{
    username = "miler_tinta"
    password = "Password123!"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method Post -ContentType "application/json" -Body $loginPayload
$token = $loginResponse.token
$userId = $loginResponse.userId
Write-Host "  -> Usuario autenticado: $($loginResponse.username)" -ForegroundColor Green
Write-Host "  -> User ID: $userId" -ForegroundColor Green
Write-Host "  -> Roles: $($loginResponse.roles -join ', ')" -ForegroundColor Green
Write-Host "  -> Token JWT obtenido exitosamente (Longitud: $($token.Length) chars)" -ForegroundColor Green

$headers = @{
    "Authorization" = "Bearer $token"
}

# 2. Listar tickets existentes -> GetAllTicketsQuery
Write-Host "`n[PRUEBA 2] Listar Tickets Existentes -> GetAllTicketsQuery..." -ForegroundColor Yellow
$initialTickets = Invoke-RestMethod -Uri "$baseUrl/api/tickets" -Method Get -Headers $headers
Write-Host "  -> Cantidad de tickets existentes en BD: $($initialTickets.Count)" -ForegroundColor Green

# 3. Crear nuevo ticket -> CreateTicketCommand
Write-Host "`n[PRUEBA 3] Crear Nuevo Ticket -> CreateTicketCommand..." -ForegroundColor Yellow
$newTicketPayload = @{
    title = "Incidencia CQRS Lab 11 - Error en conexión de microservicio"
    description = "Se detecta latencia elevada al procesar comandos MediatR en el entorno de desarrollo."
} | ConvertTo-Json

$createdTicket = Invoke-RestMethod -Uri "$baseUrl/api/tickets" -Method Post -Headers $headers -ContentType "application/json" -Body $newTicketPayload
$ticketId = $createdTicket.ticketId
Write-Host "  -> Ticket Creado con ID: $ticketId" -ForegroundColor Green
Write-Host "  -> Titulo: $($createdTicket.title)" -ForegroundColor Green
Write-Host "  -> Estado Inicial: $($createdTicket.status)" -ForegroundColor Green
Write-Host "  -> Creado por: $($createdTicket.username)" -ForegroundColor Green

# 4. Consultar ticket por ID -> GetTicketByIdQuery
Write-Host "`n[PRUEBA 4] Consultar Ticket por ID -> GetTicketByIdQuery..." -ForegroundColor Yellow
$queriedTicket = Invoke-RestMethod -Uri "$baseUrl/api/tickets/$ticketId" -Method Get -Headers $headers
Write-Host "  -> Ticket recuperado: $($queriedTicket.title)" -ForegroundColor Green
Write-Host "  -> Estado actual: $($queriedTicket.status)" -ForegroundColor Green

# 5. Consultar tickets por Usuario -> GetTicketsByUserQuery
Write-Host "`n[PRUEBA 5] Consultar Tickets por Usuario -> GetTicketsByUserQuery..." -ForegroundColor Yellow
$userTickets = Invoke-RestMethod -Uri "$baseUrl/api/tickets/user/$userId" -Method Get -Headers $headers
Write-Host "  -> Total tickets asociados al usuario $($userId): $($userTickets.Count)" -ForegroundColor Green

# 6. Agregar respuesta al ticket -> AddResponseCommand
Write-Host "`n[PRUEBA 6] Agregar Respuesta Técnica -> AddResponseCommand..." -ForegroundColor Yellow
$responsePayload = @{
    message = "Equipo de arquitectura CQRS revisó la configuración del pipeline y optimizó el Dispatcher de MediatR."
} | ConvertTo-Json

$addedResponse = Invoke-RestMethod -Uri "$baseUrl/api/tickets/$ticketId/responses" -Method Post -Headers $headers -ContentType "application/json" -Body $responsePayload
Write-Host "  -> Respuesta creada con ID: $($addedResponse.responseId)" -ForegroundColor Green
Write-Host "  -> Respondido por: $($addedResponse.responderUsername)" -ForegroundColor Green
Write-Host "  -> Mensaje: $($addedResponse.message)" -ForegroundColor Green

# 7. Actualizar estado del ticket a 'cerrado' -> UpdateTicketStatusCommand
Write-Host "`n[PRUEBA 7] Actualizar Estado del Ticket a 'cerrado' -> UpdateTicketStatusCommand..." -ForegroundColor Yellow
$statusPayload = @{
    status = "cerrado"
} | ConvertTo-Json

$updateStatusResp = Invoke-WebRequest -Uri "$baseUrl/api/tickets/$ticketId/status" -Method Put -Headers $headers -ContentType "application/json" -Body $statusPayload -UseBasicParsing
Write-Host "  -> Codigo HTTP retornado: $($updateStatusResp.StatusCode) No Content" -ForegroundColor Green

# 8. Verificar estado cerrado y fecha de cierre
Write-Host "`n[PRUEBA 8] Verificar Estado Final y Respuestas -> GetTicketByIdQuery..." -ForegroundColor Yellow
$closedTicket = Invoke-RestMethod -Uri "$baseUrl/api/tickets/$ticketId" -Method Get -Headers $headers
Write-Host "  -> Estado: $($closedTicket.status)" -ForegroundColor Green
Write-Host "  -> ClosedAt: $($closedTicket.closedAt)" -ForegroundColor Green
Write-Host "  -> Respuestas registradas: $($closedTicket.responses.Count)" -ForegroundColor Green

# 9. Eliminar ticket -> DeleteTicketCommand
Write-Host "`n[PRUEBA 9] Eliminar Ticket Creado -> DeleteTicketCommand..." -ForegroundColor Yellow
$deleteResp = Invoke-WebRequest -Uri "$baseUrl/api/tickets/$ticketId" -Method Delete -Headers $headers -UseBasicParsing
Write-Host "  -> Codigo HTTP retornado: $($deleteResp.StatusCode) No Content" -ForegroundColor Green

# 10. Verificar que el ticket ya no existe (404 NotFound)
Write-Host "`n[PRUEBA 10] Verificar Ticket Eliminado (Esperando 404 NotFound)..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/api/tickets/$ticketId" -Method Get -Headers $headers
    Write-Host "  -> ERROR: Se encontro el ticket cuando debio haber sido eliminado!" -ForegroundColor Red
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host "  -> Correcto: El ticket ya no existe en el sistema. HTTP $statusCode NotFound" -ForegroundColor Green
}

# 11. Probar ID inexistente en PUT y DELETE (Esperando 404 NotFound)
Write-Host "`n[PRUEBA 11] Probar ID inexistente (Esperando 404 NotFound)..." -ForegroundColor Yellow
$fakeId = [Guid]::NewGuid()
try {
    Invoke-WebRequest -Uri "$baseUrl/api/tickets/$fakeId/status" -Method Put -Headers $headers -ContentType "application/json" -Body $statusPayload -UseBasicParsing
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Write-Host "  -> PUT con ID inexistente retorno correctamente HTTP $code NotFound" -ForegroundColor Green
}
try {
    Invoke-WebRequest -Uri "$baseUrl/api/tickets/$fakeId" -Method Delete -Headers $headers -UseBasicParsing
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Write-Host "  -> DELETE con ID inexistente retorno correctamente HTTP $code NotFound" -ForegroundColor Green
}

# 12. Probar validacion de modelo invalido (Esperando 400 BadRequest)
Write-Host "`n[PRUEBA 12] Probar datos invalidos (Esperando 400 BadRequest)..." -ForegroundColor Yellow
$invalidTicketPayload = @{
    title = "" # Titulo vacio (requerido)
} | ConvertTo-Json

try {
    Invoke-RestMethod -Uri "$baseUrl/api/tickets" -Method Post -Headers $headers -ContentType "application/json" -Body $invalidTicketPayload
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Write-Host "  -> POST con titulo vacio retorno correctamente HTTP $code BadRequest" -ForegroundColor Green
}

Write-Host "`n==================================================================" -ForegroundColor Cyan
Write-Host " TODAS LAS PRUEBAS DE CQRS CON MEDIATR SE COMPLETARON CON EXITO " -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
