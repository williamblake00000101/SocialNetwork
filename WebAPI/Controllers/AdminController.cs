using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
public class AdminController : BaseApiController
{
    private readonly IAdminService _adminService;
    private readonly IPhotoService _photoService;

    public AdminController(IAdminService adminService,
        IPhotoService photoService)
    {
        _photoService = photoService;
        _adminService = adminService;
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await _adminService.GetUsersWithRoles();
        
        return Ok(users);
    }
    
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
    {
        var result = await _adminService.EditRoles(username, roles);
        
        return Ok(result);
    }
}