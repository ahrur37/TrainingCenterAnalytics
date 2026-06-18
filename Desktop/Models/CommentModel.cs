using System;

namespace Desktop.Models;

public class CommentModel
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int AuthorId { get; set; }
    public UserModel? Author { get; set; }

    public int RequestId { get; set; }

    public DateTime CreatedAtLocal => CreatedAt.ToLocalTime();
}
