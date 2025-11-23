namespace GenFit.Core.Entities.Azure;

public class AzJob
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? Salario { get; set; }
    public string? Localizacao { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation property
    public ICollection<AzApplication> Applications { get; set; } = new List<AzApplication>();
}

