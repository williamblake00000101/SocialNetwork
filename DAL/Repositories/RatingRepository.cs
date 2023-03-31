using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly SocialNetworkContext _context;

    public RatingRepository(SocialNetworkContext context)
    {
        _context = context;
    }

    public async Task Vote(Rating rating, int appUserId)
    {
        var currentRating = await _context.Ratings
            .FirstOrDefaultAsync(x => x.PhotoId == rating.PhotoId &&
                                      x.AppUserId == appUserId);

        if (currentRating == null)
        {
            var _rating = new Rating();
            _rating.AppUserId = appUserId;
            _rating.PhotoId = rating.PhotoId;
            _rating.Rate = rating.Rate;
            _context.Ratings.Add(rating);
        }
        else
        {
            currentRating.Rate = rating.Rate;
        }

        await _context.SaveChangesAsync();
    }
}