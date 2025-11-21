# Instruções para Corrigir o Erro ORA-00904 (Case Sensitivity)

## Problema Atual

O erro `ORA-00904: "j"."id": identificador inválido` indica que o Oracle não está encontrando a coluna com o nome especificado.

## Diagnóstico

No Oracle, os nomes de identificadores (tabelas, colunas) são case-sensitive quando criados com aspas duplas. Quando criados sem aspas, são convertidos para MAIÚSCULAS.

## Como Verificar

Execute no Oracle SQL Developer ou cliente SQL:

```sql
SELECT COLUMN_NAME 
FROM USER_TAB_COLUMNS 
WHERE TABLE_NAME = 'JOBS'
ORDER BY COLUMN_ID;
```

Isso mostrará os nomes **exatos** das colunas no banco.

## Correções Possíveis

### Opção 1: Se as colunas estão em MAIÚSCULAS (ID, TITULO, etc.)

Atualize a configuração para usar maiúsculas (já aplicado):

```csharp
builder.Property(j => j.Id)
    .HasColumnName("ID")  // Maiúscula
```

### Opção 2: Se as colunas estão em minúsculas (id, titulo, etc.)

Use aspas duplas na configuração:

```csharp
builder.Property(j => j.Id)
    .HasColumnName("\"id\"")  // Com aspas duplas
```

### Opção 3: Configurar o EF Core para usar aspas duplas

No `Program.cs`, configure o provider Oracle:

```csharp
builder.Services.AddDbContext<GenFitDbContext>(options =>
    options.UseOracle(connectionString, o => 
        o.MigrationsHistoryTable("__EFMigrationsHistory")));
```

## Status Atual

A configuração está usando **MAIÚSCULAS** (ID, TITULO, DESCRICAO, etc.) porque:
1. As tabelas estão em MAIÚSCULAS (JOBS, USERS)
2. No Oracle, quando criadas sem aspas, tudo vira maiúscula
3. Este é o padrão mais comum

## Próximo Passo

**Reinicie a API e teste novamente.** Se ainda houver erro, execute a query SQL acima para verificar o case exato das colunas e ajuste a configuração conforme necessário.

