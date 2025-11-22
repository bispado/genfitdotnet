# Exemplo de Work Item para Azure Boards - GenFit

## 📋 Tarefa Inicial Recomendada

### Tipo: Task
### Título:
```
[GenFit] Implementação completa da API RESTful .NET 8 com Oracle Database e CI/CD
```

### Descrição Completa:

```markdown
## Objetivo
Implementar API RESTful completa em .NET 8 para o sistema GenFit de gestão de RH e candidatos, seguindo os requisitos da GS de DevOps Tools & Cloud Computing.

## Escopo da Implementação

### 1. Infraestrutura Azure
- [x] Script de provisionamento (script-infra-app.sh)
- [x] Resource Group
- [x] App Service Plan
- [x] App Service (api-genfit-rm558515)
- [x] App Settings configurados

### 2. Desenvolvimento da API
- [x] Estrutura do projeto (API, Core, Infrastructure, Application, Tests)
- [x] Entity Framework Core com Oracle
- [x] Controllers versionados (/api/v1/)
- [x] CRUD completo para Jobs (GET, POST, PUT, DELETE)
- [x] CRUD completo para Users (GET, POST, PUT, DELETE)
- [x] Integração com Stored Procedures Oracle
  - PRC_INSERT_USER
  - PRC_INSERT_JOB
  - PRC_INSERT_CANDIDATE_SKILL
  - PRC_INSERT_MODEL_RESULT

### 3. Funcionalidades Implementadas
- [x] Paginação e HATEOAS em todos os endpoints
- [x] Health Check customizado (Oracle Database)
- [x] Logging estruturado (Serilog)
- [x] Tracing distribuído (OpenTelemetry)
- [x] Autenticação via API Key
- [x] Versionamento de API (/api/v1/)

### 4. Testes
- [x] Testes unitários com xUnit
- [x] Testes de controllers (UsersControllerTests)
- [x] Publicação de resultados na pipeline

### 5. CI/CD
- [x] Pipeline de Build (Classic)
- [x] Pipeline de Release (Classic)
- [x] Pipeline YAML (azure-pipeline.yml)
- [x] Deploy automático no Azure App Service

### 6. Documentação
- [x] README completo com exemplos CRUD JSON
- [x] Script SQL do banco (script-bd.sql)
- [x] Swagger/OpenAPI configurado

## Critérios de Aceitação

✅ Todos os endpoints CRUD funcionando corretamente
✅ Testes automatizados passando na pipeline
✅ Deploy automático funcionando
✅ Health Check retornando status Healthy
✅ Documentação completa no README
✅ Scripts de infraestrutura funcionando
✅ Pipeline publicando artefatos e testes

## Recursos Relacionados

- Repositório: https://dev.azure.com/motosync/genfit/_git/genfit
- API em Produção: https://api-genfit-rm558515.azurewebsites.net
- Swagger: https://api-genfit-rm558515.azurewebsites.net/swagger

## Notas
Projeto desenvolvido para GS de DevOps Tools & Cloud Computing - FIAP
```

### Campos Adicionais:

**Área:** `GenFit\API`
**Iteration:** `Sprint 1` (ou deixar padrão)

**Tags:**
- `GenFit`
- `API`
- `CRUD`
- `Oracle`
- `CI/CD`
- `Azure`
- `DevOps`

**Esforço:** `40 horas`

**Prioridade:** `High`

---

## 🔗 Exemplos de Commits Vinculados

```bash
# Após criar a tarefa, supondo que o ID seja #100:

git commit -m "#100 Cria estrutura inicial do projeto .NET 8"
git commit -m "#100 Adiciona Entity Framework Core com provider Oracle"
git commit -m "#100 Implementa JobsController com CRUD completo"
git commit -m "#100 Integra procedure PRC_INSERT_JOB no JobService"
git commit -m "#100 Adiciona testes unitários para UsersController"
git commit -m "#100 Configura pipeline de Build no Azure DevOps"
git commit -m "#100 Cria pipeline de Release para deploy automático"
git commit -m "#100 Adiciona exemplos CRUD JSON no README"
```

---

## 🌿 Exemplo de Branch Vinculada

```bash
# Criar branch vinculada (ID da tarefa: #100)
git checkout -b feature/100-implementacao-inicial-api

# Desenvolver e commitar
git commit -m "#100 Implementa endpoint GET /api/v1/jobs"
git commit -m "#100 Adiciona paginação e HATEOAS"
git commit -m "#100 Configura Health Check para Oracle"

# Push e criar PR
git push origin feature/100-implementacao-inicial-api
```

---

## 🔀 Exemplo de Pull Request

**Título:**
```
#100: Implementação inicial da API RESTful com Oracle e CI/CD
```

**Descrição:**
```markdown
## Resumo
Implementa API RESTful completa em .NET 8 para sistema GenFit conforme requisitos da GS.

## Mudanças Principais
- ✅ Estrutura do projeto com arquitetura em camadas
- ✅ CRUD completo para Jobs e Users
- ✅ Integração com Oracle Database via EF Core
- ✅ Chamada de Stored Procedures Oracle
- ✅ Pipelines CI/CD configuradas
- ✅ Testes automatizados
- ✅ Documentação completa

## Work Items
Fixes #100

## Checklist
- [x] Código testado localmente
- [x] Testes passando (5 testes)
- [x] Pipeline de Build funcionando
- [x] Pipeline de Release funcionando
- [x] Deploy realizado com sucesso
- [x] Documentação atualizada
- [x] Health Check funcionando
```

---

## 📊 Exemplo de Outras Tarefas (para dividir o trabalho)

### Tarefa #101: Configuração de Infraestrutura
```
[GenFit] Configurar infraestrutura Azure via scripts CLI
```

### Tarefa #102: CRUD Jobs
```
[GenFit] Implementar CRUD completo para Jobs
```

### Tarefa #103: CRUD Users
```
[GenFit] Implementar CRUD completo para Users
```

### Tarefa #104: Integração Oracle Procedures
```
[GenFit] Integrar Stored Procedures Oracle (PRC_INSERT_USER, PRC_INSERT_JOB)
```

### Tarefa #105: Pipelines CI/CD
```
[GenFit] Configurar pipelines de Build e Release no Azure DevOps
```

### Tarefa #106: Testes Automatizados
```
[GenFit] Implementar testes unitários e de integração com xUnit
```

### Tarefa #107: Documentação
```
[GenFit] Documentar API com README, exemplos JSON e Swagger
```

