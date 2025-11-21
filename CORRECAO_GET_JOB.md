# Correção do Erro 500 no GET /api/v1/jobs/{id}

## Problema Identificado

O erro 500 estava ocorrendo ao buscar um job por ID. O problema estava relacionado ao uso de `FindAsync` com Oracle, que pode ter problemas de conversão de tipos.

## Correções Aplicadas

### 1. Método GetJobByIdAsync melhorado

**Antes:**
```csharp
var job = await _context.Jobs.FindAsync(id);
```

**Depois:**
```csharp
var job = await _context.Jobs
    .Where(j => j.Id == id)
    .Select(j => new JobDto { ... })
    .FirstOrDefaultAsync();
```

### 2. Configuração do ID no Entity Framework

Adicionada configuração explícita do tipo NUMBER para o ID:

```csharp
builder.Property(j => j.Id)
    .HasColumnName("id")
    .HasColumnType("NUMBER(10)")
    .ValueGeneratedNever();
```

### 3. Melhor tratamento de erros

- Logs mais detalhados no controller
- Mensagens de erro mais informativas
- Tratamento de exceções melhorado

## Como Testar

1. **Reinicie a API:**
   ```bash
   cd GenFit.API
   dotnet run
   ```

2. **Teste no Swagger:**
   - Acesse `http://localhost:5118/swagger`
   - Vá em `GET /api/v1/jobs/{id}`
   - Use um ID válido (ex: 6)
   - Execute

3. **Verifique os logs:**
   - Se ainda houver erro, verifique os logs da aplicação
   - Os logs agora mostram mensagens mais detalhadas

## Possíveis Causas Adicionais

Se o erro persistir, pode ser:

1. **Problema de conexão com Oracle:**
   - Verifique se o Oracle está acessível
   - Teste o endpoint `/health` para verificar a conexão

2. **Job não existe:**
   - Verifique se o ID 6 realmente existe no banco
   - Teste com outro ID

3. **Problema de permissões:**
   - Verifique se o usuário tem permissão para ler a tabela JOBS

## Próximos Passos

Se o erro continuar:
1. Verifique os logs da aplicação em `logs/genfit-*.txt`
2. Teste a conexão com Oracle diretamente
3. Verifique se a tabela JOBS existe e tem dados
4. Teste com um ID que você sabe que existe

