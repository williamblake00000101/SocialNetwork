using AutoMapper;
using BLL.DTOs;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Specifications;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;
using WebAPI.Helpers;

namespace WebAPI.Controllers;

public class UsersController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly IUserService _userService;
    public UsersController(IUserService userService, IMapper mapper, 
        IPhotoService photoService)
    {
        _userService = userService;
        _photoService = photoService;
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
}