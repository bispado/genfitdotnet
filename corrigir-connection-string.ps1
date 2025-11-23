# Script para corrigir a Connection String do Oracle no Azure App Service
# Uso: .\corrigir-connection-string.ps1

$APP_SERVICE_NAME = "api-genfit-rm558515"
$RESOURCE_GROUP = "rg-genfit-20251123"  # Ajuste conforme necessário

# Connection String do Oracle
$ORACLE_CONNECTION_STRING = "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Configurando Connection String do Oracle" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "App Service: $APP_SERVICE_NAME" -ForegroundColor White
Write-Host "Resource Group: $RESOURCE_GROUP" -ForegroundColor White
Write-Host ""

# Verificar se está logado no Azure
$context = az account show 2>$null
if (-not $context) {
    Write-Host "Fazendo login no Azure..." -ForegroundColor Yellow
    az login
}

# Configurar App Settings
Write-Host "Configurando App Settings..." -ForegroundColor Yellow

az webapp config appsettings set `
    --name $APP_SERVICE_NAME `
    --resource-group $RESOURCE_GROUP `
    --settings `
        ASPNETCORE_ENVIRONMENT="Production" `
        ConnectionStrings__OracleConnection="$ORACLE_CONNECTION_STRING" `
        ApiKey__HeaderName="X-API-Key" `
        ApiKey__Value="change-in-production"

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "==========================================" -ForegroundColor Green
    Write-Host "✅ Connection String configurada com sucesso!" -ForegroundColor Green
    Write-Host "==========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Aguarde alguns segundos para o App Service reiniciar..." -ForegroundColor Yellow
    Write-Host "Teste a API em: https://$APP_SERVICE_NAME.azurewebsites.net/health" -ForegroundColor Cyan
} else {
    Write-Host ""
    Write-Host "==========================================" -ForegroundColor Red
    Write-Host "❌ Erro ao configurar Connection String" -ForegroundColor Red
    Write-Host "==========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Verifique:" -ForegroundColor Yellow
    Write-Host "1. Se o App Service existe" -ForegroundColor White
    Write-Host "2. Se o Resource Group está correto" -ForegroundColor White
    Write-Host "3. Se você tem permissões no Azure" -ForegroundColor White
}

