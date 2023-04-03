using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public async Task<ActionResult> GetPhotosForModeration()
    {
        var photos = await _photoService.GetUnapprovedPhotos();

        return Ok(photos);
    }
    
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("approve-photo/{photoId}")]
    public async Task<ActionResult> ApprovePhoto(int photoId)
    {
        await _photoService.ApprovePhoto(photoId);
        
        return Ok();
    }
    
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("reject-photo/{photoId}")]
    public async Task<ActionResult> RejectPhoto(int photoId)
    {
        //var user = _authService.FindByEmailFromClaims(User);
        await _photoService.DeletePhotoAsync(photoId);
        
        return Ok();
    }
}