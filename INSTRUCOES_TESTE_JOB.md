# Instruções para Testar Criação de Job

## Problema Resolvido ✅

O JobsController estava apenas com TODOs e não implementado. Agora está totalmente funcional!

## O que foi implementado:

1. ✅ **IJobService** - Interface do serviço de Jobs
2. ✅ **JobService** - Implementação completa do serviço com:
   - Listagem com paginação e HATEOAS
   - Obter job por ID
   - Criar job via procedure Oracle `PRC_INSERT_JOB`
   - Atualizar job existente
3. ✅ **JobsController** - Controller completamente implementado
4. ✅ **Registro do serviço** - JobService registrado no Program.cs

## Como Testar no Swagger:

1. **Inicie a API:**
   ```bash
   cd GenFit.API
   dotnet run
   ```

2. **Acesse o Swagger:**
   ```
   http://localhost:5118/swagger
   ```

3. **Teste Criar Job (POST /api/v1/jobs):**
   - Clique no endpoint `POST /api/v1/jobs`
   - Clique em "Try it out"
   - Preencha o JSON de exemplo:
   ```json
   {
     "titulo": "Desenvolvedor .NET Senior",
     "descricao": "Vaga para desenvolvedor .NET com experiência em APIs RESTful",
     "salario": 12000.00,
     "localizacao": "São Paulo, SP",
     "tipoContrato": "CLT",
     "nivel": "Senior",
     "modeloTrabalho": "Híbrido",
     "departamento": "Tecnologia"
   }
   ```
   - Clique em "Execute"
   - Deve retornar status 201 Created com o job criado

4. **Teste Listar Jobs (GET /api/v1/jobs):**
   - Clique no endpoint `GET /api/v1/jobs`
   - Clique em "Try it out"
   - Execute
   - Deve retornar a lista com paginação e links HATEOAS

5. **Teste Obter Job por ID (GET /api/v1/jobs/{id}):**
   - Use o ID retornado na criação
   - Deve retornar o job específico

## Campos do CreateJobDto:

- `titulo` (string, obrigatório) - Título da vaga
- `descricao` (string, opcional) - Descrição da vaga
- `salario` (decimal, opcional) - Salário oferecido
- `localizacao` (string, opcional) - Localização da vaga
- `tipoContrato` (string, opcional) - Tipo de contrato (CLT, PJ, etc.)
- `nivel` (string, opcional) - Nível da vaga (Júnior, Pleno, Senior)
- `modeloTrabalho` (string, opcional) - Modelo de trabalho (Presencial, Remoto, Híbrido)
- `departamento` (string, opcional) - Departamento

## Importante:

- A criação de job usa a procedure Oracle `PRC_INSERT_JOB`
- O endpoint está em `/api/v1/jobs` (versionado)
- Retorna status 201 Created quando bem-sucedido
- Inclui links HATEOAS na paginação
- Logs de erro são registrados para debug

## Se ainda houver problemas:

1. Verifique se a API está rodando
2. Verifique a connection string do Oracle no appsettings.json
3. Verifique os logs da aplicação para erros
4. Certifique-se de que a procedure `PRC_INSERT_JOB` existe no banco Oracle

