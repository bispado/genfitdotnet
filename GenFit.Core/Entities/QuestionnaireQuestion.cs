namespace GenFit.Core.Entities;

public class QuestionnaireQuestion
{
    public int Id { get; set; }
    public string PerguntaTexto { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int? EscalaMin { get; set; }
    public int? EscalaMax { get; set; }
    public string? Opcoes { get; set; }
    public string? Categoria { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<QuestionnaireAnswer> QuestionnaireAnswers { get; set; } = new List<QuestionnaireAnswer>();
}
