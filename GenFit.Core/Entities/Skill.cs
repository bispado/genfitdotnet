namespace GenFit.Core.Entities;

public class Skill
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public string? Descricao { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    public virtual ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
}
