namespace TCA.Desktop.Models;

public class AuthResponseModel
{
    public bool Status { get; set; }
    public int RoleId { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}
