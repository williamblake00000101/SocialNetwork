using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Helpers;
using BLL.Interfaces;
using CloudinaryDotNet.Actions;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly IPhotoService _photoService;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _photoService = photoService;
    }
    
    public async Task<IEnumerable<AppUserDto>> GetUserFriendsByIdAsync(int id)
    {
        var profile = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
        if (profile == null) throw new SocialNetworkException("User profile does not exist");

        if (profile.ThisUserFriends.Count == 0 || profile.ThisUserFriends == null)
            throw new SocialNetworkException("User has not any friend and invitation");

        var friends = new List<AppUser>();
        foreach (var friendship in profile.ThisUserFriends)
        {
            if (friendship.IsConfirmed)
            {
                var friend = await _unitOfWork.UserRepository.GetUserByIdAsync(friendship.AppUserFriendId.Value);
                friends.Add(friend);
            }
        }

        if (friends.Count() == 0) throw new SocialNetworkException("User has not any confirmed friends");

        return _mapper.Map<IEnumerable<AppUserDto>>(friends);
    }

    public async Task<IEnumerable<AppUserDto>> GetUserInvitationByIdAsync(int id)
    {
        var profile = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
        if (profile == null) throw new SocialNetworkException("User profile does not exist");

        if (profile.ThisUserFriends.Count == 0 || profile.ThisUserFriends == null)
            throw new SocialNetworkException("User has not any friend and invitation");

        var friends = new List<AppUser>();
        foreach (var friendship in profile.ThisUserFriends)
        {
            if (!friendship.IsConfirmed)
            {
                var friend = await _unitOfWork.UserRepository.GetUserByIdAsync(friendship.AppUserFriendId.Value);
                friends.Add(friend);
            }
        }

        if (friends.Count() == 0) throw new SocialNetworkException("User has not any invitations");

        return _mapper.Map<IEnumerable<AppUserDto>>(friends);
    }

    public async Task<IEnumerable<AppUserDto>> GetAllInvitationsWhichUserSentById(int userId)
    {
        var profile = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (profile == null) throw new SocialNetworkException("User profile does not exist");
        if (profile.UserIsFriend.Count == 0 || profile.UserIsFriend == null)
            throw new SocialNetworkException("User has not any friend and invitation");
        var invitations = new List<AppUser>();
        foreach (var invitation in profile.UserIsFriend)
        {
            if (!invitation.IsConfirmed)
            {
                var invited = await _unitOfWork.UserRepository.GetUserByIdAsync(invitation.AppUserId.Value);
                invitations.Add(invited);
            }
        }

        if (invitations.Count() == 0) throw new SocialNetworkException("User has not any invitations");
        return _mapper.Map<IEnumerable<AppUserDto>>(invitations);
    }

    public async Task SendInvitationForFriendshipAsync(int userId, int wantedFriendId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        var friend = await _unitOfWork.UserRepository.GetUserByIdAsync(wantedFriendId);
        if (user == null || friend == null) throw new SocialNetworkException("User does not exist");

        var friendship = friend.ThisUserFriends.FirstOrDefault(fr => fr.AppUserFriendId == wantedFriendId);
        if (friendship != null) throw new SocialNetworkException("Such invitation for friendship already exist");

        friend.ThisUserFriends.Add(new UserFriends() { AppUserId = friend.Id, AppUserFriendId = user.Id });
        _unitOfWork.UserRepository.Update(friend);
        await _unitOfWork.SaveAsync();
    }

    public async Task ConfirmFriendship(int userId, int friendToConfirmId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        var friend = await _unitOfWork.UserRepository.GetUserByIdAsync(friendToConfirmId);
        if (user == null || friend == null) throw new SocialNetworkException("User does not exist");

        var friendship = user.ThisUserFriends.FirstOrDefault(fr => fr.AppUserFriendId == friendToConfirmId);
        if (friendship == null) throw new SocialNetworkException("Such invitation for friendship does not exist");

        if (friendship.IsConfirmed) throw new SocialNetworkException("This friendship is already confirmed");
        friendship.IsConfirmed = true;

        friend.ThisUserFriends.Add(new UserFriends()
            { AppUserId = friend.Id, AppUserFriendId = user.Id, IsConfirmed = true });

        _unitOfWork.UserRepository.Update(user);
        _unitOfWork.UserRepository.Update(friend);

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteFriendByFriendId(int userId, int friendToDeleteId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        var friend = await _unitOfWork.UserRepository.GetUserByIdAsync(friendToDeleteId);
        if (user == null || friend == null) throw new SocialNetworkException("User does not exist");

        var friendship = user.ThisUserFriends.FirstOrDefault(fr => fr.AppUserFriendId == friendToDeleteId);
        if (friendship == null) throw new SocialNetworkException("Such invitation for friendship does not exist");

        var invitation = user.UserIsFriend.FirstOrDefault(fr => fr.AppUserId == friendToDeleteId);
        if (invitation != null)
        {
            user.UserIsFriend.Remove(invitation);
        }

        user.ThisUserFriends.Remove(friendship);

        var friendshipSecond = friend.ThisUserFriends.FirstOrDefault(fr => fr.AppUserFriendId == userId);
        if (friendshipSecond == null) throw new SocialNetworkException("Such invitation for friendship does not exist");

        var invitationSecond = friend.UserIsFriend.FirstOrDefault(fr => fr.AppUserId == userId);
        if (invitationSecond != null)
        {
            friend.UserIsFriend.Remove(invitationSecond);
        }

        friend.ThisUserFriends.Remove(friendshipSecond);

        _unitOfWork.UserRepository.Update(user);
        _unitOfWork.UserRepository.Update(friend);
        await _unitOfWork.SaveAsync();
    }

    public async Task<PhotoDto> AddPhotoByUser(ImageUploadResult result, string userName)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
        
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        
        user.Photos.Add(photo);

        await _unitOfWork.SaveAsync();

        return _mapper.Map<PhotoDto>(photo);
    }

    public async Task<Pagination<MemberDto>> GetMembersAsync(UserParams userParams, string userName)
    {
        var currentUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
        userParams.CurrentUsername = currentUser.UserName;
        
        if (string.IsNullOrEmpty(userParams.Gender))
        {
            userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
        }
        
        var spec = new AppUserWithSpecializationSpecification(userParams);
        var countSpec = new AppUserWithFiltersForCountSpecification(userParams);

        var totalUsers = await _unitOfWork.Repository<AppUser>().CountAsync(countSpec);
        var users = await _unitOfWork.Repository<AppUser>().ListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<MemberDto>>(users);

        return new Pagination<MemberDto>(data,
            totalUsers, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<MemberDto> GetMemberAsync(string userName, bool isCurrentUser)
    {
        var query = _unitOfWork.UserRepository.GetMemberAsync(userName);
        
        if (isCurrentUser) query = query.IgnoreQueryFilters();

        var result = query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider);

        return await result.FirstOrDefaultAsync();
    }

    public async Task UpdateUser(MemberUpdateDto memberUpdateDto, string userName)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);

        if (user == null) throw new SocialNetworkException("Not Found!");

        _mapper.Map(memberUpdateDto, user);

        await _unitOfWork.SaveAsync();
    }

    public async Task SetMainPhotoByUser(int photoId, string userName)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);

        if (user == null) throw new SocialNetworkException("Not Found!");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) throw new SocialNetworkException("Not Found!");

        if (photo.IsMain) throw new SocialNetworkException("This is already your main photo!");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        await _unitOfWork.SaveAsync();
        
    }

    public async Task DeletePhotoByUser(int photoId, string userName)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);

        var photo = await _unitOfWork.PhotoRepository.GetPhotoByIdAsync(photoId);

        if (photo == null) throw new SocialNetworkException("Not Found!");

        if (photo.IsMain) throw new SocialNetworkException("You cannot delete your main photo");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) throw new SocialNetworkException("Error!");
        }

        user.Photos.Remove(photo);

        await _unitOfWork.SaveAsync();

    }
}