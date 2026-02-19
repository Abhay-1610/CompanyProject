using MediatR;

namespace CompanyProject.Application.History.Create
{
    public class CreateChangeHistoryCommand : IRequest
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ChangeType { get; set; } = string.Empty; // Create | Update | Delete
        public string? OldData { get; set; }
        public int? companyId { get; set; }
        public string? CompanyName { get; set; }
        public string? NewData { get; set; }
    }
}
