using AutoMapper;
using BLL.DTOs;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;

[Authorize]
public class RatingController : BaseApiController
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] RatingDto ratingDto)
    {
        var sourceUserId = User.GetUserId();
        await _ratingService.Vote(ratingDto, sourceUserId);

        return NoContent();
    }
}