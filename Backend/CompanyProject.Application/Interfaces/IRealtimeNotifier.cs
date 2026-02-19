namespace CompanyProject.Application.Interfaces
{
    public interface IRealtimeNotifier
    {
        Task NotifyCompanyAsync(
            int companyId,
            string eventName,
            object data);
    }
}
