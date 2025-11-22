# Guia Completo: Configuração do Azure Boards para GenFit

## 📋 Objetivo
Configurar Azure Boards conforme os requisitos da GS:
- Criar tarefa inicial
- Vincular commits, branches e PRs
- Proteger branch principal com políticas obrigatórias

---

## 🎯 Passo 1: Criar Tarefa Inicial no Azure Boards

### 1.1 Acessar o Azure Boards

1. No Azure DevOps, clique em **"Boards"** no menu lateral
2. Selecione **"Work items"**
3. Clique em **"+ New Work Item"**
4. Escolha **"Task"**

### 1.2 Preencher a Tarefa

**Título:**
```
[GenFit] Implementação inicial da API RESTful com Oracle e CI/CD
```

**Descrição:**
```
Implementar API RESTful em .NET 8 para sistema de gestão de RH e candidatos.

Tarefas incluídas:
- Configuração da infraestrutura Azure (Resource Group, App Service)
- Desenvolvimento dos endpoints CRUD para Jobs e Users
- Integração com Oracle Database e Stored Procedures
- Configuração de pipelines CI/CD (Build e Release)
- Testes automatizados com xUnit
- Documentação da API (Swagger, README com exemplos JSON)

Critérios de aceitação:
- ✅ Todos os endpoints CRUD funcionando
- ✅ Pipeline de Build executando testes e publicando artefatos
- ✅ Pipeline de Release fazendo deploy automático
- ✅ Health Check funcionando
- ✅ Documentação completa no README
```

**Atribuir para:** Você mesmo (ou deixar vazio para auto-atribuir)

**Estado:** `New` ou `Active`

**Esforço (opcional):** `40 horas`

**Tags:** `GenFit`, `API`, `CRUD`, `Oracle`, `CI/CD`

**Salvar:** Clique em **"Save"** ou pressione `Ctrl+S`

### 1.3 Copiar o ID da Tarefa

Após salvar, anote o **ID da Work Item** (ex: `#123`). Você precisará dele para vincular commits e PRs.

**Exemplo:** Se o ID for `#123`, você verá: `[GenFit] Implementação inicial... #123`

---

## 🔗 Passo 2: Vincular Commits, Branches e PRs à Tarefa

### 2.1 Vincular Commit à Tarefa

Ao fazer commits, inclua a referência ao Work Item no início da mensagem:

**Formato:**
```bash
git commit -m "#123 Implementa endpoint GET /api/v1/jobs com paginação"
```

**Exemplos de commits vinculados:**

```bash
# Commit vinculando a tarefa #123
git commit -m "#123 Adiciona estrutura inicial do projeto .NET 8"

git commit -m "#123 Implementa controller JobsController com CRUD completo"

git commit -m "#123 Integra procedure PRC_INSERT_JOB no JobService"

git commit -m "#123 Configura pipeline de Build no Azure DevOps"

git commit -m "#123 Adiciona testes unitários para UsersController"
```

**Importante:** O `#123` deve ser substituído pelo ID real da sua tarefa!

### 2.2 Vincular Branch à Tarefa

Ao criar uma branch para desenvolver a tarefa, inclua o ID no nome:

**Formato:**
```
feature/123-implementacao-api-inicial
```

**Exemplos:**
```bash
# Criar branch vinculada à tarefa #123
git checkout -b feature/123-implementacao-api-inicial

# Ou
git checkout -b feature/123-crud-jobs

# Ou
git checkout -b bugfix/123-corrige-teste-falhando
```

**Padrões sugeridos:**
- `feature/123-descricao` - Para novas funcionalidades
- `bugfix/123-descricao` - Para correções de bugs
- `hotfix/123-descricao` - Para correções urgentes

### 2.3 Vincular Pull Request à Tarefa

Ao criar um Pull Request no Azure DevOps:

1. **Título do PR:**
   ```
   #123: Implementação inicial da API RESTful com Oracle e CI/CD
   ```

2. **Descrição do PR:**
   ```
   ## Resumo
   Implementa API RESTful em .NET 8 para sistema GenFit com integração Oracle e pipelines CI/CD.

   ## Mudanças
   - ✅ Criação da estrutura do projeto (API, Core, Infrastructure, Application, Tests)
   - ✅ Implementação de endpoints CRUD para Jobs e Users
   - ✅ Integração com Oracle Database via Entity Framework Core
   - ✅ Chamada de Stored Procedures (PRC_INSERT_USER, PRC_INSERT_JOB)
   - ✅ Configuração de Health Check, Logging e Tracing
   - ✅ Testes unitários com xUnit
   - ✅ Pipeline de Build e Release no Azure DevOps

   ## Work Items relacionados
   Fixes #123

   ## Checklist
   - [x] Código testado localmente
   - [x] Testes passando
   - [x] Documentação atualizada
   ```

3. **No campo "Work items":**
   - Clique em **"+"** ou digite `#123`
   - Selecione a tarefa que aparece

4. **Verificar vinculação:**
   - Após criar o PR, a tarefa #123 aparecerá automaticamente na aba "Related work items" do Pull Request

---

## 🛡️ Passo 3: Proteger Branch Principal (main/master)

### 3.1 Acessar Configurações da Branch

1. No Azure DevOps, vá em **"Repos"** → **"Branches"**
2. Clique nos **"..."** (três pontos) ao lado da branch `main`
3. Selecione **"Branch policies"**

### 3.2 Configurar Revisor Obrigatório

1. Em **"Branch policies"**, role até **"Require a minimum number of reviewers"**
2. **Habilite** a opção
3. Configure:
   - **Minimum number of reviewers:** `1`
   - **Allow requestors to approve their own changes:** ✅ **SIM** (para simulação, conforme requisito 5)
   - **When new changes are pushed, reset all code reviewer votes:** ✅ **SIM** (opcional)
   
### 3.3 Configurar Revisor Padrão (Seu RM)

1. Ainda em **"Require a minimum number of reviewers"**
2. Clique em **"Add required reviewers"**
3. Selecione seu RM (Revisor Padrão)
   - Digite o nome do RM ou email
   - Exemplo: `rm558515@fiap.edu.br` ou o nome do usuário
4. Selecione o usuário na lista
5. **Marque como required reviewer**

### 3.4 Vincular Work Item Obrigatório

1. Role até **"Check for linked work items"**
2. **Habilite** a opção
3. Configure:
   - **Require links in comment only:** ❌ (desmarcado - requer link no PR)
   - **Required work item types:** Deixe padrão ou selecione `Task`, `User Story`, `Bug`

### 3.5 Configurações Adicionais Recomendadas

#### Build Validation (Opcional mas recomendado)
1. Role até **"Build validation"**
2. Clique em **"+ Build policy"**
3. Configure:
   - **Build pipeline:** Selecione sua pipeline de Build (ex: `genfit-CI`)
   - **Display name:** `Validate Build`
   - **Trigger:** `Automatic (whenever the source branch is updated)`
   - **Policy requirement:** `Required`

#### Status Check (Opcional)
1. Role até **"Status checks"**
2. Clique em **"+ Status policy"**
3. Configure para garantir que testes passem antes do merge

### 3.6 Salvar Políticas

1. Clique em **"Save"** no topo da página
2. As políticas serão aplicadas imediatamente

---

## ✅ Verificação: Como Funciona na Prática

### Fluxo Completo de Trabalho

1. **Criar tarefa no Boards:**
   ```
   [GenFit] Implementar endpoint DELETE /api/v1/jobs #124
   ```

2. **Criar branch vinculada:**
   ```bash
   git checkout -b feature/124-delete-job-endpoint
   ```

3. **Fazer commits vinculados:**
   ```bash
   git commit -m "#124 Implementa método DeleteJobAsync no JobService"
   git commit -m "#124 Adiciona endpoint DELETE no JobsController"
   git commit -m "#124 Adiciona testes para endpoint DELETE"
   ```

4. **Criar Pull Request:**
   - Título: `#124: Implementar endpoint DELETE /api/v1/jobs`
   - Descrição: `Fixes #124`
   - Work Item: Selecionar #124 automaticamente

5. **O PR será bloqueado até:**
   - ✅ Ter pelo menos 1 revisor (você pode aprovar seu próprio PR)
   - ✅ Ter um Work Item vinculado (#124)
   - ✅ Pipeline de Build passar (se configurado)

6. **Após aprovação e merge:**
   - A branch `feature/124-...` será mergeada em `main`
   - O Work Item #124 pode ser marcado como "Done"
   - Os commits aparecerão vinculados na tarefa #124

---

## 📝 Exemplo Prático Completo

### Cenário: Adicionar nova funcionalidade

**1. Criar Tarefa:**
```
Título: [GenFit] Adicionar endpoint GET /api/v1/skills/{id}
ID: #125
Estado: Active
```

**2. Criar Branch:**
```bash
git checkout -b feature/125-get-skill-by-id
```

**3. Desenvolver e Commitar:**
```bash
git add .
git commit -m "#125 Adiciona método GetSkillByIdAsync no SkillService"
git commit -m "#125 Implementa endpoint GET /api/v1/skills/{id}"
git commit -m "#125 Adiciona testes para GetSkillById"
```

**4. Push e Criar PR:**
```bash
git push origin feature/125-get-skill-by-id
```

No Azure DevOps:
- Crie PR: `feature/125-get-skill-by-id` → `main`
- Título: `#125: Adicionar endpoint GET /api/v1/skills/{id}`
- Descrição: `Fixes #125`
- Vincule Work Item: `#125`

**5. Aprovar PR:**
- Você pode aprovar seu próprio PR (simulação)
- Ou o RM aprova

**6. Merge:**
- Após aprovação, faça merge
- A tarefa #125 será automaticamente vinculada ao merge commit

---

## 🔍 Como Verificar se Está Funcionando

### Verificar Vinculação de Commits

1. Vá em **Boards** → **Work items**
2. Clique na tarefa #123 (ou qualquer uma)
3. Aba **"Development"** mostra:
   - ✅ Commits vinculados
   - ✅ Branches vinculadas
   - ✅ Pull Requests vinculados

### Verificar Políticas da Branch

1. Vá em **Repos** → **Branches**
2. Clique em **"..."** → **"Branch policies"** na branch `main`
3. Verifique se todas as políticas estão habilitadas:
   - ✅ Require a minimum number of reviewers
   - ✅ Check for linked work items
   - ✅ Required reviewers incluem seu RM

---

## ⚠️ Dicas Importantes

1. **Sempre inclua o ID da tarefa nos commits:**
   - ✅ Correto: `#123 Adiciona feature X`
   - ❌ Errado: `Adiciona feature X` (sem ID)

2. **Use padrão no nome das branches:**
   - ✅ `feature/123-descricao`
   - ✅ `bugfix/123-descricao`
   - ❌ `nova-feature` (sem ID)

3. **Sempre vincule Work Item no PR:**
   - Digite `#123` na descrição ou selecione no campo "Work items"

4. **Verifique as políticas antes de criar PR:**
   - Se o PR for bloqueado, verifique se:
     - Tem Work Item vinculado
     - Tem revisor atribuído
     - Build passou (se configurado)

---

## 📞 Próximos Passos

Após configurar tudo:

1. ✅ Criar primeira tarefa no Boards
2. ✅ Criar branch vinculada
3. ✅ Fazer alguns commits de teste
4. ✅ Criar PR de teste
5. ✅ Verificar se as políticas estão funcionando
6. ✅ Documentar o ID da tarefa principal para referência no vídeo

**ID da Tarefa Principal:** `#XXX` (substitua pelo ID real que você criar)

