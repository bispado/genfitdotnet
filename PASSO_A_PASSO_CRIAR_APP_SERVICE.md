# Passo a Passo: Criar App Service no Azure

## ⚠️ Erro Atual
```
Error: Resource 'api-genfit-rm558515' doesn't exist. Resource should exist before deployment.
```

## ✅ Solução: Criar o App Service ANTES do Deploy

### Opção 1: Criar Manualmente via Portal Azure (Mais Simples)

1. **Acesse o Portal do Azure:**
   - Vá para: https://portal.azure.com
   - Faça login com sua conta

2. **Criar Resource Group:**
   - Clique em "Create a resource"
   - Procure por "Resource group"
   - Clique em "Create"
   - **Name:** `rg-genfit-20251122`
   - **Region:** `Brazil South`
   - Clique em "Review + create" e depois "Create"

3. **Criar App Service Plan:**
   - Clique em "Create a resource"
   - Procure por "App Service Plan"
   - Clique em "Create"
   - **Subscription:** Sua assinatura
   - **Resource Group:** `rg-genfit-20251122` (criado acima)
   - **Name:** `asp-genfit`
   - **Operating System:** Linux
   - **Region:** Brazil South
   - **Pricing tier:** Basic B1
   - Clique em "Review + create" e depois "Create"

4. **Criar App Service (Web App):**
   - Clique em "Create a resource"
   - Procure por "Web App"
   - Clique em "Create"
   - **Subscription:** Sua assinatura
   - **Resource Group:** `rg-genfit-20251122`
   - **Name:** `api-genfit-rm558515` ⚠️ **IMPORTANTE: Este é o nome que o pipeline espera**
   - **Publish:** Code
   - **Runtime stack:** .NET 8
   - **Operating System:** Linux
   - **Region:** Brazil South
   - **App Service Plan:** `asp-genfit` (criado acima)
   - Clique em "Review + create" e depois "Create"

5. **Configurar App Settings:**
   - Após criar, vá até o App Service `api-genfit-rm558515`
   - No menu lateral, clique em "Configuration"
   - Clique em "Application settings"
   - Clique em "+ New application setting" e adicione:

   | Name | Value |
   |------|-------|
   | `ASPNETCORE_ENVIRONMENT` | `Production` |
   | `ConnectionStrings__OracleConnection` | `Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;` |
   | `ApiKey__HeaderName` | `X-API-Key` |
   | `ApiKey__Value` | `change-in-production` |

   - Clique em "Save"

6. **Habilitar Always On:**
   - Ainda na página "Configuration"
   - Clique em "General settings"
   - Ative "Always On"
   - Clique em "Save"

### Opção 2: Criar via Azure Cloud Shell (Mais Rápido)

1. **Acesse o Azure Cloud Shell:**
   - Vá para: https://shell.azure.com/
   - Ou clique no ícone `>_` no canto superior direito do Portal Azure

2. **Execute os comandos abaixo:**

```bash
# Definir variáveis
RESOURCE_GROUP="rg-genfit-20251122"
APP_SERVICE_PLAN="asp-genfit"
APP_SERVICE_NAME="api-genfit-rm558515"
LOCATION="brazilsouth"

# Criar Resource Group
az group create --name $RESOURCE_GROUP --location $LOCATION

# Criar App Service Plan
az appservice plan create \
  --name $APP_SERVICE_PLAN \
  --resource-group $RESOURCE_GROUP \
  --sku B1 \
  --is-linux

# Criar App Service
az webapp create \
  --name $APP_SERVICE_NAME \
  --resource-group $RESOURCE_GROUP \
  --plan $APP_SERVICE_PLAN \
  --runtime "DOTNETCORE|8.0"

# Configurar App Settings
az webapp config appsettings set \
  --name $APP_SERVICE_NAME \
  --resource-group $RESOURCE_GROUP \
  --settings \
    ASPNETCORE_ENVIRONMENT="Production" \
    ConnectionStrings__OracleConnection="Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;" \
    ApiKey__HeaderName="X-API-Key" \
    ApiKey__Value="change-in-production"

# Habilitar Always On
az webapp config set \
  --name $APP_SERVICE_NAME \
  --resource-group $RESOURCE_GROUP \
  --always-on true

# Verificar se foi criado
az webapp show --name $APP_SERVICE_NAME --resource-group $RESOURCE_GROUP --query "{Name:name, State:state, URL:defaultHostName}" --output table
```

### Opção 3: Adicionar Etapa na Pipeline de Release (Automatizado)

Adicione uma etapa **ANTES** do "Deploy no Serviço de Aplicativo" na pipeline de Release:

1. **Na pipeline de Release, clique em "+ Add" para adicionar um novo Job**
2. **Adicione uma tarefa "Azure CLI"**
3. **Configure:**
   - **Display name:** `Criar App Service se não existir`
   - **Azure Resource Manager connection:** Sua conexão do Azure
   - **Script Type:** `Shell Script`
   - **Script Location:** `Inline Script`
   - **Inline Script:** Cole o script abaixo

```bash
#!/bin/bash
set -e

RESOURCE_GROUP="rg-genfit-20251122"
APP_SERVICE_PLAN="asp-genfit"
APP_SERVICE_NAME="api-genfit-rm558515"
LOCATION="brazilsouth"

# Verificar se o App Service já existe
if az webapp show --name $APP_SERVICE_NAME --resource-group $RESOURCE_GROUP &>/dev/null; then
    echo "App Service $APP_SERVICE_NAME já existe. Pulando criação."
else
    echo "Criando App Service $APP_SERVICE_NAME..."
    
    # Criar Resource Group se não existir
    az group create --name $RESOURCE_GROUP --location $LOCATION 2>/dev/null || echo "Resource Group já existe"
    
    # Criar App Service Plan se não existir
    az appservice plan create \
      --name $APP_SERVICE_PLAN \
      --resource-group $RESOURCE_GROUP \
      --sku B1 \
      --is-linux 2>/dev/null || echo "App Service Plan já existe"
    
    # Criar App Service
    az webapp create \
      --name $APP_SERVICE_NAME \
      --resource-group $RESOURCE_GROUP \
      --plan $APP_SERVICE_PLAN \
      --runtime "DOTNETCORE|8.0"
    
    # Configurar App Settings
    az webapp config appsettings set \
      --name $APP_SERVICE_NAME \
      --resource-group $RESOURCE_GROUP \
      --settings \
        ASPNETCORE_ENVIRONMENT="Production" \
        ConnectionStrings__OracleConnection="$(OracleConnection)" \
        ApiKey__HeaderName="X-API-Key" \
        ApiKey__Value="change-in-production"
    
    # Habilitar Always On
    az webapp config set \
      --name $APP_SERVICE_NAME \
      --resource-group $RESOURCE_GROUP \
      --always-on true
    
    echo "App Service criado com sucesso!"
fi
```

4. **Configure a dependência:** O job "Deploy no Serviço de Aplicativo" deve depender deste novo job

## ✅ Verificar se Funcionou

Após criar o App Service, execute novamente o pipeline de Release. O erro deve desaparecer.

## 🔍 Verificar se o App Service Existe

Execute no Azure Cloud Shell:
```bash
az webapp list --query "[?name=='api-genfit-rm558515'].{Name:name, ResourceGroup:resourceGroup, State:state, URL:defaultHostName}" --output table
```

Se aparecer o App Service na lista, está criado corretamente!

