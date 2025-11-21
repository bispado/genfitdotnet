namespace GenFit.Core.Entities;

public class QuestionnaireAnswer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public int? RespostaLikert { get; set; }
    public string? RespostaTexto { get; set; }
    public string? RespostaOpcao { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual QuestionnaireQuestion Question { get; set; } = null!;
}
