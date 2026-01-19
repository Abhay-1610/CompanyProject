using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CompanyProject.Infrastructure.SignalR
{
    public class CompanyHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var companyId = Context.User?.FindFirst("CompanyId")?.Value;

            if (!string.IsNullOrEmpty(companyId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId,companyId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var companyId = Context.User?.FindFirst("CompanyId")?.Value;

            if (!string.IsNullOrEmpty(companyId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId,companyId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
