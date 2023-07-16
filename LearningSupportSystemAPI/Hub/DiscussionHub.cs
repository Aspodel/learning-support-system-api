using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LearningSupportSystemAPI;

// [Authorize]
public class DiscussionHub : Hub
{
    public async Task SendMessage(Message message)
    {
        await Clients.Group($"Discussion-{message.DiscussionId}").SendAsync("ReceiveMessage", message);
    }

    public async Task JoinDiscussion(int discussionId)
    {
        var groupName = $"Discussion-{discussionId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveDiscussion(int discussionId)
    {
        var groupName = $"Discussion-{discussionId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

}