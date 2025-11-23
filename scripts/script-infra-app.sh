#!/bin/bash

# Script de infraestrutura para deploy da API GenFit no Azure
# Parâmetros esperados: ORACLE_HOST ORACLE_PORT ORACLE_SID ORACLE_USER ORACLE_PASS LOCATION
# Uso: ./scripts/script-infra-app.sh <ORACLE_HOST> <ORACLE_PORT> <ORACLE_SID> <ORACLE_USER> <ORACLE_PASS> <LOCATION>
# Ou: ./scripts/script-infra-app.sh -ORACLE_HOST host -ORACLE_PORT port -ORACLE_SID sid -ORACLE_USER user -ORACLE_PASS pass -LOCATION location

set -e

# Valores padrão
ORACLE_HOST="oracle.fiap.com.br"
ORACLE_PORT="1521"
ORACLE_SID="ORCL"
ORACLE_USER="pf0841"
ORACLE_PASS=""
LOCATION="brazilsouth"
NOME_WEBAPP=""

# Processar argumentos nomeados ou posicionais
if [[ "$1" =~ ^- ]]; then
    # Formato nomeado: -ORACLE_HOST valor -ORACLE_PORT valor...
    while [[ $# -gt 0 ]]; do
        case $1 in
            -ORACLE_HOST)
                ORACLE_HOST="$2"
                shift 2
                ;;
            -ORACLE_PORT)
                ORACLE_PORT="$2"
                shift 2
                ;;
            -ORACLE_SID)
                ORACLE_SID="$2"
                shift 2
                ;;
            -ORACLE_USER)
                ORACLE_USER="$2"
                shift 2
                ;;
            -ORACLE_PASS)
                ORACLE_PASS="$2"
                shift 2
                ;;
            -LOCATION)
                LOCATION="$2"
                shift 2
                ;;
            -NOME_WEBAPP)
                NOME_WEBAPP="$2"
                shift 2
                ;;
            *)
                echo "Argumento desconhecido: $1"
                shift
                ;;
        esac
    done
else
    # Formato posicional: valor1 valor2 valor3...
    ORACLE_HOST=${1:-"$ORACLE_HOST"}
    ORACLE_PORT=${2:-"$ORACLE_PORT"}
    ORACLE_SID=${3:-"$ORACLE_SID"}
    ORACLE_USER=${4:-"$ORACLE_USER"}
    ORACLE_PASS=${5:-"$ORACLE_PASS"}
    LOCATION=${6:-"$LOCATION"}
    NOME_WEBAPP=${7:-"$NOME_WEBAPP"}
fi

# Usar variável de ambiente NOME_WEBAPP se não foi passada como argumento
if [[ -z "$NOME_WEBAPP" && -n "$NOME_WEBAPP_ENV" ]]; then
    NOME_WEBAPP="$NOME_WEBAPP_ENV"
fi

# Configurações do Azure
RESOURCE_GROUP="rg-genfit-$(date +%Y%m%d)"
APP_SERVICE_PLAN="asp-genfit"
APP_SERVICE_NAME="${NOME_WEBAPP:-api-genfit-$(whoami | tr '[:upper:]' '[:lower:]')}"
SKU="B1"

# Configurações do Azure SQL
SQL_SERVER_NAME="sql-genfit-$(echo $APP_SERVICE_NAME | tr -d '-' | cut -c1-20)"
SQL_DB_NAME="GenFitDB"
SQL_ADMIN_USER="sqladmin"
SQL_ADMIN_PASSWORD="GenFit@2025#Secure"

echo "=========================================="
echo "Criando infraestrutura no Azure"
echo "=========================================="
echo "Oracle Host: $ORACLE_HOST"
echo "Oracle Port: $ORACLE_PORT"
echo "Oracle SID: $ORACLE_SID"
echo "Oracle User: $ORACLE_USER"
echo "Location: $LOCATION"
echo "Resource Group: $RESOURCE_GROUP"
echo "App Service: $APP_SERVICE_NAME"
echo "SQL Server: $SQL_SERVER_NAME"
echo "SQL Database: $SQL_DB_NAME"
echo "=========================================="

# Verificar se está logado no Azure
echo "Verificando login no Azure..."
az account show > /dev/null 2>&1 || {
    echo "ERRO: Não está logado no Azure. Execute 'az login' primeiro."
    exit 1
}

# Verificar se o App Service já existe e em qual Resource Group
echo "Verificando se App Service já existe..."
EXISTING_RG=$(az webapp list --query "[?name=='$APP_SERVICE_NAME'].resourceGroup" -o tsv 2>/dev/null | head -n 1)

if [ -n "$EXISTING_RG" ]; then
    echo "AVISO: App Service '$APP_SERVICE_NAME' já existe no Resource Group '$EXISTING_RG'"
    echo "Usando Resource Group existente: $EXISTING_RG"
    RESOURCE_GROUP="$EXISTING_RG"
else
    # Criar Resource Group apenas se o App Service não existir
    echo "Criando Resource Group: $RESOURCE_GROUP..."
    az group create --name "$RESOURCE_GROUP" --location "$LOCATION" || {
        echo "ERRO: Falha ao criar Resource Group"
        exit 1
    }
fi

# Verificar se App Service Plan já existe no Resource Group
echo "Verificando App Service Plan: $APP_SERVICE_PLAN..."
PLAN_EXISTS=$(az appservice plan list --resource-group "$RESOURCE_GROUP" --query "[?name=='$APP_SERVICE_PLAN']" -o tsv)

if [ -z "$PLAN_EXISTS" ]; then
    # Criar App Service Plan apenas se não existir
    echo "Criando App Service Plan: $APP_SERVICE_PLAN..."
    az appservice plan create \
        --name "$APP_SERVICE_PLAN" \
        --resource-group "$RESOURCE_GROUP" \
        --sku "$SKU" \
        --is-linux || {
        echo "ERRO: Falha ao criar App Service Plan"
        exit 1
    }
else
    echo "App Service Plan '$APP_SERVICE_PLAN' já existe. Usando o existente."
fi

# ============================================================
# Criar Azure SQL Server e Database
# ============================================================
echo "Verificando Azure SQL Server: $SQL_SERVER_NAME..."
SQL_SERVER_EXISTS=$(az sql server list --resource-group "$RESOURCE_GROUP" --query "[?name=='$SQL_SERVER_NAME']" -o tsv 2>/dev/null)

if [ -z "$SQL_SERVER_EXISTS" ]; then
    echo "Criando Azure SQL Server: $SQL_SERVER_NAME..."
    az sql server create \
        --name "$SQL_SERVER_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --location "$LOCATION" \
        --admin-user "$SQL_ADMIN_USER" \
        --admin-password "$SQL_ADMIN_PASSWORD" || {
        echo "ERRO: Falha ao criar SQL Server"
        exit 1
    }
    
    # Configurar firewall para permitir serviços Azure
    echo "Configurando firewall do SQL Server..."
    az sql server firewall-rule create \
        --resource-group "$RESOURCE_GROUP" \
        --server "$SQL_SERVER_NAME" \
        --name "AllowAzureServices" \
        --start-ip-address "0.0.0.0" \
        --end-ip-address "0.0.0.0" || {
        echo "AVISO: Falha ao configurar firewall (pode já existir)"
    }
else
    echo "SQL Server '$SQL_SERVER_NAME' já existe. Usando o existente."
fi

# Verificar se SQL Database já existe
echo "Verificando Azure SQL Database: $SQL_DB_NAME..."
SQL_DB_EXISTS=$(az sql db list --resource-group "$RESOURCE_GROUP" --server "$SQL_SERVER_NAME" --query "[?name=='$SQL_DB_NAME']" -o tsv 2>/dev/null)

if [ -z "$SQL_DB_EXISTS" ]; then
    echo "Criando Azure SQL Database: $SQL_DB_NAME..."
    az sql db create \
        --resource-group "$RESOURCE_GROUP" \
        --server "$SQL_SERVER_NAME" \
        --name "$SQL_DB_NAME" \
        --service-objective "Basic" \
        --backup-storage-redundancy "Local" || {
        echo "ERRO: Falha ao criar SQL Database"
        exit 1
    }
    echo "✅ Azure SQL Database criado com sucesso!"
else
    echo "SQL Database '$SQL_DB_NAME' já existe. Usando o existente."
fi

# Construir connection string do Azure SQL
AZURE_SQL_CONNECTION_STRING="Server=tcp:$SQL_SERVER_NAME.database.windows.net,1433;Initial Catalog=$SQL_DB_NAME;Persist Security Info=False;User ID=$SQL_ADMIN_USER;Password=$SQL_ADMIN_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# Verificar se App Service já existe
WEBAPP_EXISTS=$(az webapp list --resource-group "$RESOURCE_GROUP" --query "[?name=='$APP_SERVICE_NAME']" -o tsv)

if [ -z "$WEBAPP_EXISTS" ]; then
    # Criar App Service apenas se não existir
    echo "Criando App Service: $APP_SERVICE_NAME..."
    az webapp create \
        --name "$APP_SERVICE_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --plan "$APP_SERVICE_PLAN" \
        --runtime "DOTNETCORE|8.0" || {
        echo "ERRO: Falha ao criar App Service"
        exit 1
    }
else
    echo "App Service '$APP_SERVICE_NAME' já existe. Usando o existente."
fi

# Configurar App Settings
echo "Configurando App Settings..."

# Construir connection string do Oracle
ORACLE_CONNECTION_STRING="Data Source=$ORACLE_HOST:$ORACLE_PORT/$ORACLE_SID;User Id=$ORACLE_USER;Password=$ORACLE_PASS;"

az webapp config appsettings set \
    --name "$APP_SERVICE_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --settings \
        ASPNETCORE_ENVIRONMENT="Production" \
        ConnectionStrings__OracleConnection="$ORACLE_CONNECTION_STRING" \
        ConnectionStrings__AzureSqlConnection="$AZURE_SQL_CONNECTION_STRING" \
        ApiKey__HeaderName="X-API-Key" \
        ApiKey__Value="change-in-production" || {
    echo "ERRO: Falha ao configurar App Settings"
    exit 1
}

# Configurar sempre on
echo "Configurando Always On..."
az webapp config set \
    --name "$APP_SERVICE_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --always-on true || {
    echo "AVISO: Falha ao configurar Always On"
}

echo "=========================================="
echo "Infraestrutura criada com sucesso!"
echo "=========================================="
echo "Resource Group: $RESOURCE_GROUP"
echo "App Service: https://$APP_SERVICE_NAME.azurewebsites.net"
echo "SQL Server: $SQL_SERVER_NAME.database.windows.net"
echo "SQL Database: $SQL_DB_NAME"
echo "=========================================="
echo ""
echo "IMPORTANTE: Execute o script SQL para criar as tabelas:"
echo "  scripts/script-bd-azure.sql"
echo "  Conecte-se ao SQL Server e execute o script"
echo "=========================================="
