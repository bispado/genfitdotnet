namespace GenFit.Core.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string TabelaAfetada { get; set; } = string.Empty;
    public int RegistroId { get; set; }
    public string Operacao { get; set; } = string.Empty; // INSERT, UPDATE, DELETE
    public string? UsuarioBanco { get; set; }
    public DateTime DataHora { get; set; }
    public string? DadosAntigos { get; set; }
    public string? DadosNovos { get; set; }
}
