using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(IAuthService authService,
        ITokenService tokenService, IMapper mapper)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _authService = authService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<AppUserDto>> GetCurrentUser()
    {
        return await _authService.FindByEmailFromClaims(User);
    }
    
    [HttpPost("register")] 
    public async Task<ActionResult<AppUserDto>> Register(RegisterDto registerDto)
    {
        return await _authService.RegisterAsync(registerDto);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AppUserDto>> Login(LoginDto loginDto)
    {
        return await _authService.LoginAsync(loginDto);
    }
    
    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _authService.CheckEmailExistsAsync(email);
    }
    
    [HttpGet("usernameexists")]
    public async Task<ActionResult<bool>> CheckUserNameExistsAsync([FromQuery] string userName)
    {
        return await _authService.CheckUserNameExistsAsync(userName);
    }
}