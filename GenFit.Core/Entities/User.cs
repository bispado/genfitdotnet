namespace GenFit.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty; // candidate ou employee
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? SenhaHash { get; set; }
    public string? Cpf { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? LinkedInUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    public virtual ICollection<QuestionnaireAnswer> QuestionnaireAnswers { get; set; } = new List<QuestionnaireAnswer>();
    public virtual ICollection<ModelResult> ModelResults { get; set; } = new List<ModelResult>();
}
