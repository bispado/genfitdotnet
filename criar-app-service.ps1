# Script PowerShell para criar o App Service no Azure
# Execute: .\criar-app-service.ps1

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Criando App Service no Azure" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Cyan

# Variáveis
$RESOURCE_GROUP = "rg-genfit-20251122"
$APP_SERVICE_PLAN = "asp-genfit"
$APP_SERVICE_NAME = "api-genfit-rm558515"
$LOCATION = "brazilsouth"

Write-Host "`nConfigurações:" -ForegroundColor Yellow
Write-Host "Resource Group: $RESOURCE_GROUP"
Write-Host "App Service Plan: $APP_SERVICE_PLAN"
Write-Host "App Service Name: $APP_SERVICE_NAME"
Write-Host "Location: $LOCATION"
Write-Host "`n==========================================" -ForegroundColor Cyan

# Verificar se está logado no Azure
Write-Host "`nVerificando login no Azure..." -ForegroundColor Yellow
$account = az account show 2>$null
if (-not $account) {
    Write-Host "Não está logado. Fazendo login..." -ForegroundColor Yellow
    az login
}

# Criar Resource Group
Write-Host "`nCriando Resource Group: $RESOURCE_GROUP..." -ForegroundColor Yellow
az group create --name $RESOURCE_GROUP --location $LOCATION

# Criar App Service Plan
Write-Host "`nCriando App Service Plan: $APP_SERVICE_PLAN..." -ForegroundColor Yellow
az appservice plan create `
  --name $APP_SERVICE_PLAN `
  --resource-group $RESOURCE_GROUP `
  --sku B1 `
  --is-linux

# Criar App Service
Write-Host "`nCriando App Service: $APP_SERVICE_NAME..." -ForegroundColor Yellow
az webapp create `
  --name $APP_SERVICE_NAME `
  --resource-group $RESOURCE_GROUP `
  --plan $APP_SERVICE_PLAN `
  --runtime 'DOTNETCORE|8.0'

# Configurar App Settings
Write-Host "`nConfigurando App Settings..." -ForegroundColor Yellow
az webapp config appsettings set `
  --name $APP_SERVICE_NAME `
  --resource-group $RESOURCE_GROUP `
  --settings `
    ASPNETCORE_ENVIRONMENT="Production" `
    ConnectionStrings__OracleConnection="Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;" `
    ApiKey__HeaderName="X-API-Key" `
    ApiKey__Value="change-in-production"

# Habilitar Always On
Write-Host "`nHabilitando Always On..." -ForegroundColor Yellow
az webapp config set `
  --name $APP_SERVICE_NAME `
  --resource-group $RESOURCE_GROUP `
  --always-on true

# Verificar se foi criado
Write-Host "`nVerificando App Service criado..." -ForegroundColor Yellow
az webapp show --name $APP_SERVICE_NAME --resource-group $RESOURCE_GROUP --query "{Name:name, State:state, URL:defaultHostName}" --output table

Write-Host "`n==========================================" -ForegroundColor Cyan
Write-Host "App Service criado com sucesso!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "`nURL do App Service:" -ForegroundColor Yellow
Write-Host "https://$APP_SERVICE_NAME.azurewebsites.net" -ForegroundColor White
Write-Host "`nAgora você pode executar o pipeline de Release novamente!" -ForegroundColor Green

