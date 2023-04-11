using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Errors;

namespace WebAPI.Controllers;

public class BuggyController : BaseApiController
{
    private readonly IUserService _userService;

    public BuggyController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<MemberDto> GetNotFound()
    {
        var thing = _userService.GetMemberByIdAsync(-1);

        if (thing == null) return NotFound();

        return thing.Result;
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        var thing = _userService.GetMemberByIdAsync(-1);

        var thingToReturn = thing.ToString();

        return thingToReturn;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}