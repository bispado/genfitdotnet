namespace GenFit.Core.Entities.Azure;

public class AzSkill
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public DateTime CreatedAt { get; set; }
}

