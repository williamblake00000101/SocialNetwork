using DAL.Entities;

namespace DAL.Interfaces;

public interface IRatingRepository
{
    Task Vote(Rating rating, int appUserId);
}