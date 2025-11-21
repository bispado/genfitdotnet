-- Script para verificar os nomes exatos das colunas no Oracle
-- Execute este script no Oracle para descobrir o case exato das colunas

-- Verificar colunas da tabela JOBS
SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH
FROM USER_TAB_COLUMNS 
WHERE TABLE_NAME = 'JOBS'
ORDER BY COLUMN_ID;

-- Verificar colunas da tabela USERS
SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH
FROM USER_TAB_COLUMNS 
WHERE TABLE_NAME = 'USERS'
ORDER BY COLUMN_ID;

-- Testar query direta (ajuste conforme o case das colunas)
-- Se as colunas estão em MAIÚSCULAS:
SELECT * FROM JOBS WHERE ID = 6;

-- Se as colunas estão em minúsculas:
-- SELECT * FROM JOBS WHERE "id" = 6;
