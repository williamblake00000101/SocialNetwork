using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

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
    

}