using Microsoft.AspNetCore.SignalR;

namespace EduRequestSystemAPI.Hubs;

public class CommentHub : Hub
{
    public async Task JoinRequest(int requestId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"request-{requestId}");
    }

    public async Task LeaveRequest(int requestId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"request-{requestId}");
    }
}
