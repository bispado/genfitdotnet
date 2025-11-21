namespace GenFit.Core.Entities;

public class CandidateSkill
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SkillId { get; set; }
    public decimal? NivelProficiencia { get; set; }
    public DateTime DataAquisicao { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;
}
