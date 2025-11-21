using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using GenFit.Infrastructure.Data;

namespace GenFit.Infrastructure.Services;

public class OracleProcedureService
{
    private readonly GenFitDbContext _context;

    public OracleProcedureService(GenFitDbContext context)
    {
        _context = context;
    }

    public async Task<int> ExecutePrcInsertUserAsync(
        string nome,
        string email,
        string role,
        string? senhaHash = null,
        string? cpf = null,
        string? telefone = null,
        DateTime? dataNascimento = null,
        string? linkedinUrl = null)
    {
        var userIdParam = new OracleParameter("p_user_id", OracleDbType.Int32, ParameterDirection.Output);

        await _context.Database.ExecuteSqlRawAsync(
            "BEGIN PRC_INSERT_USER(:p_nome, :p_email, :p_role, :p_senha_hash, :p_cpf, :p_telefone, :p_data_nascimento, :p_linkedin_url, :p_user_id); END;",
            new OracleParameter("p_nome", nome),
            new OracleParameter("p_email", email),
            new OracleParameter("p_role", role),
            new OracleParameter("p_senha_hash", (object?)senhaHash ?? DBNull.Value),
            new OracleParameter("p_cpf", (object?)cpf ?? DBNull.Value),
            new OracleParameter("p_telefone", (object?)telefone ?? DBNull.Value),
            new OracleParameter("p_data_nascimento", (object?)dataNascimento ?? DBNull.Value),
            new OracleParameter("p_linkedin_url", (object?)linkedinUrl ?? DBNull.Value),
            userIdParam
        );

        return ConvertOracleValueToInt(userIdParam.Value);
    }

    public async Task<int> ExecutePrcInsertJobAsync(
        string titulo,
        string? descricao = null,
        decimal? salario = null,
        string? localizacao = null,
        string? tipoContrato = null,
        string? nivel = null,
        string? modeloTrabalho = null,
        string? departamento = null)
    {
        var jobIdParam = new OracleParameter("p_job_id", OracleDbType.Int32, ParameterDirection.Output);

        await _context.Database.ExecuteSqlRawAsync(
            "BEGIN PRC_INSERT_JOB(:p_titulo, :p_descricao, :p_salario, :p_localizacao, :p_tipo_contrato, :p_nivel, :p_modelo_trabalho, :p_departamento, :p_job_id); END;",
            new OracleParameter("p_titulo", titulo),
            new OracleParameter("p_descricao", (object?)descricao ?? DBNull.Value),
            new OracleParameter("p_salario", (object?)salario ?? DBNull.Value),
            new OracleParameter("p_localizacao", (object?)localizacao ?? DBNull.Value),
            new OracleParameter("p_tipo_contrato", (object?)tipoContrato ?? DBNull.Value),
            new OracleParameter("p_nivel", (object?)nivel ?? DBNull.Value),
            new OracleParameter("p_modelo_trabalho", (object?)modeloTrabalho ?? DBNull.Value),
            new OracleParameter("p_departamento", (object?)departamento ?? DBNull.Value),
            jobIdParam
        );

        return ConvertOracleValueToInt(jobIdParam.Value);
    }

    public async Task<int> ExecutePrcInsertCandidateSkillAsync(
        int userId,
        int skillId,
        decimal? nivelProficiencia = null)
    {
        var candidateSkillIdParam = new OracleParameter("p_candidate_skill_id", OracleDbType.Int32, ParameterDirection.Output);

        await _context.Database.ExecuteSqlRawAsync(
            "BEGIN PRC_INSERT_CANDIDATE_SKILL(:p_user_id, :p_skill_id, :p_nivel_proficiencia, :p_candidate_skill_id); END;",
            new OracleParameter("p_user_id", userId),
            new OracleParameter("p_skill_id", skillId),
            new OracleParameter("p_nivel_proficiencia", (object?)nivelProficiencia ?? DBNull.Value),
            candidateSkillIdParam
        );

        return ConvertOracleValueToInt(candidateSkillIdParam.Value);
    }

    public async Task<int> ExecutePrcInsertModelResultAsync(
        int userId,
        int jobId,
        decimal? scoreAfinidadeCultural = null,
        decimal? scoreCompatibilidadeProfissional = null,
        string? redFlags = null,
        string? recomendacao = null,
        string? detalhes = null)
    {
        var modelResultIdParam = new OracleParameter("p_model_result_id", OracleDbType.Int32, ParameterDirection.Output);

        await _context.Database.ExecuteSqlRawAsync(
            "BEGIN PRC_INSERT_MODEL_RESULT(:p_user_id, :p_job_id, :p_score_afinidade_cultural, :p_score_compatibilidade_profissional, :p_red_flags, :p_recomendacao, :p_detalhes, :p_model_result_id); END;",
            new OracleParameter("p_user_id", userId),
            new OracleParameter("p_job_id", jobId),
            new OracleParameter("p_score_afinidade_cultural", (object?)scoreAfinidadeCultural ?? DBNull.Value),
            new OracleParameter("p_score_compatibilidade_profissional", (object?)scoreCompatibilidadeProfissional ?? DBNull.Value),
            new OracleParameter("p_red_flags", (object?)redFlags ?? DBNull.Value),
            new OracleParameter("p_recomendacao", (object?)recomendacao ?? DBNull.Value),
            new OracleParameter("p_detalhes", (object?)detalhes ?? DBNull.Value),
            modelResultIdParam
        );

        return ConvertOracleValueToInt(modelResultIdParam.Value);
    }

    private int ConvertOracleValueToInt(object? value)
    {
        if (value == null || value == DBNull.Value)
            throw new InvalidOperationException("O valor retornado pela procedure é nulo");

        // Tratar diferentes tipos que o Oracle pode retornar
        // Oracle retorna NUMBER como OracleDecimal
        if (value is OracleDecimal oracleDecimal)
        {
            // Converter OracleDecimal para decimal primeiro, depois para int
            return (int)oracleDecimal.Value;
        }
        
        // Se já for decimal, converter diretamente
        if (value is decimal dec)
            return (int)dec;
        
        // Se já for int, retornar diretamente
        if (value is int intVal)
            return intVal;
        
        // Se for long, converter para int
        if (value is long longVal)
            return (int)longVal;

        // Tentar conversão genérica via ToString() primeiro
        try
        {
            // Converter para string e depois para int (mais seguro)
            if (decimal.TryParse(value.ToString(), out decimal decimalValue))
                return (int)decimalValue;
            
            return Convert.ToInt32(value);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Não foi possível converter o valor do tipo {value?.GetType().Name} para int: {ex.Message}", ex);
        }
    }
}
