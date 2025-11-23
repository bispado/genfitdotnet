-- ============================================================
-- Script de Criação do Banco de Dados Azure SQL - GenFit
-- Sistema de Gestão de RH e Candidatos - O Futuro do Trabalho
-- ============================================================

-- ============================================================
-- TABELA: AZ_JOBS
-- Vagas de emprego
-- ============================================================
CREATE TABLE AZ_JOBS (
    id INT PRIMARY KEY IDENTITY(1,1),
    titulo NVARCHAR(200) NOT NULL,
    descricao NVARCHAR(MAX),
    salario DECIMAL(10,2),
    localizacao NVARCHAR(100),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- ============================================================
-- TABELA: AZ_COMPANIES
-- Empresas
-- ============================================================
CREATE TABLE AZ_COMPANIES (
    id INT PRIMARY KEY IDENTITY(1,1),
    nome NVARCHAR(200) NOT NULL,
    cnpj NVARCHAR(18),
    setor NVARCHAR(100),
    created_at DATETIME2 DEFAULT GETDATE()
);

-- ============================================================
-- TABELA: AZ_SKILLS
-- Skills/Competências
-- ============================================================
CREATE TABLE AZ_SKILLS (
    id INT PRIMARY KEY IDENTITY(1,1),
    nome NVARCHAR(150) NOT NULL,
    categoria NVARCHAR(100),
    created_at DATETIME2 DEFAULT GETDATE()
);

-- ============================================================
-- TABELA: AZ_APPLICATIONS
-- Candidaturas
-- ============================================================
CREATE TABLE AZ_APPLICATIONS (
    id INT PRIMARY KEY IDENTITY(1,1),
    job_id INT NOT NULL,
    candidate_name NVARCHAR(200) NOT NULL,
    candidate_email NVARCHAR(200) NOT NULL,
    status NVARCHAR(50) DEFAULT 'Pendente',
    created_at DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_APPLICATIONS_JOBS FOREIGN KEY (job_id) REFERENCES AZ_JOBS(id) ON DELETE CASCADE
);

-- ============================================================
-- ÍNDICES
-- ============================================================
CREATE INDEX IX_JOBS_TITULO ON AZ_JOBS(titulo);
CREATE INDEX IX_JOBS_LOCALIZACAO ON AZ_JOBS(localizacao);
CREATE INDEX IX_COMPANIES_NOME ON AZ_COMPANIES(nome);
CREATE INDEX IX_SKILLS_NOME ON AZ_SKILLS(nome);
CREATE INDEX IX_APPLICATIONS_JOB_ID ON AZ_APPLICATIONS(job_id);
CREATE INDEX IX_APPLICATIONS_STATUS ON AZ_APPLICATIONS(status);

-- ============================================================
-- TRIGGER para atualizar updated_at em AZ_JOBS
-- ============================================================
CREATE TRIGGER TRG_AZ_JOBS_UPDATE
ON AZ_JOBS
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE AZ_JOBS
    SET updated_at = GETDATE()
    FROM AZ_JOBS j
    INNER JOIN inserted i ON j.id = i.id;
END;

