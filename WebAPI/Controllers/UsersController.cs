using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
    
    //[Cached(600)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<SpecializationDto>>> GetSpecializationTypes()
    {
        return Ok(await _specializationService.GetSpecializationTypes());
    }
}