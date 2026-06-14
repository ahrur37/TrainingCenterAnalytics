using System;

namespace Desktop.Models;

public class RequestModel
{
    public int Id { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? ContactInfo { get; set; }

    public int DirectionId { get; set; }
    public DirectionModel? Direction { get; set; }

    public int TrainingFormatId { get; set; }
    public TrainingFormatModel? TrainingFormat { get; set; }

    public int AuthorId { get; set; }
    public UserModel? Author { get; set; }

    public int? AssigneeId { get; set; }
    public UserModel? Assignee { get; set; }

    public int StatusId { get; set; }
    public StatusModel? Status { get; set; }
}
