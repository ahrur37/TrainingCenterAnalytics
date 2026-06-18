using System;
using System.Threading.Tasks;
using Desktop.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace TCA.Desktop.Services;

public class SignalRService : IAsyncDisposable
{
    private HubConnection? _connection;

    public event Action<CommentModel>? CommentAdded;

    public async Task StartAsync(string baseUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/hubs/comments")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<CommentModel>("CommentAdded", comment =>
        {
            CommentAdded?.Invoke(comment);
        });

        await _connection.StartAsync();
    }

    public async Task JoinRequest(int requestId)
    {
        if (_connection is { State: HubConnectionState.Connected })
            await _connection.InvokeAsync("JoinRequest", requestId);
    }

    public async Task LeaveRequest(int requestId)
    {
        if (_connection is { State: HubConnectionState.Connected })
            await _connection.InvokeAsync("LeaveRequest", requestId);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
            await _connection.DisposeAsync();
    }
}
