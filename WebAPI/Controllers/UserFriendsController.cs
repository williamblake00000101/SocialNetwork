using System.Security.Claims;
using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
public class UserFriendsController : BaseApiController
{
    private readonly IUserService _userService;

    public UserFriendsController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("user-friends/{id}")]
    public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUserFriendsById(int id)
    {
        var result = await _userService.GetUserFriendsByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    [Route("own-friends")]
    public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUserOwnFriendsById()
    {
        var result =
            await _userService.GetUserFriendsByIdAsync(
                Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
        return Ok(result);
    }

    [HttpGet]
    [Route("get-own-invitations")]
    public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUserOwnInvitationsById()
    {
        var result =
            await _userService.GetUserInvitationByIdAsync(
                Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
        return Ok(result);
    }

    [HttpPut]
    [Route("confirm/{friendId}")]
    public async Task<ActionResult> ConfirmFriendshipWithUser(int friendId)
    {
        await _userService.ConfirmFriendship(
            Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value), friendId);
        return Ok();
    }

    [HttpDelete]
    [Route("{friendId}")]
    public async Task<ActionResult> DeleteUserFromFriends(int friendId)
    {
        await _userService.DeleteFriendByFriendId(
            Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value), friendId);
        return Ok();
    }

    [HttpPost]
    [Route("invite/{friendId}")]
    public async Task<ActionResult> SendInvitationForFriendship(int friendId)
    {
        await _userService.SendInvitationForFriendshipAsync(
            Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value), friendId);
        return Ok();
    }
}