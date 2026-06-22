namespace Desktop.Models;

public class UpdateRequestModel
{
    public string Topic { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public int DirectionId { get; set; }
    public int TrainingFormatId { get; set; }
}
