namespace CompanyProject.Application.Interfaces
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public int? CompanyId { get; set; }
    }
}
