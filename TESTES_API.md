# Testes da API GenFit

## Pré-requisitos

1. API rodando em `http://localhost:5118`
2. Oracle Database configurado e acessível
3. API Key configurada (padrão: `test-api-key-dev`)

## Testes Básicos

### 1. Health Check (Sem autenticação)

```powershell
Invoke-RestMethod -Uri "http://localhost:5118/health" -Method Get
```

**Resultado esperado:**
```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "oracle-db",
      "status": "Healthy",
      "duration": "..."
    },
    {
      "name": "self",
      "status": "Healthy",
      "duration": "..."
    }
  ]
}
```

### 2. Wellcome Endpoint (Sem autenticação)

```powershell
Invoke-RestMethod -Uri "http://localhost:5118/api/v1/wellcome" -Method Get
```

**Resultado esperado:**
```
Bem-vindo à API GenFit - O Futuro do Trabalho
```

### 3. Listar Usuários (Com API Key)

```powershell
$headers = @{ "X-API-Key" = "test-api-key-dev" }
Invoke-RestMethod -Uri "http://localhost:5118/api/v1/users?pageNumber=1&pageSize=10" -Method Get -Headers $headers
```

**Resultado esperado:**
```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 0,
  "totalPages": 0,
  "hasPrevious": false,
  "hasNext": false,
  "links": {
    "self": "...",
    "first": "...",
    "last": "..."
  }
}
```

### 4. Criar Usuário (Com API Key)

```powershell
$headers = @{
    "X-API-Key" = "test-api-key-dev"
    "Content-Type" = "application/json"
}
$body = @{
    role = "candidate"
    nome = "Teste Usuario API"
    email = "teste.api@example.com"
    senhaHash = "hash123"
    cpf = "123.456.789-00"
    telefone = "(11) 99999-9999"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5118/api/v1/users" -Method Post -Headers $headers -Body $body
```

**Resultado esperado:**
```json
{
  "id": 1,
  "role": "candidate",
  "nome": "Teste Usuario API",
  "email": "teste.api@example.com",
  ...
}
```

### 5. Obter Usuário por ID

```powershell
$headers = @{ "X-API-Key" = "test-api-key-dev" }
Invoke-RestMethod -Uri "http://localhost:5118/api/v1/users/1" -Method Get -Headers $headers
```

## Testes com cURL (alternativa)

### Health Check
```bash
curl http://localhost:5118/health
```

### Wellcome
```bash
curl http://localhost:5118/api/v1/wellcome
```

### Listar Usuários
```bash
curl -H "X-API-Key: test-api-key-dev" http://localhost:5118/api/v1/users
```

### Criar Usuário
```bash
curl -X POST -H "X-API-Key: test-api-key-dev" -H "Content-Type: application/json" \
  -d '{"role":"candidate","nome":"Teste","email":"teste@example.com"}' \
  http://localhost:5118/api/v1/users
```

## Verificações Importantes

1. **Conexão Oracle:** O Health Check deve retornar status "Healthy" para "oracle-db"
2. **Versionamento:** Todos os endpoints devem estar em `/api/v1/`
3. **Paginação:** Endpoints de listagem devem retornar informações de paginação
4. **HATEOAS:** Respostas paginadas devem incluir links de navegação
5. **Status Codes:** Verificar que os status codes HTTP estão corretos (200, 201, 404, etc.)

## Problemas Comuns

### Erro 401 Unauthorized
- Verificar se o header `X-API-Key` está sendo enviado
- Verificar se o valor da API Key está correto no `appsettings.json`

### Erro de conexão Oracle
- Verificar se o Oracle está acessível: `telnet oracle.fiap.com.br 1521`
- Verificar credenciais no `appsettings.json`
- Verificar logs da aplicação para detalhes do erro

### Endpoint não encontrado (404)
- Verificar se a API está rodando
- Verificar a URL (deve incluir `/api/v1/`)
- Verificar o versionamento está configurado corretamente

