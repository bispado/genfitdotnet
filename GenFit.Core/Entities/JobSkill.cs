namespace GenFit.Core.Entities;

public class JobSkill
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int SkillId { get; set; }
    public string Obrigatoria { get; set; } = "S"; // S ou N
    public decimal Peso { get; set; } = 1.0m;

    // Navigation properties
    public virtual Job Job { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;
}
