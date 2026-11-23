$loginResp = curl.exe -s -X POST http://localhost:5088/api/auth/login -H "Content-Type: application/json" --data-binary "@test_login.json" | ConvertFrom-Json
$token = $loginResp.token
Write-Host "Token JWT autenticado para: $($loginResp.username) ($($loginResp.roles -join ', '))"

# 1. Crear ticket con JWT
Write-Host "`n[1] Creando ticket con JWT Bearer..."
$ticketResp = curl.exe -s -X POST http://localhost:5088/api/tickets -H "Content-Type: application/json" -H "Authorization: Bearer $token" --data-binary "@test_create_ticket.json" | ConvertFrom-Json
$ticketId = $ticketResp.ticketId
Write-Host "-> Ticket Creado ID: $ticketId"
Write-Host "-> Titulo: $($ticketResp.title)"
Write-Host "-> Estado Inicial: $($ticketResp.status)"

# 2. Agregar respuesta al ticket
Write-Host "`n[2] Agregando respuesta de soporte al ticket..."
'{ "message": "Hemos revisado los logs del firewall y se procedió a habilitar el puerto 5432. Por favor validar conexión." }' | Set-Content -Path test_response.json
$respJson = curl.exe -s -X POST "http://localhost:5088/api/tickets/$ticketId/responses" -H "Content-Type: application/json" -H "Authorization: Bearer $token" --data-binary "@test_response.json" | ConvertFrom-Json
Write-Host "-> Respuesta ID: $($respJson.responseId)"
Write-Host "-> Mensaje: $($respJson.message)"

# 3. Actualizar estado a cerrado
Write-Host "`n[3] Actualizando estado a 'cerrado'..."
'{ "status": "cerrado" }' | Set-Content -Path test_status.json
$updateStatusOutput = curl.exe -s -X PUT "http://localhost:5088/api/tickets/$ticketId/status" -H "Content-Type: application/json" -H "Authorization: Bearer $token" --data-binary "@test_status.json" -w "%{http_code}"
Write-Host "-> Codigo HTTP de actualizacion: $updateStatusOutput (204 No Content)"

# 4. Obtener ticket con detalle y respuestas
Write-Host "`n[4] Consultando detalle del ticket con respuestas y estado final..."
$fullTicket = curl.exe -s -X GET "http://localhost:5088/api/tickets/$ticketId" -H "Authorization: Bearer $token" | ConvertFrom-Json
Write-Host "-> Ticket: $($fullTicket.title)"
Write-Host "-> Estado Final: $($fullTicket.status)"
Write-Host "-> Fecha Cierre: $($fullTicket.closedAt)"
Write-Host "-> Total Respuestas: $($fullTicket.responses.Count)"

# 5. Listar todos los tickets
Write-Host "`n[5] Listando todos los tickets..."
$allTickets = curl.exe -s -X GET "http://localhost:5088/api/tickets" -H "Authorization: Bearer $token" | ConvertFrom-Json
Write-Host "-> Total tickets registrados en BD: $($allTickets.Count)"
