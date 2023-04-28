using DAL.Entities;
using DAL.Specifications;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    IQueryable<AppUser> GetUserByEmailAsync(string email);
    Task<AppUser> GetByEmailAsync(string email);
    Task<AppUser> GetUserByPhotoId(int photoId);
    Task<IQueryable<AppUser>> GetUserFriendsByIdAsync(int id);
    Task<IQueryable<AppUser>> GetInvitationForFriendshipByIdAsync(int id);
    IQueryable<AppUser> GetMemberAsync(string userName);
    IQueryable<AppUser> GetMembersAsync(UserParams userParams);
}