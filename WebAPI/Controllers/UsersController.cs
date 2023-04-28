using AutoMapper;
using BLL.DTOs;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Specifications;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Errors;
using WebAPI.Extensions;
using WebAPI.Helpers;

namespace WebAPI.Controllers;

public class UsersController : BaseApiController
{
    private readonly IPhotoService _photoService;
    private readonly IUserService _userService;
    private readonly ISpecializationService _specializationService;

    public UsersController(IUserService userService,
        IPhotoService photoService, ISpecializationService specializationService)
    {
        _userService = userService;
        _photoService = photoService;
        _specializationService = specializationService;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        var currentUsername = User.GetUserName();

        if (currentUsername == null) return NotFound();

        var result = await _userService.GetMembersAsync(userParams, currentUsername);
        
        Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, 
            result.TotalCount, result.TotalPages));

        return Ok(result);
    }

    [HttpGet("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var currentUsername = User.GetUserName();

        if (currentUsername == null) return NotFound(new ApiResponse(404));

        return await _userService.GetMemberAsync(currentUsername,
            isCurrentUser: currentUsername == username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var currentUsername = User.GetUserName();

        if (currentUsername == null) return NotFound();

        await _userService.UpdateUser(memberUpdateDto, currentUsername);

        return NoContent();
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var currentUsername = User.GetUserName();

        if (currentUsername == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photoDto = _userService.AddPhotoByUser(result, currentUsername);

        var user = _userService.GetMemberAsync(User.GetUserName(), true);

        return CreatedAtAction(nameof(GetUser),
            new { username = user.Result.UserName }, photoDto);
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var currentUsername = User.GetUserName();

        if (currentUsername == null) return NotFound();

        await _userService.SetMainPhotoByUser(photoId, currentUsername);

        return NoContent();
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var currentUsername = User.GetUserName();

        if (currentUsername == null) return NotFound();

        await _userService.DeletePhotoByUser(photoId, currentUsername);

        return Ok();
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<SpecializationDto>>> GetSpecializationTypes()
    {
        return Ok(await _specializationService.GetSpecializationTypes());
    }
}