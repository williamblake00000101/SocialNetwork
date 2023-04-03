namespace BLL.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<object>> GetUsersWithRoles();
    Task<IEnumerable<string>> EditRoles(string username, string roles);
}