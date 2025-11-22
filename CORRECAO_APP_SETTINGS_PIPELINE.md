# Correção: App Settings no Pipeline de Release

## ❌ Problema Atual

O erro mostra que a connection string está sendo quebrada:
```
Error: BadRequest - Parameter name cannot be empty. (CODE: 400)
Data: {"ASPNETCORE_ENVIRONMENT":"Production","ConnectionStrings__OracleConnection":"Data","":"Id=rm558515;Password=Fiap#2025;"}
```

## ✅ Solução

### Opção 1: Formato Correto no Campo "App settings" (Recomendado)

No campo **App settings** da task "Deploy no Serviço de Aplicativo", use o formato **JSON** ou **linha por linha**:

**Formato JSON (uma linha):**
```json
{"ASPNETCORE_ENVIRONMENT":"$(ASPNETCORE_ENVIRONMENT)","ConnectionStrings__OracleConnection":"$(OracleConnection)"}
```

**Formato linha por linha (mais fácil de ler):**
```
ASPNETCORE_ENVIRONMENT=$(ASPNETCORE_ENVIRONMENT)
ConnectionStrings__OracleConnection=$(OracleConnection)
```

### Opção 2: Usar Variável com Escape Correto

Se preferir usar o formato `-KEY VALUE`, você precisa escapar a connection string corretamente. Mas é mais complicado.

### Opção 3: Remover App Settings do Pipeline (Mais Simples)

Como os App Settings já foram configurados diretamente no App Service (via script PowerShell), você pode **remover** o campo "App settings" da task de deploy.

Os App Settings já estão configurados no App Service:
- ✅ `ASPNETCORE_ENVIRONMENT`: Production
- ✅ `ConnectionStrings__OracleConnection`: Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;
- ✅ `ApiKey__HeaderName`: X-API-Key
- ✅ `ApiKey__Value`: change-in-production

## 📝 Passo a Passo para Corrigir

1. **Abra a Pipeline de Release no Azure DevOps**
2. **Edite o Stage "Deploy em Dev"**
3. **Clique na task "Deploy no Serviço de Aplicativo"**
4. **Encontre o campo "App settings"**
5. **Escolha uma das opções:**

   **Opção A - Remover (Recomendado):**
   - Deixe o campo **vazio** ou **remova** completamente
   - Os App Settings já estão configurados no App Service

   **Opção B - Usar formato correto:**
   - Use o formato linha por linha:
     ```
     ASPNETCORE_ENVIRONMENT=$(ASPNETCORE_ENVIRONMENT)
     ConnectionStrings__OracleConnection=$(OracleConnection)
     ```
   - Certifique-se de que a variável `$(OracleConnection)` está definida nas variáveis do Release Pipeline com o valor completo:
     ```
     Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;
     ```

6. **Salve a pipeline**
7. **Execute novamente o Release**

## ⚠️ Importante

- O formato `-ASPNETCORE_ENVIRONMENT $(ASPNETCORE_ENVIRONMENT) -ConnectionStrings_OracleConnection "$(OracleConnection)"` **NÃO funciona** porque o Azure DevOps interpreta os espaços e quebra a string.

- Use **duplo underscore** (`__`) para `ConnectionStrings__OracleConnection` (não `_`)

- A variável `$(OracleConnection)` deve conter a connection string **completa**:
  ```
  Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;
  ```

## ✅ Verificação

Após o deploy, verifique se os App Settings estão corretos:
```bash
az webapp config appsettings list --name api-genfit-rm558515 --resource-group rg-genfit-20251122 --query "[?name=='ASPNETCORE_ENVIRONMENT' || name=='ConnectionStrings__OracleConnection'].{Name:name, Value:value}" --output table
```

