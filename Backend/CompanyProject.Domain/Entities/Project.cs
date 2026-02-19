
namespace CompanyProject.Domain.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }      // PK

        public int CompanyId { get; set; }       // FK
        public Company Company { get; set; } = null!;

        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }

        public string Status { get; set; } = "InProgress";
        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public string CreatedByUserId { get; set; } = null!;
    }
}
