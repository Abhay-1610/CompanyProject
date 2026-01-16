namespace CompanyProject.Domain.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }          // PK
        public string CompanyName { get; set; } = null!;

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
