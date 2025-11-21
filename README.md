# GenFit API - Sistema de Gestão de RH e Candidatos

> API RESTful desenvolvida em .NET 8 para o sistema GenFit, voltada ao tema **"O Futuro do Trabalho"**. O sistema permite gerenciar candidatos, vagas de emprego, skills, cursos de requalificação e análises de compatibilidade usando IA.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?style=flat&logo=oracle)](https://www.oracle.com/database/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=flat&logo=.net)](https://learn.microsoft.com/ef/core/)

## 🚀 Tecnologias

- **.NET 8.0** - Framework principal
- **Entity Framework Core** - ORM com provider Oracle
- **Oracle Database** - Banco de dados relacional
- **Serilog** - Logging estruturado
- **OpenTelemetry** - Tracing e observabilidade
- **xUnit** - Framework de testes
- **Swagger/OpenAPI** - Documentação interativa
- **Asp.Versioning** - Versionamento de API

## 📋 Funcionalidades

✅ **Boas Práticas REST**
- Paginação e HATEOAS em todos os endpoints de listagem
- Status codes HTTP adequados
- Uso correto dos verbos HTTP (GET, POST, PUT, DELETE)

✅ **Monitoramento e Observabilidade**
- Health Check customizado para Oracle Database
- Logging estruturado com Serilog (console e arquivo)
- Tracing distribuído com OpenTelemetry

✅ **Versionamento de API**
- Versionamento via URL (`/api/v1/`)
- Suporte a múltiplas versões simultâneas
- Controle de versão via query string, header ou media type

✅ **Integração e Persistência**
- Entity Framework Core com Oracle
- Migrations automatizadas
- Integração com Stored Procedures Oracle
  - `PRC_INSERT_USER`
  - `PRC_INSERT_JOB`
  - `PRC_INSERT_CANDIDATE_SKILL`
  - `PRC_INSERT_MODEL_RESULT`

✅ **Autenticação e Segurança**
- Autenticação via API Key
- Middleware customizado para validação
- Endpoints públicos (health, swagger, welcome)

✅ **Testes Automatizados**
- Testes unitários com xUnit
- Testes de integração (em desenvolvimento)

## 🏗️ Estrutura do Projeto

```
GenFit/
├── GenFit.API/              # Projeto principal da API
│   ├── Controllers/V1/      # Controllers versionados
│   ├── Middleware/          # Middleware customizado
│   └── Program.cs           # Configuração da aplicação
├── GenFit.Core/             # Entidades de domínio
│   └── Entities/            # Entidades (User, Job, Skill, etc.)
├── GenFit.Infrastructure/   # Camada de infraestrutura
│   ├── Data/                # DbContext e configurações EF
│   ├── HealthChecks/        # Health checks customizados
│   └── Services/            # Serviços de infraestrutura
├── GenFit.Application/      # Camada de aplicação
│   ├── DTOs/                # Data Transfer Objects
│   ├── Services/            # Serviços de aplicação
│   └── Common/              # Classes compartilhadas
└── GenFit.Tests/           # Testes unitários e de integração
```

## ⚙️ Configuração

### Pré-requisitos

- .NET 8.0 SDK
- Oracle Database (ou acesso a um servidor Oracle)
- Visual Studio 2022 / VS Code / Rider (opcional)

### Instalação

1. Clone o repositório:
```bash
git clone https://github.com/bispado/genfitdotnet.git
cd genfitdotnet
```

2. Configure a connection string no `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "OracleConnection": "Data Source=HOST:PORT/SID;User Id=USER;Password=PASSWORD;"
  },
  "ApiKey": {
    "HeaderName": "X-API-Key",
    "Value": "your-secret-api-key"
  }
}
```

3. Restaure as dependências:
```bash
dotnet restore
```

4. Execute as migrations (se necessário):
```bash
dotnet ef database update --project GenFit.Infrastructure --startup-project GenFit.API
```

5. Execute a API:
```bash
dotnet run --project GenFit.API
```

A API estará disponível em: `http://localhost:5118`

## 📚 Endpoints Principais

### Jobs (Vagas) - CRUD Completo
- `GET /api/v1/jobs` - Lista vagas com paginação e HATEOAS
- `GET /api/v1/jobs/{id}` - Obtém vaga por ID
- `POST /api/v1/jobs` - Cria vaga (via procedure Oracle)
- `PUT /api/v1/jobs/{id}` - Atualiza vaga
- `DELETE /api/v1/jobs/{id}` - Remove vaga

### Users (Usuários)
- `GET /api/v1/users` - Lista usuários com paginação
- `GET /api/v1/users/{id}` - Obtém usuário por ID
- `POST /api/v1/users` - Cria usuário (via procedure Oracle)
- `PUT /api/v1/users/{id}` - Atualiza usuário
- `DELETE /api/v1/users/{id}` - Remove usuário

### Skills (Competências)
- `GET /api/v1/skills` - Lista skills com paginação
- `GET /api/v1/skills/{id}` - Obtém skill por ID

### Health Check
- `GET /health` - Verifica saúde da API e conexão com Oracle

### Welcome
- `GET /api/v1/wellcome` - Endpoint de teste (público)

## 🔐 Autenticação

A API utiliza autenticação via **API Key**. Configure a chave no `appsettings.json` e envie no header `X-API-Key` nas requisições.

**Exemplo de requisição:**
```bash
curl -X GET "http://localhost:5118/api/v1/jobs" \
  -H "X-API-Key: your-secret-api-key" \
  -H "Accept: application/json"
```

**Endpoints públicos (não requerem API Key):**
- `/health` - Health Check
- `/swagger` - Documentação Swagger
- `/api/v1/wellcome` - Endpoint de boas-vindas

## 🔍 Paginação e HATEOAS

Todos os endpoints de listagem suportam paginação:

```
GET /api/v1/jobs?pageNumber=1&pageSize=10
```

A resposta inclui links HATEOAS:

```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 100,
  "totalPages": 10,
  "hasPrevious": false,
  "hasNext": true,
  "links": {
    "self": "/api/v1/jobs?pageNumber=1&pageSize=10",
    "first": "/api/v1/jobs?pageNumber=1&pageSize=10",
    "last": "/api/v1/jobs?pageNumber=10&pageSize=10",
    "next": "/api/v1/jobs?pageNumber=2&pageSize=10"
  }
}
```

## 🏥 Health Check

O endpoint `/health` verifica:
- Conectividade com Oracle Database
- Status geral da aplicação

Resposta de exemplo:
```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "oracle-db",
      "status": "Healthy",
      "duration": "00:00:00.1234567"
    }
  ]
}
```

## 🧪 Testes

Execute os testes com:

```bash
dotnet test
```

## 📖 Swagger/OpenAPI

Acesse a documentação interativa em:
- **Desenvolvimento:** `http://localhost:5118/swagger`
- **Produção:** `https://{your-app-service}.azurewebsites.net/swagger`

## 🚀 Deploy no Azure

O projeto inclui o script `infra-app.sh` para configuração automática da infraestrutura no Azure via Azure DevOps Pipeline.

### Variáveis de Ambiente

Configure no Azure App Service:
- `ASPNETCORE_ENVIRONMENT`: Production
- `ConnectionStrings__OracleConnection`: String de conexão do Oracle
- `ApiKey__HeaderName`: X-API-Key
- `ApiKey__Value`: Sua chave secreta

## 📝 Versionamento da API

A API utiliza versionamento via URL:
- **Versão atual:** `v1`
- **Formato:** `/api/v1/{resource}`

O versionamento pode ser especificado via:
- URL: `/api/v1/jobs`
- Query string: `?api-version=1.0`
- Header: `X-Version: 1.0`
- Media type: `application/json;ver=1.0`

## 🎯 Status Codes HTTP

- `200 OK` - Sucesso
- `201 Created` - Recurso criado com sucesso
- `204 No Content` - Recurso removido com sucesso
- `400 Bad Request` - Requisição inválida
- `401 Unauthorized` - API Key inválida ou ausente
- `404 Not Found` - Recurso não encontrado
- `500 Internal Server Error` - Erro interno do servidor

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos como parte do curso FIAP.

## 👥 Contribuição

Desenvolvido para o projeto **"O Futuro do Trabalho"** - GenFit

## 📞 Suporte

Para questões sobre a API, consulte a documentação Swagger ou abra uma issue no repositório.

---

**🔗 Repositório:** [https://github.com/bispado/genfitdotnet](https://github.com/bispado/genfitdotnet)
