# Instruções para Criar o App Service no Azure

## Problema
O pipeline de Release está falhando porque o App Service `api-genfit-rm558515` não existe no Azure.

## Solução

Você precisa criar o App Service antes de executar o pipeline de Release. Existem duas opções:

### Opção 1: Executar o Script infra-app.sh na Pipeline de CI (Recomendado)

Adicione uma etapa na pipeline de CI para criar a infraestrutura antes do build:

1. Na pipeline de CI, adicione uma nova etapa **antes** do "Build and Test"
2. Adicione uma tarefa **Azure CLI**
3. Configure:
   - **Display name**: Criar Infraestrutura
   - **Azure Resource Manager connection**: Selecione sua conexão do Azure
   - **Script Type**: Shell Script
   - **Script path**: `infra-app.sh`
   - **Script Arguments**: 
     ```
     -ORACLE_HOST $(ORACLE_HOST) -ORACLE_PORT $(ORACLE_PORT) -ORACLE_SID $(ORACLE_SID) -ORACLE_USER $(ORACLE_USER) -ORACLE_PASS $(ORACLE_PASS) -LOCATION $(LOCATION) -NOME_WEBAPP $(NOME_WEBAPP)
     ```

4. Configure a dependência: O "Build and Test" deve depender de "Criar Infraestrutura"

### Opção 2: Executar o Script Manualmente via Azure Cloud Shell

1. Acesse o [Azure Cloud Shell](https://shell.azure.com/)
2. Faça upload do arquivo `infra-app.sh`
3. Execute:
   ```bash
   chmod +x infra-app.sh
   ./infra-app.sh -ORACLE_HOST oracle.fiap.com.br -ORACLE_PORT 1521 -ORACLE_SID ORCL -ORACLE_USER rm558515 -ORACLE_PASS "Fiap#2025" -LOCATION brazilsouth -NOME_WEBAPP api-genfit-rm558515
   ```

### Opção 3: Criar Manualmente no Portal do Azure

1. Acesse o [Portal do Azure](https://portal.azure.com)
2. Clique em "Create a resource"
3. Procure por "Web App"
4. Configure:
   - **Resource Group**: Crie um novo (ex: `rg-genfit-20251122`)
   - **Name**: `api-genfit-rm558515`
   - **Publish**: Code
   - **Runtime stack**: .NET 8
   - **Operating System**: Linux (ou Windows)
   - **Region**: Brazil South
   - **App Service Plan**: Crie um novo (ex: `asp-genfit`)
5. Após criar, configure os App Settings:
   - `ASPNETCORE_ENVIRONMENT`: `Production`
   - `ConnectionStrings__OracleConnection`: `Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;`

## Verificar se o App Service Existe

Execute no Azure Cloud Shell:
```bash
az webapp list --query "[?name=='api-genfit-rm558515'].{Name:name, ResourceGroup:resourceGroup, State:state}" --output table
```

## Variáveis Necessárias no Pipeline

Certifique-se de que as seguintes variáveis estão configuradas na pipeline:

- `NOME_WEBAPP`: `api-genfit-rm558515`
- `ORACLE_HOST`: `oracle.fiap.com.br`
- `ORACLE_PORT`: `1521`
- `ORACLE_SID`: `ORCL`
- `ORACLE_USER`: `rm558515`
- `ORACLE_PASS`: `Fiap#2025` (marcar como secreto)
- `LOCATION`: `brazilsouth`

## Após Criar o App Service

Depois que o App Service for criado, o pipeline de Release deve funcionar corretamente.

