using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LearningSupportSystemAPI;

public class DiscussionHub : Hub
{
    public async Task SendMessage(int discussionId, string content)
    {
        // Get the current user's ID
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Here you can perform additional checks to validate the user, discussion, etc.

        // Save the message to the database (you'll need to implement your data access here)

        // Broadcast the message to all connected clients in the same group (discussion)
        await Clients.Group(discussionId.ToString()).SendAsync("ReceiveMessage", userId, content);
    }

    public async Task JoinDiscussionGroup(int discussionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, discussionId.ToString());
    }

    public async Task LeaveDiscussionGroup(int discussionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, discussionId.ToString());
    }
}