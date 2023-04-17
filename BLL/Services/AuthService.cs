using System.Security.Claims;
using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<AppUserDto> FindByEmailFromClaims(object ob)
    {
        var user = await _unitOfWork.UserManager.FindByEmailFromClaimsPrincipal(ob as ClaimsPrincipal);

        return new AppUserDto
        {
            Email = user.Email,
            Token = await _tokenService.CreateToken(user),
            UserName = user.UserName,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Gender = user.Gender
        };
    }
    
    public async Task<AppUserDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await CheckUserNameExistsAsync(registerDto.UserName)) throw new SocialNetworkException("UserName is taken");
        
        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.UserName.ToLower();

        var result = await _unitOfWork.UserManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) throw new SocialNetworkException("Error!");

        var roleResult = await _unitOfWork.UserManager.AddToRoleAsync(user, "Member");

        if (!roleResult.Succeeded) throw new SocialNetworkException("Error!");

        return new AppUserDto
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = await _tokenService.CreateToken(user),
            Gender = user.Gender
        };
    }

    public async Task<AppUserDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _unitOfWork.UserManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null) throw new SocialNetworkException("Invalid email");

        var result = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) throw new SocialNetworkException("Invalid password");

        return new AppUserDto
        {
            Email = user.Email,
            Token = await _tokenService.CreateToken(user),
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Gender = user.Gender
        };
    }

    public async Task<bool> CheckUserNameExistsAsync(string userName)
    {
        if (userName == null) throw new SocialNetworkException("UserName doesn't exist");
        return await _unitOfWork.UserManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        if (email == null) throw new SocialNetworkException("Email doesn't exist");
        return await _unitOfWork.UserManager.FindByEmailAsync(email) != null;
    }
}