namespace GenFit.Core.Entities.Azure;

public class AzApplication
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendente";
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public AzJob? Job { get; set; }
}

