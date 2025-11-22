# ⚡ Instruções Rápidas: Criar App Service

## ❌ Erro Atual
```
Error: Resource 'api-genfit-rm558515' doesn't exist.
```

## ✅ Solução em 3 Passos

### Passo 1: Abrir PowerShell ou Terminal

No Windows:
- Pressione `Win + X` e escolha "Windows PowerShell" ou "Terminal"
- Ou abra o Azure Cloud Shell: https://shell.azure.com/

### Passo 2: Executar o Script

**Opção A - PowerShell (Windows):**
```powershell
# Navegue até a pasta do projeto
cd "C:\Users\gusta\OneDrive\Documentos\fiap\gs\dotnet"

# Execute o script
.\criar-app-service.ps1
```

**Opção B - Azure Cloud Shell (Qualquer SO):**
```bash
# Copie e cole este comando completo:
RESOURCE_GROUP="rg-genfit-20251122" && APP_SERVICE_PLAN="asp-genfit" && APP_SERVICE_NAME="api-genfit-rm558515" && LOCATION="brazilsouth" && az group create --name $RESOURCE_GROUP --location $LOCATION && az appservice plan create --name $APP_SERVICE_PLAN --resource-group $RESOURCE_GROUP --sku B1 --is-linux && az webapp create --name $APP_SERVICE_NAME --resource-group $RESOURCE_GROUP --plan $APP_SERVICE_PLAN --runtime "DOTNETCORE|8.0" && az webapp config appsettings set --name $APP_SERVICE_NAME --resource-group $RESOURCE_GROUP --settings ASPNETCORE_ENVIRONMENT="Production" ConnectionStrings__OracleConnection="Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;" ApiKey__HeaderName="X-API-Key" ApiKey__Value="change-in-production" && az webapp config set --name $APP_SERVICE_NAME --resource-group $RESOURCE_GROUP --always-on true && echo "✅ App Service criado com sucesso!"
```

**Opção C - Comandos Separados (Mais Fácil de Debugar):**
```bash
# 1. Criar Resource Group
az group create --name rg-genfit-20251122 --location brazilsouth

# 2. Criar App Service Plan
az appservice plan create --name asp-genfit --resource-group rg-genfit-20251122 --sku B1 --is-linux

# 3. Criar App Service
az webapp create --name api-genfit-rm558515 --resource-group rg-genfit-20251122 --plan asp-genfit --runtime "DOTNETCORE|8.0"

# 4. Configurar App Settings
az webapp config appsettings set --name api-genfit-rm558515 --resource-group rg-genfit-20251122 --settings ASPNETCORE_ENVIRONMENT="Production" ConnectionStrings__OracleConnection="Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;" ApiKey__HeaderName="X-API-Key" ApiKey__Value="change-in-production"

# 5. Habilitar Always On
az webapp config set --name api-genfit-rm558515 --resource-group rg-genfit-20251122 --always-on true
```

### Passo 3: Verificar se Foi Criado

```bash
az webapp show --name api-genfit-rm558515 --resource-group rg-genfit-20251122 --query "{Name:name, State:state, URL:defaultHostName}" --output table
```

Se aparecer o App Service na lista, está criado! ✅

### Passo 4: Executar Pipeline Novamente

Agora você pode executar o pipeline de Release novamente. O erro deve desaparecer!

## 🔍 Verificar Login no Azure

Se der erro de autenticação, execute:
```bash
az login
```

## 📝 Notas Importantes

- ⚠️ O nome do App Service **DEVE** ser exatamente: `api-genfit-rm558515`
- ⚠️ O Resource Group será: `rg-genfit-20251122`
- ⚠️ A região será: `brazilsouth`
- ⏱️ A criação leva cerca de 1-2 minutos

## 🆘 Ainda com Problemas?

1. Verifique se você tem permissões na assinatura do Azure
2. Verifique se o Azure CLI está instalado: `az --version`
3. Verifique se está logado: `az account show`

