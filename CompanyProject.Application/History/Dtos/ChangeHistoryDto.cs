namespace CompanyProject.Application.History.Dtos
{
    public class ChangeHistoryDto
    {
        public int ChangeId { get; set; }

        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }

        public string ChangeType { get; set; } = null!;

        public string? OldData { get; set; }
        public string? NewData { get; set; }

        public string ChangedBy { get; set; } = null!;
        public string? ChangedByEmail { get; set; }

        public DateTime ChangedAt { get; set; }
    }
}
