namespace GenFit.Application.DTOs;

public class JobDto
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
}

public class CreateJobDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? Salario { get; set; }
    public string? Localizacao { get; set; }
    public string? TipoContrato { get; set; }
    public string? Nivel { get; set; }
    public string? ModeloTrabalho { get; set; }
    public string? Departamento { get; set; }
}

public class UpdateJobDto
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public decimal? Salario { get; set; }
    public string? Localizacao { get; set; }
    public string? TipoContrato { get; set; }
    public string? Nivel { get; set; }
    public string? ModeloTrabalho { get; set; }
    public string? Departamento { get; set; }
}
