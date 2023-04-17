using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SocialNetworkContext _context;

    public UserRepository(SocialNetworkContext context)
    {
        _context = context;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users.Include(x => x.Photos)
            .ToListAsync();
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public IQueryable<AppUser> GetUserByEmailAsync(string email)
    {
        var query = _context.Users
            .Where(x => x.Email == email)
            .Include(p => p.Photos)
            .AsQueryable();
        
        return query;
    }

    public async Task<AppUser> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.Email == email);
    }

    public async Task<AppUser> GetUserByPhotoId(int photoId)
    {
        return await _context.Users
            .Include(p => p.Photos)
            .IgnoreQueryFilters()
            .Where(p => p.Photos.Any(p => p.Id == photoId))
            .FirstOrDefaultAsync();
    }
    
    public async Task<IQueryable<AppUser>> GetUserFriendsByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        var friends = Enumerable.Empty<AppUser>().AsQueryable();
        if (user != null && user.ThisUserFriends.Count != 0)
        {
            foreach (var friendship in user.ThisUserFriends)
            {
                if (friendship.IsConfirmed)
                {
                    var friend = await _context.Users.FindAsync(friendship);
                    friends.Append(friend);
                }
            }
        }

        return friends;
    }

    public async Task<IQueryable<AppUser>> GetInvitationForFriendshipByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        var friends = Enumerable.Empty<AppUser>().AsQueryable();
        if (user != null && user.ThisUserFriends.Count != 0)
        {
            foreach (var friendship in user.ThisUserFriends)
            {
                if (!friendship.IsConfirmed)
                {
                    var friend = await _context.Users.FindAsync(friendship);
                    friends.Append(friend);
                }
            }
        }

        return friends;
    }

    public IQueryable<AppUser> GetMemberAsync(string userName)
    {
        var query = _context.Users
            .Where(x => x.UserName == userName)
            .AsQueryable();
        
        return query;
    }
}