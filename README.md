# GenFit API - Sistema de Gestão de RH e Candidatos

> **Global Solution (GS) - DevOps Tools & Cloud Computing**  
> API RESTful desenvolvida em .NET 8 para o sistema GenFit, voltada ao tema **"O Futuro do Trabalho"**. O sistema permite gerenciar candidatos, vagas de emprego, skills, cursos de requalificação e análises de compatibilidade usando IA.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?style=flat&logo=oracle)](https://www.oracle.com/database/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=flat&logo=.net)](https://learn.microsoft.com/ef/core/)
[![Azure DevOps](https://img.shields.io/badge/Azure%20DevOps-0078D4?style=flat&logo=azure-devops)](https://dev.azure.com/motosync/genfit)

---

## 🔗 Links da Entrega

- **🔗 Azure DevOps Organization:** [https://dev.azure.com/motosync/genfit](https://dev.azure.com/motosync/genfit)
- **📦 Repositório Azure DevOps:** [https://dev.azure.com/motosync/genfit/_git/genfit-CI](https://dev.azure.com/motosync/genfit/_git/genfit-CI)
- **🌐 API em Produção:** [https://api-genfit-rm558515.azurewebsites.net](https://api-genfit-rm558515.azurewebsites.net)
- **📖 Swagger (Documentação):** [https://api-genfit-rm558515.azurewebsites.net/swagger](https://api-genfit-rm558515.azurewebsites.net/swagger)
- **🏥 Health Check:** [https://api-genfit-rm558515.azurewebsites.net/health](https://api-genfit-rm558515.azurewebsites.net/health)
- **🔗 Repositório GitHub:** [https://github.com/bispado/genfitdotnet](https://github.com/bispado/genfitdotnet)

---

## 🎯 DevOps Tools & Cloud Computing (GS)

Este projeto foi desenvolvido como solução para a **Global Solution (GS)** de **DevOps Tools & Cloud Computing**, demonstrando a integração completa das ferramentas Azure DevOps conforme os requisitos da entrega.

### ✅ Requisitos Atendidos

#### 1. Provisionamento em Nuvem (Azure CLI) ✅
- **Script:** `scripts/script-infra-app.sh`
- **Recursos criados:**
  - Resource Group (`rg-genfit-YYYYMMDD`)
  - App Service Plan (`asp-genfit` - SKU B1, Linux)
  - App Service (`api-genfit-rm558515` - .NET 8.0)
- **Características:**
  - Script idempotente (pode ser executado múltiplas vezes)
  - Verifica recursos existentes antes de criar
  - Configura App Settings automaticamente
  - Suporta parâmetros nomeados e variáveis de ambiente

#### 2. Projeto no Azure DevOps ✅
- **Organização:** `https://dev.azure.com/motosync/genfit`
- **Projeto:** GenFit
- **Permissões:** Professor convidado com permissões Basic (Organização) e Contributor (Projeto)

#### 3. Código no Azure Repos ✅
- **Repositório:** `https://dev.azure.com/motosync/genfit/_git/genfit-CI`
- **Branch principal:** `main` (protegida)
- **Políticas de branch:**
  - ✅ Revisor obrigatório
  - ✅ Vinculação de Work Item obrigatória
  - ✅ Revisor padrão configurado
- **Versionamento:** Git completo com histórico de commits

#### 4. Azure Boards ✅
- **Work Items:** Criados e vinculados a commits, branches e Pull Requests
- **Rastreamento:** Histórico completo do ciclo de vida do desenvolvimento
- **Links:** Commits, branches e PRs vinculados aos Work Items

#### 5. Pipeline de Build (CI) ✅
- **Tipo:** YAML (`azure-pipelines.yml` na raiz)
- **Trigger:** Automaticamente após merge via Pull Request na branch `main`
- **Etapas:**
  1. Provisionamento de infraestrutura via Azure CLI
  2. Restore de dependências .NET
  3. Build da aplicação
  4. Execução de testes automatizados (xUnit)
  5. **Publicação de resultados de testes** (formato VSTest/TRX)
  6. Publicação de artefatos para deploy
- **Testes publicados:** Resultados visíveis na aba "Tests" do Azure DevOps

#### 6. Pipeline de Release (CD) ✅
- **Tipo:** Classic Release Pipeline
- **Nome:** `Deploy em dev`
- **Trigger:** Automaticamente após Build gerar novo artefato
- **Etapas:**
  1. Download de artefatos da Build Pipeline
  2. Deploy automático para Azure App Service
  3. Configuração de App Settings via variáveis de ambiente

#### 7. Requisitos de Implementação ✅
- ✅ Projeto privado com Git para versionamento
- ✅ Azure Boards vinculado ao Repos (Commits, Branches, PRs)
- ✅ Branch principal protegida (Revisor obrigatório, Work Item obrigatório, Revisor padrão)
- ✅ Build acionado somente após Merge via PR
- ✅ Aluno pode aprovar sua própria PR (simulação)
- ✅ Release executa automaticamente após novo artefato
- ✅ Deploy via Web App PaaS (Azure App Service)
- ✅ Banco de dados em PaaS (Oracle Database - FIAP Cloud)
- ✅ Scripts de infraestrutura no repositório (`scripts/script-infra-app.sh`)
- ✅ Arquivo `scripts/script-bd.sql` na pasta `/scripts`
- ✅ Scripts Azure CLI com prefixo `script-infra` (`script-infra-app.sh`)
- ✅ Arquivo `azure-pipelines.yml` na raiz do repositório (YAML)
- ✅ CRUD exposto em JSON no README (veja seção abaixo)
- ✅ Variáveis de ambiente protegidas (senhas não expostas)
- ✅ Desenho macro da arquitetura (veja seção abaixo)

---

## 📊 Arquitetura da Solução

### Diagrama Macro da Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│                    Azure App Service                        │
│              (PaaS - Linux, .NET 8.0)                       │
│  URL: https://api-genfit-rm558515.azurewebsites.net        │
├─────────────────────────────────────────────────────────────┤
│  Camada de Apresentação (GenFit.API)                        │
│  ├── Controllers/V1/ (Jobs, Users, Skills)                  │
│  ├── Middleware/ (API Key Authentication)                   │
│  └── Program.cs (Configuração, Health Checks, Swagger)      │
│  ↓                                                           │
│  Camada de Aplicação (GenFit.Application)                   │
│  ├── Services/ (JobService, UserService)                    │
│  ├── DTOs/ (JobDto, UserDto, CreateJobDto)                  │
│  └── Common/ (PagedResult, PaginationParameters)            │
│  ↓                                                           │
│  Camada de Infraestrutura (GenFit.Infrastructure)          │
│  ├── Data/ (GenFitDbContext, Entity Configurations)         │
│  ├── Services/ (OracleProcedureService)                      │
│  └── HealthChecks/ (OracleHealthCheck)                      │
│  ↓                                                           │
│  Camada de Domínio (GenFit.Core)                            │
│  └── Entities/ (User, Job, Skill, Course, etc.)             │
└─────────────────────────────────────────────────────────────┘
                        ↓ Entity Framework Core
┌─────────────────────────────────────────────────────────────┐
│              Oracle Database (FIAP Cloud)                   │
│  Host: oracle.fiap.com.br:1521/ORCL                        │
│  Tabelas: USERS, JOBS, SKILLS, COURSES,                     │
│          CANDIDATE_SKILLS, JOB_SKILLS,                      │
│          MODEL_RESULTS, AUDIT_LOGS, etc.                    │
│  Stored Procedures: PRC_INSERT_USER, PRC_INSERT_JOB, etc.  │
└─────────────────────────────────────────────────────────────┘
```

### Fluxo CI/CD Completo

```
┌─────────────┐
│  Developer  │
└──────┬──────┘
       │ Commit
       ↓
┌─────────────┐
│   Branch    │ (test/feature)
└──────┬──────┘
       │ Push
       ↓
┌─────────────┐
│ Pull Request│ (vinculado a Work Item)
└──────┬──────┘
       │ Aprovação + Merge
       ↓
┌─────────────────────────────────────┐
│     Build Pipeline (CI)             │
│  ┌───────────────────────────────┐  │
│  │ 1. Provisionamento (Azure CLI) │  │
│  │ 2. Restore Dependencies       │  │
│  │ 3. Build Application          │  │
│  │ 4. Run Tests (xUnit)          │  │
│  │ 5. Publish Test Results       │  │
│  │ 6. Publish Artifacts          │  │
│  └───────────────────────────────┘  │
└──────────────┬──────────────────────┘
               │ Trigger automático
               ↓
┌─────────────────────────────────────┐
│    Release Pipeline (CD)            │
│  ┌───────────────────────────────┐  │
│  │ 1. Download Artifacts         │  │
│  │ 2. Deploy to App Service      │  │
│  │ 3. Configure App Settings     │  │
│  └───────────────────────────────┘  │
└──────────────┬──────────────────────┘
               │
               ↓
┌─────────────────────────────────────┐
│   Azure App Service (Produção)      │
│   https://api-genfit-rm558515...     │
└─────────────────────────────────────┘
```

---

## 🚀 Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **Entity Framework Core** - ORM com provider Oracle
- **Oracle Database** - Banco de dados relacional (FIAP Cloud)
- **Serilog** - Logging estruturado
- **OpenTelemetry** - Tracing e observabilidade
- **xUnit** - Framework de testes
- **Swagger/OpenAPI** - Documentação interativa
- **Asp.Versioning** - Versionamento de API
- **Azure App Service** - PaaS para hospedagem
- **Azure DevOps** - CI/CD, Boards, Repos, Pipelines

---

## 📋 Funcionalidades da API

✅ **Boas Práticas REST**
- Paginação e HATEOAS em todos os endpoints de listagem
- Status codes HTTP adequados (200, 201, 204, 400, 404, 500)
- Uso correto dos verbos HTTP (GET, POST, PUT, DELETE)

✅ **Monitoramento e Observabilidade**
- Health Check customizado para Oracle Database (`/health`)
- Logging estruturado com Serilog (console e arquivo)
- Tracing distribuído com OpenTelemetry

✅ **Versionamento de API**
- Versionamento via URL (`/api/v1/`)
- Suporte a múltiplas versões simultâneas
- Controle via query string, header ou media type

✅ **Integração e Persistência**
- Entity Framework Core com Oracle
- Migrations automatizadas
- **Integração com Stored Procedures Oracle:**
  - `PRC_INSERT_USER` - Criação de usuários
  - `PRC_INSERT_JOB` - Criação de vagas
  - `PRC_INSERT_CANDIDATE_SKILL` - Vinculação de skills
  - `PRC_INSERT_MODEL_RESULT` - Resultados de IA

✅ **Autenticação e Segurança**
- Autenticação via API Key
- Middleware customizado para validação
- Endpoints públicos (health, swagger, welcome)

✅ **Testes Automatizados**
- Testes unitários com xUnit
- Testes de integração para controllers
- Publicação automática na Build Pipeline

---

## 🔄 Exemplos de CRUD em JSON

### Tabela: JOBS (Vagas)

#### CREATE - Criar Vaga
**POST** `/api/v1/jobs`
```json
{
  "titulo": "Desenvolvedor .NET Senior",
  "descricao": "Vaga para desenvolvedor .NET com experiência em APIs RESTful e Oracle Database",
  "salario": 12000.00,
  "localizacao": "São Paulo - SP",
  "tipoContrato": "CLT",
  "nivel": "Senior",
  "modeloTrabalho": "Híbrido",
  "departamento": "Tecnologia"
}
```

**Resposta (201 Created):**
```json
{
  "id": 1,
  "titulo": "Desenvolvedor .NET Senior",
  "descricao": "Vaga para desenvolvedor .NET com experiência em APIs RESTful e Oracle Database",
  "salario": 12000.00,
  "localizacao": "São Paulo - SP",
  "tipoContrato": "CLT",
  "nivel": "Senior",
  "modeloTrabalho": "Híbrido",
  "departamento": "Tecnologia",
  "createdAt": "2025-11-22T10:00:00Z",
  "updatedAt": "2025-11-22T10:00:00Z"
}
```

#### READ - Listar Vagas
**GET** `/api/v1/jobs?pageNumber=1&pageSize=10`

**Resposta (200 OK):**
```json
{
  "items": [
    {
      "id": 1,
      "titulo": "Desenvolvedor .NET Senior",
      "descricao": "Vaga para desenvolvedor .NET...",
      "salario": 12000.00,
      "localizacao": "São Paulo - SP",
      "tipoContrato": "CLT",
      "nivel": "Senior",
      "modeloTrabalho": "Híbrido",
      "departamento": "Tecnologia",
      "createdAt": "2025-11-22T10:00:00Z",
      "updatedAt": "2025-11-22T10:00:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPrevious": false,
  "hasNext": false,
  "links": {
    "self": "/api/v1/jobs?pageNumber=1&pageSize=10",
    "first": "/api/v1/jobs?pageNumber=1&pageSize=10",
    "last": "/api/v1/jobs?pageNumber=1&pageSize=10"
  }
}
```

#### READ - Obter Vaga por ID
**GET** `/api/v1/jobs/1`

**Resposta (200 OK):**
```json
{
  "id": 1,
  "titulo": "Desenvolvedor .NET Senior",
  "descricao": "Vaga para desenvolvedor .NET com experiência em APIs RESTful e Oracle Database",
  "salario": 12000.00,
  "localizacao": "São Paulo - SP",
  "tipoContrato": "CLT",
  "nivel": "Senior",
  "modeloTrabalho": "Híbrido",
  "departamento": "Tecnologia",
  "createdAt": "2025-11-22T10:00:00Z",
  "updatedAt": "2025-11-22T10:00:00Z"
}
```

#### UPDATE - Atualizar Vaga
**PUT** `/api/v1/jobs/1`
```json
{
  "titulo": "Desenvolvedor .NET Senior - Atualizado",
  "salario": 15000.00,
  "localizacao": "São Paulo - SP (Remoto)"
}
```

**Resposta (200 OK):**
```json
{
  "id": 1,
  "titulo": "Desenvolvedor .NET Senior - Atualizado",
  "descricao": "Vaga para desenvolvedor .NET com experiência em APIs RESTful e Oracle Database",
  "salario": 15000.00,
  "localizacao": "São Paulo - SP (Remoto)",
  "tipoContrato": "CLT",
  "nivel": "Senior",
  "modeloTrabalho": "Híbrido",
  "departamento": "Tecnologia",
  "createdAt": "2025-11-22T10:00:00Z",
  "updatedAt": "2025-11-22T11:00:00Z"
}
```

#### DELETE - Remover Vaga
**DELETE** `/api/v1/jobs/1`

**Resposta (200 OK):**
```json
{
  "message": "Deletado com sucesso",
  "id": 1
}
```

### Tabela: USERS (Usuários)

#### CREATE - Criar Usuário
**POST** `/api/v1/users`
```json
{
  "role": "candidate",
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "senhaHash": "hashed_password_here",
  "cpf": "123.456.789-00",
  "telefone": "(11) 99999-9999",
  "dataNascimento": "1990-01-15T00:00:00Z",
  "linkedInUrl": "https://linkedin.com/in/joaosilva"
}
```

**Resposta (201 Created):**
```json
{
  "id": 1,
  "role": "candidate",
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "cpf": "123.456.789-00",
  "telefone": "(11) 99999-9999",
  "dataNascimento": "1990-01-15T00:00:00Z",
  "linkedInUrl": "https://linkedin.com/in/joaosilva",
  "createdAt": "2025-11-22T10:00:00Z",
  "updatedAt": "2025-11-22T10:00:00Z"
}
```

#### READ - Listar Usuários
**GET** `/api/v1/users?pageNumber=1&pageSize=10`

**Resposta (200 OK):**
```json
{
  "items": [
    {
      "id": 1,
      "role": "candidate",
      "nome": "João Silva",
      "email": "joao.silva@example.com",
      "cpf": "123.456.789-00",
      "telefone": "(11) 99999-9999",
      "dataNascimento": "1990-01-15T00:00:00Z",
      "linkedInUrl": "https://linkedin.com/in/joaosilva",
      "createdAt": "2025-11-22T10:00:00Z",
      "updatedAt": "2025-11-22T10:00:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPrevious": false,
  "hasNext": false,
  "links": {
    "self": "/api/v1/users?pageNumber=1&pageSize=10",
    "first": "/api/v1/users?pageNumber=1&pageSize=10",
    "last": "/api/v1/users?pageNumber=1&pageSize=10"
  }
}
```

#### READ - Obter Usuário por ID
**GET** `/api/v1/users/1`

**Resposta (200 OK):**
```json
{
  "id": 1,
  "role": "candidate",
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "cpf": "123.456.789-00",
  "telefone": "(11) 99999-9999",
  "dataNascimento": "1990-01-15T00:00:00Z",
  "linkedInUrl": "https://linkedin.com/in/joaosilva",
  "createdAt": "2025-11-22T10:00:00Z",
  "updatedAt": "2025-11-22T10:00:00Z"
}
```

#### UPDATE - Atualizar Usuário
**PUT** `/api/v1/users/1`
```json
{
  "nome": "João Silva Santos",
  "telefone": "(11) 88888-8888",
  "linkedInUrl": "https://linkedin.com/in/joaosilvasantos"
}
```

**Resposta (200 OK):**
```json
{
  "id": 1,
  "role": "candidate",
  "nome": "João Silva Santos",
  "email": "joao.silva@example.com",
  "cpf": "123.456.789-00",
  "telefone": "(11) 88888-8888",
  "dataNascimento": "1990-01-15T00:00:00Z",
  "linkedInUrl": "https://linkedin.com/in/joaosilvasantos",
  "createdAt": "2025-11-22T10:00:00Z",
  "updatedAt": "2025-11-22T11:00:00Z"
}
```

#### DELETE - Remover Usuário
**DELETE** `/api/v1/users/1`

**Resposta (204 No Content)**

---

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
├── GenFit.Tests/           # Testes unitários e de integração
├── scripts/
│   ├── script-infra-app.sh  # Script de provisionamento Azure CLI
│   └── script-bd.sql       # Script de criação do banco de dados
└── azure-pipelines.yml     # Pipeline YAML de Build
```

---

## 📚 Endpoints Principais

### Jobs (Vagas) - CRUD Completo
- `GET /api/v1/jobs` - Lista vagas com paginação e HATEOAS
- `GET /api/v1/jobs/{id}` - Obtém vaga por ID
- `POST /api/v1/jobs` - Cria vaga (via procedure Oracle `PRC_INSERT_JOB`)
- `PUT /api/v1/jobs/{id}` - Atualiza vaga
- `DELETE /api/v1/jobs/{id}` - Remove vaga

### Users (Usuários) - CRUD Completo
- `GET /api/v1/users` - Lista usuários com paginação
- `GET /api/v1/users/{id}` - Obtém usuário por ID
- `POST /api/v1/users` - Cria usuário (via procedure Oracle `PRC_INSERT_USER`)
- `PUT /api/v1/users/{id}` - Atualiza usuário
- `DELETE /api/v1/users/{id}` - Remove usuário

### Skills (Competências)
- `GET /api/v1/skills` - Lista skills com paginação
- `GET /api/v1/skills/{id}` - Obtém skill por ID

### Health Check
- `GET /health` - Verifica saúde da API e conexão com Oracle

### Welcome
- `GET /api/v1/wellcome` - Endpoint de teste (público)

---

## 🔐 Autenticação

A API utiliza autenticação via **API Key**. Configure a chave no `appsettings.json` e envie no header `X-API-Key` nas requisições.

**Endpoints públicos (não requerem API Key):**
- `/` - Rota raiz (redireciona para `/swagger`)
- `/health` - Health Check
- `/swagger` - Documentação Swagger
- `/api/v1/wellcome` - Endpoint de boas-vindas

---

## 🚀 Provisionamento de Infraestrutura

### Script de Infraestrutura

O script `scripts/script-infra-app.sh` cria automaticamente todos os recursos necessários no Azure:

**Recursos criados:**
- Resource Group (`rg-genfit-YYYYMMDD`)
- App Service Plan (`asp-genfit` - SKU B1, Linux)
- App Service (`api-genfit-rm558515` - .NET 8.0)
- App Settings configurados automaticamente

**Características:**
- ✅ Idempotente (pode ser executado múltiplas vezes)
- ✅ Verifica recursos existentes antes de criar
- ✅ Suporta parâmetros nomeados e posicionais
- ✅ Tratamento de erros robusto

**Uso na Pipeline:**
O script é executado automaticamente na Build Pipeline (Job 1: Criar Infra Inicial) com os parâmetros:
```bash
-ORACLE_HOST oracle.fiap.com.br
-ORACLE_PORT 1521
-ORACLE_SID ORCL
-ORACLE_USER rm558515
-ORACLE_PASS Fiap#2025
-LOCATION brazilsouth
```

---

## 🔄 CI/CD Pipeline (Azure DevOps)

### 📋 Azure Boards
- ✅ Work Items criados e vinculados a commits, branches e Pull Requests
- ✅ Rastreamento completo do ciclo de vida do desenvolvimento
- ✅ Histórico completo de alterações

### 📦 Azure Repos
- **Repositório:** `https://dev.azure.com/motosync/genfit/_git/genfit-CI`
- **Branch principal:** `main` (protegida)
- **Políticas de branch:**
  - ✅ Revisor obrigatório
  - ✅ Vinculação de Work Item obrigatória
  - ✅ Revisor padrão configurado

### 🔧 Azure Pipelines

#### Pipeline de Build (CI)
- **Arquivo:** `azure-pipelines.yml` (YAML na raiz)
- **Trigger:** Automaticamente após merge via Pull Request na branch `main`
- **Etapas:**
  1. **Provisionamento de infraestrutura** via Azure CLI
  2. **Restore** de dependências .NET
  3. **Build** da aplicação
  4. **Test** - Execução de testes automatizados (xUnit)
  5. **Publish Test Results** - Publicação de resultados (formato VSTest/TRX)
  6. **Publish** - Publicação de artefatos para deploy

#### Pipeline de Release (CD)
- **Tipo:** Classic Release Pipeline
- **Nome:** `Deploy em dev`
- **Trigger:** Automaticamente após Build gerar novo artefato
- **Etapas:**
  1. Download de artefatos da Build Pipeline
  2. Deploy automático para Azure App Service
  3. Configuração de App Settings via variáveis de ambiente

---

## 📄 Arquivos de Entrega

- ✅ `azure-pipelines.yml` - Pipeline YAML na raiz do repositório
- ✅ `scripts/script-infra-app.sh` - Script Azure CLI para provisionamento
- ✅ `scripts/script-bd.sql` - Script SQL com schema completo do banco
- ✅ `README.md` - Documentação completa com exemplos JSON de CRUD

---

## 🧪 Testes Automatizados

- **Framework:** xUnit
- **Cobertura:** Controllers, Services
- **Publicação:** Resultados publicados na Build Pipeline (formato VSTest/TRX)
- **Visualização:** Resultados visíveis na aba "Tests" do Azure DevOps

---

## 📖 Swagger/OpenAPI

Acesse a documentação interativa:
- **Produção:** `https://api-genfit-rm558515.azurewebsites.net/swagger`
- **Rota raiz:** `https://api-genfit-rm558515.azurewebsites.net/` (redireciona para Swagger)

---

## 📝 Versionamento da API

- **Versão atual:** `v1`
- **Formato:** `/api/v1/{resource}`
- **Controle via:** URL, query string, header ou media type

---

## 🎯 Status Codes HTTP

- `200 OK` - Sucesso
- `201 Created` - Recurso criado com sucesso
- `204 No Content` - Recurso removido com sucesso
- `400 Bad Request` - Requisição inválida
- `401 Unauthorized` - API Key inválida ou ausente
- `404 Not Found` - Recurso não encontrado
- `500 Internal Server Error` - Erro interno do servidor

---

## 📞 Informações de Contato

**Desenvolvido para:** Global Solution (GS) - DevOps Tools & Cloud Computing  
**Tema:** "O Futuro do Trabalho"  
**Disciplina:** Advanced Business Development with .NET  
**Instituição:** FIAP

---

**📅 Última atualização:** 2025-11-23  
**✅ Status:** API funcional em produção | CI/CD configurado | Testes automatizados | Swagger habilitado
