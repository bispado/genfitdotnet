namespace GenFit.Application.DTOs;

public class SkillDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public string? Descricao { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSkillDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public string? Descricao { get; set; }
}

public class UpdateSkillDto
{
    public string? Nome { get; set; }
    public string? Categoria { get; set; }
    public string? Descricao { get; set; }
}
