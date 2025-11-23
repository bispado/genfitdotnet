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
echo "=========================================="

# Verificar se está logado no Azure
echo "Verificando login no Azure..."
az account show > /dev/null 2>&1 || {
    echo "ERRO: Não está logado no Azure. Execute 'az login' primeiro."
    exit 1
}

# Criar Resource Group
echo "Criando Resource Group: $RESOURCE_GROUP..."
az group create --name "$RESOURCE_GROUP" --location "$LOCATION" || {
    echo "ERRO: Falha ao criar Resource Group"
    exit 1
}

# Criar App Service Plan
echo "Criando App Service Plan: $APP_SERVICE_PLAN..."
az appservice plan create \
    --name "$APP_SERVICE_PLAN" \
    --resource-group "$RESOURCE_GROUP" \
    --sku "$SKU" \
    --is-linux || {
    echo "ERRO: Falha ao criar App Service Plan"
    exit 1
}

# Criar App Service
echo "Criando App Service: $APP_SERVICE_NAME..."
az webapp create \
    --name "$APP_SERVICE_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --plan "$APP_SERVICE_PLAN" \
    --runtime "DOTNETCORE|8.0" || {
    echo "ERRO: Falha ao criar App Service"
    exit 1
}

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
echo "=========================================="
