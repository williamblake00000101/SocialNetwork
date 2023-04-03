using BLL.DTOs;

namespace BLL.Interfaces;

public interface IAuthService
{
    Task<AppUserDto> FindByEmailFromClaims(object ob);
    Task<AppUserDto> RegisterAsync(RegisterDto registerDto);
    Task<AppUserDto> LoginAsync(LoginDto loginDto);
    Task<bool> CheckUserNameExistsAsync(string userName);
    Task<bool> CheckEmailExistsAsync(string email);
}