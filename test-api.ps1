# Script de teste da API GenFit

Write-Host "=========================================="
Write-Host "Testando API GenFit"
Write-Host "=========================================="

$baseUrl = "http://localhost:5118"
$apiKey = "test-api-key-dev"

# Teste 1: Health Check
Write-Host "`n1. Testando Health Check..." -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/health" -Method Get -ContentType "application/json"
    Write-Host "Status: $($response.status)" -ForegroundColor Green
    $response.checks | ForEach-Object {
        Write-Host "  - $($_.name): $($_.status)" -ForegroundColor $(if ($_.status -eq "Healthy") { "Green" } else { "Red" })
    }
} catch {
    Write-Host "Erro ao testar Health Check: $_" -ForegroundColor Red
}

# Teste 2: Wellcome Endpoint
Write-Host "`n2. Testando Wellcome Endpoint..." -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/v1/wellcome" -Method Get
    Write-Host "Resposta: $response" -ForegroundColor Green
} catch {
    Write-Host "Erro ao testar Wellcome: $_" -ForegroundColor Red
}

# Teste 3: Listar Usuários (com API Key)
Write-Host "`n3. Testando GET /api/v1/users..." -ForegroundColor Cyan
try {
    $headers = @{
        "X-API-Key" = $apiKey
    }
    $response = Invoke-RestMethod -Uri "$baseUrl/api/v1/users?pageNumber=1&pageSize=10" -Method Get -Headers $headers -ContentType "application/json"
    Write-Host "Total de usuários: $($response.totalCount)" -ForegroundColor Green
    Write-Host "Página: $($response.pageNumber)/$($response.totalPages)" -ForegroundColor Green
    Write-Host "Links HATEOAS:" -ForegroundColor Yellow
    $response.links.PSObject.Properties | ForEach-Object {
        Write-Host "  - $($_.Name): $($_.Value)" -ForegroundColor Gray
    }
} catch {
    Write-Host "Erro ao listar usuários: $_" -ForegroundColor Red
}

# Teste 4: Criar Usuário (com API Key)
Write-Host "`n4. Testando POST /api/v1/users..." -ForegroundColor Cyan
try {
    $headers = @{
        "X-API-Key" = $apiKey
        "Content-Type" = "application/json"
    }
    $body = @{
        role = "candidate"
        nome = "Teste Usuario API"
        email = "teste.api@example.com"
        senhaHash = "hash123"
        cpf = "123.456.789-00"
        telefone = "(11) 99999-9999"
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$baseUrl/api/v1/users" -Method Post -Headers $headers -Body $body
    Write-Host "Usuário criado com sucesso!" -ForegroundColor Green
    Write-Host "ID: $($response.id)" -ForegroundColor Green
    Write-Host "Nome: $($response.nome)" -ForegroundColor Green
    Write-Host "Email: $($response.email)" -ForegroundColor Green
} catch {
    Write-Host "Erro ao criar usuário: $_" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Detalhes: $responseBody" -ForegroundColor Yellow
    }
}

Write-Host "`n=========================================="
Write-Host "Testes concluídos!"
Write-Host "=========================================="
