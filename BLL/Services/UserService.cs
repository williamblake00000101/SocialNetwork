using System.Collections;
using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
}