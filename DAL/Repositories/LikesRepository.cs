using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class LikesRepository : ILikesRepository
{
    private readonly SocialNetworkContext _context;

    public LikesRepository(SocialNetworkContext context)
    {
        _context = context;
    }

    public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await _context.Likes.FindAsync(sourceUserId, targetUserId);
    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await _context.Users
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}