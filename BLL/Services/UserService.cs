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


    public Task<IEnumerable<AppUserDto>> GetUserFriendsByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AppUserDto>> GetUserInvitationByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AppUserDto>> GetAllInvitationsWhichUserSentById(int userId)
    {
        var profile = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (profile == null) throw new SocialNetworkException("User profile does not exist");
        if (profile.UserIsFriend.Count == 0 || profile.UserIsFriend == null) throw new SocialNetworkException("User has not any friend and invitation");
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

    public Task SendInvitationForFriendshipAsync(int userId, int wantedFriendId)
    {
        throw new NotImplementedException();
    }

    public Task ConfirmFriendship(int userId, int friendToConfirmId)
    {
        throw new NotImplementedException();
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