using BLL.DTOs;

namespace BLL.Interfaces;

public interface IUserService
{
    Task<IEnumerable<AppUserDto>> GetUserFriendsByIdAsync(int id);
    Task<IEnumerable<AppUserDto>> GetUserInvitationByIdAsync(int id);
    Task<IEnumerable<AppUserDto>> GetAllInvitationsWhichUserSentById(int userId);
    Task SendInvitationForFriendshipAsync(int userId, int wantedFriendId);
    Task ConfirmFriendship(int userId, int friendToConfirmId);
    Task DeleteFriendByFriendId(int userId, int friendToDeleteId);
}