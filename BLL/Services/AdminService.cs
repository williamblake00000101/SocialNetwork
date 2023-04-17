using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdminService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<object>>  GetUsersWithRoles()
    {
        var users = await _unitOfWork.UserManager.Users
            .OrderBy(u => u.UserName)
            .Select(u => new
            {
                u.Id,
                Username = u.UserName,
                Email = u.Email,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            })
            .ToListAsync();

        return users;
    }

    public async Task<IEnumerable<string>> EditRoles(string username, string roles)
    {
        if (string.IsNullOrEmpty(roles)) throw new SocialNetworkException("You must select at least one role");

        var selectedRoles = roles.Split(",").ToArray();

        var user = await _unitOfWork.UserManager.FindByNameAsync(username);

        if (user == null) throw new SocialNetworkException("Not Found!");

        var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);

        var result = await _unitOfWork.UserManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if (!result.Succeeded) throw new SocialNetworkException("Failed to add to roles");

        result = await _unitOfWork.UserManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if (!result.Succeeded) throw new SocialNetworkException("Failed to remove from roles");

        var tmp =await _unitOfWork.UserManager.GetRolesAsync(user);
        
        return tmp;
    }
    
    
}