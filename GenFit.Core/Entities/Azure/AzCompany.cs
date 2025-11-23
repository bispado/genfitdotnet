namespace GenFit.Core.Entities.Azure;

public class AzCompany
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Setor { get; set; }
    public DateTime CreatedAt { get; set; }
}

