using BLL.DTOs;

namespace BLL.Interfaces;

public interface IRatingService
{
    Task Vote(RatingDto rating, int appUserId);
}