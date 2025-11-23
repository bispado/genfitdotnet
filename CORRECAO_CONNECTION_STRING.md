# Correção da Connection String do Oracle no Azure App Service

## Problema
Erro ao criar job via Swagger: `ORA-50007: Connection string is not well-formed`

## Solução

### Opção 1: Via Portal do Azure (Recomendado)

1. Acesse o [Portal do Azure](https://portal.azure.com)
2. Navegue até o App Service: `api-genfit-rm558515`
3. Vá em **Configuration** → **Application settings**
4. Procure por `ConnectionStrings__OracleConnection`
5. Se não existir, clique em **+ New application setting**
6. Configure:
   - **Name:** `ConnectionStrings__OracleConnection`
   - **Value:** `Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;`
7. Clique em **Save**
8. Aguarde a reinicialização do App Service

### Opção 2: Via Azure CLI

```bash
az webapp config appsettings set \
  --name api-genfit-rm558515 \
  --resource-group rg-genfit-YYYYMMDD \
  --settings ConnectionStrings__OracleConnection="Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;"
```

**Importante:** Se a senha contém caracteres especiais (como `#`), pode ser necessário usar aspas simples ou escapar o caractere.

### Opção 3: Via Release Pipeline (Azure DevOps)

1. Acesse o Azure DevOps: `https://dev.azure.com/motosync/genfit`
2. Vá em **Pipelines** → **Releases** → `Deploy em dev`
3. Clique em **Edit**
4. Vá na etapa de deploy do App Service
5. Na seção **Application and Configuration Settings**, adicione:
   - **App settings:**
     ```
     -ASPNETCORE_ENVIRONMENT Production
     -ConnectionStrings__OracleConnection "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;"
     ```
6. Salve e faça um novo deploy

### Opção 4: Atualizar via Script (Recomendado para automação)

Execute o script de infraestrutura novamente com os parâmetros corretos:

```bash
bash scripts/script-infra-app.sh \
  -ORACLE_HOST oracle.fiap.com.br \
  -ORACLE_PORT 1521 \
  -ORACLE_SID ORCL \
  -ORACLE_USER rm558515 \
  -ORACLE_PASS "Fiap#2025" \
  -LOCATION brazilsouth
```

**Nota:** A senha com `#` deve estar entre aspas duplas.

## Verificação

Após configurar, teste a API:

1. Acesse: `https://api-genfit-rm558515.azurewebsites.net/health`
2. Deve retornar status `Healthy` se a connection string estiver correta
3. Teste criar um job via Swagger: `POST /api/v1/Jobs`

## Formato Correto da Connection String

```
Data Source=HOST:PORT/SID;User Id=USER;Password=PASSWORD;
```

Exemplo:
```
Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;
```

## Caracteres Especiais na Senha

Se a senha contém caracteres especiais, pode ser necessário:
- Usar aspas duplas: `"Fiap#2025"`
- Escapar caracteres: `Fiap\#2025` (dependendo do contexto)
- Usar variáveis de ambiente no Azure DevOps (recomendado para produção)

## Variáveis de Ambiente no Azure DevOps

Para maior segurança, configure a connection string como variável secreta:

1. Azure DevOps → **Pipelines** → **Library**
2. Crie um **Variable Group** (ex: `genfit-secrets`)
3. Adicione variável:
   - **Name:** `OracleConnection`
   - **Value:** `Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;`
   - **Marque como Secret**
4. No Release Pipeline, vincule o Variable Group
5. Use no App Settings: `ConnectionStrings__OracleConnection=$(OracleConnection)`

