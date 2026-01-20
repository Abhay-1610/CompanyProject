namespace CompanyProject.Application.Interfaces
{
    // Gives information about the logged-in user
    public interface ICurrentUser
    {
        string UserId { get; }

        // null => SuperAdmin (not company-scoped)
        int? CompanyId { get; }

        string Role { get; }

        bool IsSuperAdmin { get; }
    }
}
