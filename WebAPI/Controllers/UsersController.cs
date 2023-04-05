using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;
using WebAPI.Helpers;

namespace WebAPI.Controllers;

public class UsersController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly IUserService _userService;
    private readonly ISpecializationService _specializationService;
    public UsersController(IUserService userService, IMapper mapper, 
        IPhotoService photoService, ISpecializationService specializationService)
    {
        _userService = userService;
        _photoService = photoService;
        _specializationService = specializationService;
        _mapper = mapper;
    }
    /*
    [Cached(600)]
    [HttpGet]
    public async Task<ActionResult<Pagination<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
    {
        var currentUser = User.GetUserName();

    }
    */
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var currentUsername = User.GetUserName();
        return await _userService.GetMemberAsync(username, 
            isCurrentUser: currentUsername == username);
    }
    
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var currentUsername = User.GetUserName();

        if (currentUsername == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photoDto =_userService.AddPhotoByUser(result, currentUsername);

        return CreatedAtAction(nameof(GetUser), 
            new {username = currentUsername}, photoDto);

    }
    
    //[Cached(600)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<SpecializationDto>>> GetSpecializationTypes()
    {
        return Ok(await _specializationService.GetSpecializationTypes());
    }
}