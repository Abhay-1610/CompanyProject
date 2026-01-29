
namespace CompanyProject.Domain.Entities
{
    public class ChangeHistory
    {
        public int ChangeId { get; set; }        // PK

        public int? CompanyId { get; set; }       // FK
        public string CompanyName { get; set; }

        public int? ProjectId { get; set; }       // FK
        public string ProjectName { get; set; }

        public string ChangeType { get; set; } = null!; // Create | Update | Delete

        public string? OldData { get; set; }
        public string? NewData { get; set; }
        public string? ChangedByEmail { get; set; }
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedAt { get; set; }
    }
}
