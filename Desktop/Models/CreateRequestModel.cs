namespace Desktop.Models;

public class CreateRequestModel
{
    public string Topic { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ContactInfo { get; set; }
    public int DirectionId { get; set; }
    public int TrainingFormatId { get; set; }
    public int AuthorId { get; set; }
}
