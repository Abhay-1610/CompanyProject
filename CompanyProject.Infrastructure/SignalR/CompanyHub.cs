using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CompanyProject.Infrastructure.SignalR
{
    public class CompanyHub : Hub
    {
        // Triggered automatically when a client successfully connects
        public override async Task OnConnectedAsync()
        {
            var companyId = Context.User?.FindFirst("companyId")?.Value;

            if (!string.IsNullOrEmpty(companyId))
            {
                // This enables company-specific message broadcasting
                await Groups.AddToGroupAsync(Context.ConnectionId, companyId);

                // Increment in-memory online user count for this company
                int count = CompanyOnlineTracker.Increment(int.Parse(companyId));

                await Clients.Group(companyId).SendAsync("OnlineUsersChanged", count);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var companyId = Context.User?.FindFirst("companyId")?.Value;

            if (!string.IsNullOrEmpty(companyId))
            {
                // Remove this connection from the company SignalR group
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, companyId);

                // Decrement in-memory online user count for this company
                int count = CompanyOnlineTracker.Decrement(int.Parse(companyId));

                await Clients.Group(companyId).SendAsync("OnlineUsersChanged", count);
            }
        }
    }
}
