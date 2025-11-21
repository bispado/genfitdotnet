namespace GenFit.Core.Entities;

public class ModelResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int JobId { get; set; }
    public decimal? ScoreAfinidadeCultural { get; set; }
    public decimal? ScoreCompatibilidadeProfissional { get; set; }
    public string? RedFlags { get; set; }
    public string? Recomendacao { get; set; }
    public string? Detalhes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Job Job { get; set; } = null!;
}
