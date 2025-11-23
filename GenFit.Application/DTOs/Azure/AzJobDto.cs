namespace GenFit.Application.DTOs.Azure;

public class AzJobDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? Salario { get; set; }
    public string? Localizacao { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateAzJobDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? Salario { get; set; }
    public string? Localizacao { get; set; }
}

public class UpdateAzJobDto
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public decimal? Salario { get; set; }
    public string? Localizacao { get; set; }
}

