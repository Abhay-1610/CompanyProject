
namespace CompanyProject.Domain.Entities
{
    public class ChangeHistory
    {
        public int ChangeId { get; set; }        // PK

        public int CompanyId { get; set; }       // FK
        public Company Company { get; set; } = null!;

        public int ProjectId { get; set; }       // FK
        public Project Project { get; set; } = null!;

        public string ChangeType { get; set; } = null!; // Create | Update | Delete

        public string? OldData { get; set; }
        public string? NewData { get; set; }

        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedAt { get; set; }
    }
}
