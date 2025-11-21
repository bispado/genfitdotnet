namespace GenFit.Core.Entities;

public class Course
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; }
    public int? DuracaoHoras { get; set; }
    public string? Nivel { get; set; }
    public DateTime CreatedAt { get; set; }
}
