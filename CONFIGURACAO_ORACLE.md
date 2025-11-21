# Configuração do Oracle Database - GenFit API

## Credenciais Configuradas

- **Usuario:** rm558515
- **Senha:** Fiap#2025
- **Host:** oracle.fiap.com.br
- **Porta:** 1521
- **SID:** ORCL

## Connection String

```
Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;
```

## Arquivo de Configuração

A connection string foi configurada em:
- `GenFit.API/appsettings.json`
- `GenFit.API/appsettings.Development.json`

## Testando a API

### 1. Iniciar a API

```bash
cd GenFit.API
dotnet run
```

A API estará disponível em:
- HTTP: http://localhost:5118
- Swagger: http://localhost:5118/swagger

### 2. Testar Health Check

```bash
curl http://localhost:5118/health
```

### 3. Testar Wellcome Endpoint

```bash
curl http://localhost:5118/api/v1/wellcome
```

### 4. Testar Listar Usuários (com API Key)

```bash
curl -H "X-API-Key: test-api-key-dev" http://localhost:5118/api/v1/users
```

### 5. Testar Criar Usuário (com API Key)

```bash
curl -X POST -H "X-API-Key: test-api-key-dev" -H "Content-Type: application/json" \
  -d '{"role":"candidate","nome":"Teste Usuario","email":"teste@example.com"}' \
  http://localhost:5118/api/v1/users
```

## Verificar Conexão Oracle

O Health Check endpoint verifica automaticamente a conexão com o Oracle:
- Endpoint: `/health`
- Retorna status da conexão com Oracle
- Verifica disponibilidade do banco de dados

## Troubleshooting

Se houver problemas de conexão:

1. Verifique se o Oracle está acessível:
   ```bash
   telnet oracle.fiap.com.br 1521
   ```

2. Verifique as credenciais no `appsettings.json`

3. Verifique os logs da aplicação para erros de conexão

4. Teste a connection string diretamente com um cliente Oracle

## Próximos Passos

1. Executar migrations do Entity Framework Core (se necessário)
2. Testar todas as procedures Oracle
3. Verificar se as tabelas existem no banco
4. Testar todos os endpoints da API

