-- ============================================================
-- Script de Criação do Banco de Dados GenFit
-- Sistema de Gestão de RH e Candidatos - O Futuro do Trabalho
-- ============================================================

-- ============================================================
-- SEQUENCES
-- ============================================================

-- Sequence para USERS
CREATE SEQUENCE SEQ_USERS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para SKILLS
CREATE SEQUENCE SEQ_SKILLS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para JOBS
CREATE SEQUENCE SEQ_JOBS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para COURSES
CREATE SEQUENCE SEQ_COURSES
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para CANDIDATE_SKILLS
CREATE SEQUENCE SEQ_CANDIDATE_SKILLS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para JOB_SKILLS
CREATE SEQUENCE SEQ_JOB_SKILLS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para QUESTIONNAIRE_QUESTIONS
CREATE SEQUENCE SEQ_QUESTIONNAIRE_QUESTIONS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para QUESTIONNAIRE_ANSWERS
CREATE SEQUENCE SEQ_QUESTIONNAIRE_ANSWERS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para MODEL_RESULTS
CREATE SEQUENCE SEQ_MODEL_RESULTS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Sequence para AUDIT_LOGS
CREATE SEQUENCE SEQ_AUDIT_LOGS
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- ============================================================
-- Tabela: USERS
-- Armazena candidatos e funcionários
-- ============================================================
CREATE TABLE USERS (
    id NUMBER NOT NULL,
    role VARCHAR2(20) NOT NULL,
    nome VARCHAR2(150) NOT NULL,
    email VARCHAR2(150) NOT NULL,
    senha_hash VARCHAR2(255),
    cpf VARCHAR2(14),
    telefone VARCHAR2(20),
    data_nascimento DATE,
    linkedin_url VARCHAR2(255),
    created_at DATE DEFAULT SYSDATE,
    updated_at DATE DEFAULT SYSDATE,
    CONSTRAINT PK_USERS PRIMARY KEY (id),
    CONSTRAINT UK_USERS_EMAIL UNIQUE (email)
);

-- ============================================================
-- Tabela: SKILLS
-- Catálogo de competências do futuro do trabalho
-- ============================================================
CREATE TABLE SKILLS (
    id NUMBER NOT NULL,
    codigo VARCHAR2(10) NOT NULL,
    nome VARCHAR2(150) NOT NULL,
    categoria VARCHAR2(100),
    descricao VARCHAR2(500),
    created_at DATE DEFAULT SYSDATE,
    CONSTRAINT PK_SKILLS PRIMARY KEY (id),
    CONSTRAINT UK_SKILLS_CODIGO UNIQUE (codigo)
);

-- ============================================================
-- Tabela: JOBS
-- Vagas de emprego disponíveis
-- ============================================================
CREATE TABLE JOBS (
    id NUMBER NOT NULL,
    titulo VARCHAR2(150) NOT NULL,
    descricao CLOB,
    salario NUMBER(10,2),
    localizacao VARCHAR2(150),
    tipo_contrato VARCHAR2(50),
    nivel VARCHAR2(50),
    modelo_trabalho VARCHAR2(50),
    departamento VARCHAR2(100),
    created_at DATE DEFAULT SYSDATE,
    updated_at DATE DEFAULT SYSDATE,
    CONSTRAINT PK_JOBS PRIMARY KEY (id)
);

-- ============================================================
-- Tabela: COURSES
-- Cursos de requalificação e reskilling
-- ============================================================
CREATE TABLE COURSES (
    id NUMBER NOT NULL,
    nome VARCHAR2(200) NOT NULL,
    descricao VARCHAR2(500),
    categoria VARCHAR2(100),
    duracao_horas NUMBER,
    nivel VARCHAR2(50),
    created_at DATE DEFAULT SYSDATE,
    CONSTRAINT PK_COURSES PRIMARY KEY (id)
);

-- ============================================================
-- Tabela: CANDIDATE_SKILLS
-- Skills dos candidatos
-- ============================================================
CREATE TABLE CANDIDATE_SKILLS (
    id NUMBER NOT NULL,
    user_id NUMBER NOT NULL,
    skill_id NUMBER NOT NULL,
    nivel_proficiencia NUMBER(3,2),
    data_aquisicao DATE DEFAULT SYSDATE,
    CONSTRAINT PK_CANDIDATE_SKILLS PRIMARY KEY (id),
    CONSTRAINT FK_CANDIDATE_SKILLS_USER FOREIGN KEY (user_id) REFERENCES USERS(id),
    CONSTRAINT FK_CANDIDATE_SKILLS_SKILL FOREIGN KEY (skill_id) REFERENCES SKILLS(id),
    CONSTRAINT UK_CANDIDATE_SKILLS UNIQUE (user_id, skill_id)
);

-- ============================================================
-- Tabela: JOB_SKILLS
-- Skills exigidas pelas vagas
-- ============================================================
CREATE TABLE JOB_SKILLS (
    id NUMBER NOT NULL,
    job_id NUMBER NOT NULL,
    skill_id NUMBER NOT NULL,
    obrigatoria CHAR(1) DEFAULT 'S',
    peso NUMBER(3,2) DEFAULT 1.0,
    CONSTRAINT PK_JOB_SKILLS PRIMARY KEY (id),
    CONSTRAINT FK_JOB_SKILLS_JOB FOREIGN KEY (job_id) REFERENCES JOBS(id),
    CONSTRAINT FK_JOB_SKILLS_SKILL FOREIGN KEY (skill_id) REFERENCES SKILLS(id),
    CONSTRAINT UK_JOB_SKILLS UNIQUE (job_id, skill_id)
);

-- ============================================================
-- Tabela: QUESTIONNAIRE_QUESTIONS
-- Perguntas do questionário cultural
-- ============================================================
CREATE TABLE QUESTIONNAIRE_QUESTIONS (
    id NUMBER NOT NULL,
    pergunta_texto VARCHAR2(500) NOT NULL,
    tipo VARCHAR2(30) NOT NULL,
    escala_min NUMBER,
    escala_max NUMBER,
    opcoes VARCHAR2(500),
    categoria VARCHAR2(100),
    created_at DATE DEFAULT SYSDATE,
    CONSTRAINT PK_QUESTIONNAIRE_QUESTIONS PRIMARY KEY (id)
);

-- ============================================================
-- Tabela: QUESTIONNAIRE_ANSWERS
-- Respostas dos candidatos ao questionário
-- ============================================================
CREATE TABLE QUESTIONNAIRE_ANSWERS (
    id NUMBER NOT NULL,
    user_id NUMBER NOT NULL,
    question_id NUMBER NOT NULL,
    resposta_likert NUMBER,
    resposta_texto VARCHAR2(1000),
    resposta_opcao VARCHAR2(200),
    created_at DATE DEFAULT SYSDATE,
    CONSTRAINT PK_QUESTIONNAIRE_ANSWERS PRIMARY KEY (id),
    CONSTRAINT FK_QUESTIONNAIRE_ANSWERS_USER FOREIGN KEY (user_id) REFERENCES USERS(id),
    CONSTRAINT FK_QUESTIONNAIRE_ANSWERS_QUESTION FOREIGN KEY (question_id) REFERENCES QUESTIONNAIRE_QUESTIONS(id)
);

-- ============================================================
-- Tabela: MODEL_RESULTS
-- Resultados da análise de IA (afinidade cultural e red flags)
-- ============================================================
CREATE TABLE MODEL_RESULTS (
    id NUMBER NOT NULL,
    user_id NUMBER NOT NULL,
    job_id NUMBER NOT NULL,
    score_afinidade_cultural NUMBER(5,2),
    score_compatibilidade_profissional NUMBER(5,2),
    red_flags VARCHAR2(1000),
    recomendacao VARCHAR2(50),
    detalhes CLOB,
    created_at DATE DEFAULT SYSDATE,
    CONSTRAINT PK_MODEL_RESULTS PRIMARY KEY (id),
    CONSTRAINT FK_MODEL_RESULTS_USER FOREIGN KEY (user_id) REFERENCES USERS(id),
    CONSTRAINT FK_MODEL_RESULTS_JOB FOREIGN KEY (job_id) REFERENCES JOBS(id)
);

-- ============================================================
-- Tabela: AUDIT_LOGS
-- Logs de auditoria universal (preenchida por triggers)
-- ============================================================
CREATE TABLE AUDIT_LOGS (
    id NUMBER NOT NULL,
    tabela_afetada VARCHAR2(50) NOT NULL,
    registro_id NUMBER NOT NULL,
    operacao VARCHAR2(10) NOT NULL,
    usuario_banco VARCHAR2(100),
    data_hora DATE DEFAULT SYSDATE,
    dados_antigos CLOB,
    dados_novos CLOB,
    CONSTRAINT PK_AUDIT_LOGS PRIMARY KEY (id)
);

-- ============================================================
-- PROCEDURE: PRC_INSERT_USER
-- Insere um novo usuário (candidato ou funcionário)
-- Valida: email, CPF e role
-- ============================================================
CREATE OR REPLACE PROCEDURE PRC_INSERT_USER (
    p_nome IN VARCHAR2,
    p_email IN VARCHAR2,
    p_role IN VARCHAR2,
    p_senha_hash IN VARCHAR2 DEFAULT NULL,
    p_cpf IN VARCHAR2 DEFAULT NULL,
    p_telefone IN VARCHAR2 DEFAULT NULL,
    p_data_nascimento IN DATE DEFAULT NULL,
    p_linkedin_url IN VARCHAR2 DEFAULT NULL,
    p_user_id OUT NUMBER
) AS
    v_email_validado BOOLEAN := FALSE;
    v_cpf_validado BOOLEAN := FALSE;
BEGIN
    -- Validação de campos obrigatórios
    IF p_nome IS NULL OR LENGTH(TRIM(p_nome)) = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'Erro: Nome é obrigatório.');
    END IF;
    
    IF p_email IS NULL OR LENGTH(TRIM(p_email)) = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Erro: Email é obrigatório.');
    END IF;
    
    IF p_role IS NULL OR LENGTH(TRIM(p_role)) = 0 THEN
        RAISE_APPLICATION_ERROR(-20003, 'Erro: Role é obrigatória (candidate ou employee).');
    END IF;
    
    -- Validação de role
    IF p_role NOT IN ('candidate', 'employee') THEN
        RAISE_APPLICATION_ERROR(-20004, 'Erro: Role inválida. Use "candidate" ou "employee".');
    END IF;
    
    -- Validação de email com REGEXP
    IF REGEXP_LIKE(p_email, '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$') THEN
        v_email_validado := TRUE;
    ELSE
        RAISE_APPLICATION_ERROR(-20005, 'Erro: Email com formato inválido.');
    END IF;
    
    -- Validação de CPF com REGEXP (aceita com ou sem pontuação)
    IF p_cpf IS NOT NULL THEN
        IF REGEXP_LIKE(p_cpf, '^[0-9]{3}\.[0-9]{3}\.[0-9]{3}-[0-9]{2}$') OR 
           REGEXP_LIKE(p_cpf, '^[0-9]{11}$') THEN
            v_cpf_validado := TRUE;
        ELSE
            RAISE_APPLICATION_ERROR(-20006, 'Erro: CPF com formato inválido. Use XXX.XXX.XXX-XX ou 11 dígitos.');
        END IF;
    END IF;
    
    -- Verificar se email já existe
    DECLARE
        v_email_count NUMBER := 0;
    BEGIN
        SELECT COUNT(*) INTO v_email_count
        FROM USERS
        WHERE UPPER(email) = UPPER(p_email);
        
        IF v_email_count > 0 THEN
            RAISE_APPLICATION_ERROR(-20007, 'Erro: Email já cadastrado no sistema.');
        END IF;
    END;
    
    -- Inserir usuário
    p_user_id := SEQ_USERS.NEXTVAL;
    
    INSERT INTO USERS (
        id, role, nome, email, senha_hash, cpf, telefone, 
        data_nascimento, linkedin_url, created_at, updated_at
    ) VALUES (
        p_user_id, p_role, p_nome, p_email, p_senha_hash, p_cpf, 
        p_telefone, p_data_nascimento, p_linkedin_url, SYSDATE, SYSDATE
    );
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Usuário inserido com sucesso! ID: ' || p_user_id);
    
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END PRC_INSERT_USER;
/

-- ============================================================
-- PROCEDURE: PRC_INSERT_JOB
-- Insere uma nova vaga de emprego
-- ============================================================
CREATE OR REPLACE PROCEDURE PRC_INSERT_JOB (
    p_titulo IN VARCHAR2,
    p_descricao IN CLOB DEFAULT NULL,
    p_salario IN NUMBER DEFAULT NULL,
    p_localizacao IN VARCHAR2 DEFAULT NULL,
    p_tipo_contrato IN VARCHAR2 DEFAULT NULL,
    p_nivel IN VARCHAR2 DEFAULT NULL,
    p_modelo_trabalho IN VARCHAR2 DEFAULT NULL,
    p_departamento IN VARCHAR2 DEFAULT NULL,
    p_job_id OUT NUMBER
) AS
BEGIN
    -- Validação de campos obrigatórios
    IF p_titulo IS NULL OR LENGTH(TRIM(p_titulo)) = 0 THEN
        RAISE_APPLICATION_ERROR(-20011, 'Erro: Título da vaga é obrigatório.');
    END IF;
    
    -- Inserir vaga
    p_job_id := SEQ_JOBS.NEXTVAL;
    
    INSERT INTO JOBS (
        id, titulo, descricao, salario, localizacao, tipo_contrato, 
        nivel, modelo_trabalho, departamento, created_at, updated_at
    ) VALUES (
        p_job_id, p_titulo, p_descricao, p_salario, p_localizacao, 
        p_tipo_contrato, p_nivel, p_modelo_trabalho, p_departamento, SYSDATE, SYSDATE
    );
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Vaga inserida com sucesso! ID: ' || p_job_id);
    
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END PRC_INSERT_JOB;
/

-- ============================================================
-- PROCEDURE: PRC_INSERT_CANDIDATE_SKILL
-- Adiciona uma skill a um candidato
-- ============================================================
CREATE OR REPLACE PROCEDURE PRC_INSERT_CANDIDATE_SKILL (
    p_user_id IN NUMBER,
    p_skill_id IN NUMBER,
    p_nivel_proficiencia IN NUMBER DEFAULT NULL,
    p_candidate_skill_id OUT NUMBER
) AS
    v_user_exists NUMBER := 0;
    v_skill_exists NUMBER := 0;
    v_user_role VARCHAR2(20);
BEGIN
    -- Verificar se usuário existe e é candidato
    SELECT COUNT(*), MAX(role) INTO v_user_exists, v_user_role
    FROM USERS
    WHERE id = p_user_id;
    
    IF v_user_exists = 0 THEN
        RAISE_APPLICATION_ERROR(-20021, 'Erro: Usuário não encontrado.');
    END IF;
    
    IF v_user_role != 'candidate' THEN
        RAISE_APPLICATION_ERROR(-20022, 'Erro: Usuário deve ser um candidato.');
    END IF;
    
    -- Verificar se skill existe
    SELECT COUNT(*) INTO v_skill_exists
    FROM SKILLS
    WHERE id = p_skill_id;
    
    IF v_skill_exists = 0 THEN
        RAISE_APPLICATION_ERROR(-20023, 'Erro: Skill não encontrada.');
    END IF;
    
    -- Verificar se candidato já possui essa skill
    DECLARE
        v_skill_duplicada NUMBER := 0;
    BEGIN
        SELECT COUNT(*) INTO v_skill_duplicada
        FROM CANDIDATE_SKILLS
        WHERE user_id = p_user_id AND skill_id = p_skill_id;
        
        IF v_skill_duplicada > 0 THEN
            RAISE_APPLICATION_ERROR(-20024, 'Erro: Candidato já possui esta skill.');
        END IF;
    END;
    
    -- Validação de nível de proficiência (0 a 1.0)
    IF p_nivel_proficiencia IS NOT NULL THEN
        IF p_nivel_proficiencia < 0 OR p_nivel_proficiencia > 1 THEN
            RAISE_APPLICATION_ERROR(-20025, 'Erro: Nível de proficiência deve estar entre 0 e 1.');
        END IF;
    END IF;
    
    -- Inserir skill do candidato
    p_candidate_skill_id := SEQ_CANDIDATE_SKILLS.NEXTVAL;
    
    INSERT INTO CANDIDATE_SKILLS (
        id, user_id, skill_id, nivel_proficiencia, data_aquisicao
    ) VALUES (
        p_candidate_skill_id, p_user_id, p_skill_id, p_nivel_proficiencia, SYSDATE
    );
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Skill adicionada ao candidato com sucesso! ID: ' || p_candidate_skill_id);
    
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END PRC_INSERT_CANDIDATE_SKILL;
/

-- ============================================================
-- PROCEDURE: PRC_INSERT_MODEL_RESULT
-- Armazena resultado da análise feita pela IA
-- ============================================================
CREATE OR REPLACE PROCEDURE PRC_INSERT_MODEL_RESULT (
    p_user_id IN NUMBER,
    p_job_id IN NUMBER,
    p_score_afinidade_cultural IN NUMBER DEFAULT NULL,
    p_score_compatibilidade_profissional IN NUMBER DEFAULT NULL,
    p_red_flags IN VARCHAR2 DEFAULT NULL,
    p_recomendacao IN VARCHAR2 DEFAULT NULL,
    p_detalhes IN CLOB DEFAULT NULL,
    p_model_result_id OUT NUMBER
) AS
    v_user_exists NUMBER := 0;
    v_job_exists NUMBER := 0;
BEGIN
    -- Verificar se usuário existe
    SELECT COUNT(*) INTO v_user_exists
    FROM USERS
    WHERE id = p_user_id;
    
    IF v_user_exists = 0 THEN
        RAISE_APPLICATION_ERROR(-20031, 'Erro: Usuário não encontrado.');
    END IF;
    
    -- Verificar se vaga existe
    SELECT COUNT(*) INTO v_job_exists
    FROM JOBS
    WHERE id = p_job_id;
    
    IF v_job_exists = 0 THEN
        RAISE_APPLICATION_ERROR(-20032, 'Erro: Vaga não encontrada.');
    END IF;
    
    -- Validação de scores (0 a 100)
    IF p_score_afinidade_cultural IS NOT NULL THEN
        IF p_score_afinidade_cultural < 0 OR p_score_afinidade_cultural > 100 THEN
            RAISE_APPLICATION_ERROR(-20033, 'Erro: Score de afinidade cultural deve estar entre 0 e 100.');
        END IF;
    END IF;
    
    IF p_score_compatibilidade_profissional IS NOT NULL THEN
        IF p_score_compatibilidade_profissional < 0 OR p_score_compatibilidade_profissional > 100 THEN
            RAISE_APPLICATION_ERROR(-20034, 'Erro: Score de compatibilidade profissional deve estar entre 0 e 100.');
        END IF;
    END IF;
    
    -- Inserir resultado da IA
    p_model_result_id := SEQ_MODEL_RESULTS.NEXTVAL;
    
    INSERT INTO MODEL_RESULTS (
        id, user_id, job_id, score_afinidade_cultural, score_compatibilidade_profissional,
        red_flags, recomendacao, detalhes, created_at
    ) VALUES (
        p_model_result_id, p_user_id, p_job_id, p_score_afinidade_cultural, 
        p_score_compatibilidade_profissional, p_red_flags, p_recomendacao, p_detalhes, SYSDATE
    );
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Resultado da IA armazenado com sucesso! ID: ' || p_model_result_id);
    
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END PRC_INSERT_MODEL_RESULT;
/

-- ============================================================
-- ÍNDICES PARA MELHOR PERFORMANCE
-- ============================================================

CREATE INDEX IDX_USERS_EMAIL ON USERS(email);
CREATE INDEX IDX_USERS_ROLE ON USERS(role);
CREATE INDEX IDX_JOBS_TITULO ON JOBS(titulo);
CREATE INDEX IDX_CANDIDATE_SKILLS_USER ON CANDIDATE_SKILLS(user_id);
CREATE INDEX IDX_CANDIDATE_SKILLS_SKILL ON CANDIDATE_SKILLS(skill_id);
CREATE INDEX IDX_JOB_SKILLS_JOB ON JOB_SKILLS(job_id);
CREATE INDEX IDX_MODEL_RESULTS_USER_JOB ON MODEL_RESULTS(user_id, job_id);

-- ============================================================
-- FIM DO SCRIPT
-- ============================================================

