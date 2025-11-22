# Comando rápido para criar apenas o App Service (já que o Resource Group e Plan já existem)
# Execute: .\comando-rapido-criar-app-service.ps1

$RESOURCE_GROUP = "rg-genfit-20251122"
$APP_SERVICE_PLAN = "asp-genfit"
$APP_SERVICE_NAME = "api-genfit-rm558515"

Write-Host "Criando App Service: $APP_SERVICE_NAME..." -ForegroundColor Yellow

$RUNTIME = "DOTNETCORE|8.0"
az webapp create --name $APP_SERVICE_NAME --resource-group $RESOURCE_GROUP --plan $APP_SERVICE_PLAN --runtime $RUNTIME

Write-Host "`nConfigurando App Settings..." -ForegroundColor Yellow
az webapp config appsettings set `
  --name $APP_SERVICE_NAME `
  --resource-group $RESOURCE_GROUP `
  --settings `
    ASPNETCORE_ENVIRONMENT="Production" `
    ConnectionStrings__OracleConnection="Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;" `
    ApiKey__HeaderName="X-API-Key" `
    ApiKey__Value="change-in-production"

Write-Host "`nHabilitando Always On..." -ForegroundColor Yellow
az webapp config set `
  --name $APP_SERVICE_NAME `
  --resource-group $RESOURCE_GROUP `
  --always-on true

Write-Host "`n✅ App Service criado com sucesso!" -ForegroundColor Green
Write-Host "URL: https://$APP_SERVICE_NAME.azurewebsites.net" -ForegroundColor Cyan

