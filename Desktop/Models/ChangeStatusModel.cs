namespace Desktop.Models;

public class ChangeStatusModel
{
    public int RequestId { get; set; }
    public int NewStatusId { get; set; }
    public int CurrentUserId { get; set; }
}
