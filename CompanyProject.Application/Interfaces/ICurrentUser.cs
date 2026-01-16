namespace CompanyProject.Application.Interfaces
{
    // Gives information about the logged-in user
    public interface ICurrentUser
    {
        string UserId { get; }
        int CompanyId { get; }
        string Role { get; }
        bool IsBlocked { get; }
    }
}
