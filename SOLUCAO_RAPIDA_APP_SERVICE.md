# Solução Rápida: Criar App Service

## Erro Atual
```
Error: Resource 'api-genfit-rm558515' doesn't exist. Resource should exist before deployment.
```

## Solução Mais Rápida: Azure CLI

Execute este comando no **Azure Cloud Shell** ou localmente (se tiver Azure CLI instalado):

```bash
# Login no Azure (se necessário)
az login

# Criar Resource Group
az group create --name rg-genfit-20251122 --location brazilsouth

# Criar App Service Plan
az appservice plan create \
  --name asp-genfit \
  --resource-group rg-genfit-20251122 \
  --sku B1 \
  --is-linux

# Criar App Service
az webapp create \
  --name api-genfit-rm558515 \
  --resource-group rg-genfit-20251122 \
  --plan asp-genfit \
  --runtime "DOTNETCORE|8.0"

# Configurar App Settings
az webapp config appsettings set \
  --name api-genfit-rm558515 \
  --resource-group rg-genfit-20251122 \
  --settings \
    ASPNETCORE_ENVIRONMENT="Production" \
    ConnectionStrings__OracleConnection="Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;" \
    ApiKey__HeaderName="X-API-Key" \
    ApiKey__Value="change-in-production"

# Habilitar Always On
az webapp config set \
  --name api-genfit-rm558515 \
  --resource-group rg-genfit-20251122 \
  --always-on true
```

## Ou Use o Script infra-app.sh

Se preferir usar o script que já está no projeto:

```bash
chmod +x infra-app.sh
./infra-app.sh -ORACLE_HOST oracle.fiap.com.br -ORACLE_PORT 1521 -ORACLE_SID ORCL -ORACLE_USER rm558515 -ORACLE_PASS "Fiap#2025" -LOCATION brazilsouth -NOME_WEBAPP api-genfit-rm558515
```

## Verificar se Foi Criado

```bash
az webapp show --name api-genfit-rm558515 --resource-group rg-genfit-20251122 --query "{Name:name, State:state, DefaultHostName:defaultHostName}" --output table
```

## Após Criar

Depois de criar o App Service, execute novamente o pipeline de Release. Ele deve funcionar corretamente.

