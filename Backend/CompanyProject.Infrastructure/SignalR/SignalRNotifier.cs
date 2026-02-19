using CompanyProject.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CompanyProject.Infrastructure.SignalR
{
    public class SignalRNotifier : IRealtimeNotifier
    {
        private readonly IHubContext<CompanyHub> _hubContext;

        public SignalRNotifier(IHubContext<CompanyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyCompanyAsync(int companyId,string eventName,object data)
        {
            await _hubContext.Clients.Group(companyId.ToString()).SendAsync(eventName, data);
        }
    }
}
