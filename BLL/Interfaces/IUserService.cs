using BLL.DTOs;
using BLL.Helpers;
using CloudinaryDotNet.Actions;
using DAL.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Interfaces;

public interface IUserService
{
    Task<IEnumerable<AppUserDto>> GetUserFriendsByIdAsync(int id);
    Task<IEnumerable<AppUserDto>> GetUserInvitationByIdAsync(int id);
    Task<IEnumerable<AppUserDto>> GetAllInvitationsWhichUserSentById(int userId);
    Task SendInvitationForFriendshipAsync(int userId, int wantedFriendId);
    Task ConfirmFriendship(int userId, int friendToConfirmId);
    Task DeleteFriendByFriendId(int userId, int friendToDeleteId);
    Task<PhotoDto> AddPhotoByUser(ImageUploadResult result, string userName);
    Task<Pagination<MemberDto>> GetMembersAsync(UserParams userParams, string userName);
    Task<MemberDto> GetMemberAsync(string userName, bool isCurrentUser);
    Task<MemberDto> GetMemberByIdAsync(int id);
    Task UpdateUser(MemberUpdateDto memberUpdateDto, string userName);
    Task SetMainPhotoByUser(int photoId, string userName);
    Task DeletePhotoByUser(int photoId, string userName);
}