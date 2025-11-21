namespace GenFit.Core.Entities;

public class Job
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? Salario { get; set; }
    public string? Localizacao { get; set; }
    public string? TipoContrato { get; set; }
    public string? Nivel { get; set; }
    public string? ModeloTrabalho { get; set; }
    public string? Departamento { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
    public virtual ICollection<ModelResult> ModelResults { get; set; } = new List<ModelResult>();
}
