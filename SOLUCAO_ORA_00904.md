# Solução do Erro ORA-00904: "j"."id": identificador inválido

## Problema

O erro `ORA-00904: "j"."id": identificador inválido` indica que o Oracle não está encontrando a coluna "id" no formato em que o Entity Framework está tentando acessá-la.

## Causa

No Oracle, os nomes de colunas são case-sensitive. Quando você cria uma tabela:

- **Sem aspas duplas:** Oracle converte tudo para MAIÚSCULAS (ID, TITULO, etc.)
- **Com aspas duplas:** Oracle preserva o case exato (id, titulo, etc.)

O schema fornecido mostra que as tabelas foram criadas em MAIÚSCULAS (JOBS, USERS, etc.), então as colunas provavelmente também estão em MAIÚSCULAS no banco.

## Solução Aplicada

Atualizei a configuração do Entity Framework para usar nomes de colunas em **MAIÚSCULAS**, que é o padrão do Oracle quando as tabelas são criadas sem aspas duplas:

```csharp
builder.Property(j => j.Id)
    .HasColumnName("ID")  // Maiúscula ao invés de "id"
    .HasColumnType("NUMBER(10)")
    .ValueGeneratedNever();
```

Todas as colunas foram atualizadas:
- `id` → `ID`
- `titulo` → `TITULO`
- `descricao` → `DESCRICAO`
- `salario` → `SALARIO`
- etc.

## Se o Erro Persistir

Se ainda houver erro, pode ser que as colunas estejam realmente em minúsculas no banco (criadas com aspas). Nesse caso:

1. **Verifique as colunas no banco:**
   ```sql
   SELECT COLUMN_NAME 
   FROM USER_TAB_COLUMNS 
   WHERE TABLE_NAME = 'JOBS';
   ```

2. **Ajuste a configuração conforme necessário:**
   - Se estiverem em MAIÚSCULAS: use `"ID"`, `"TITULO"`, etc.
   - Se estiverem em minúsculas: use `"id"`, `"titulo"`, etc.

3. **Alternativa:** Use aspas duplas na configuração se as colunas foram criadas com aspas:
   ```csharp
   .HasColumnName("\"id\"")
   ```

## Teste

Após reiniciar a API, teste:
```
GET http://localhost:5118/api/v1/jobs/6
```

O erro deve estar resolvido se os nomes das colunas no banco corresponderem à configuração.

