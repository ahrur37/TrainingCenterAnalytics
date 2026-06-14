namespace Desktop.Models;

public class CreateCommentModel
{
    public string Content { get; set; } = string.Empty;
    public int RequestId { get; set; }
    public int AuthorId { get; set; }
}
