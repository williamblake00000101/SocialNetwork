namespace DAL.Entities;

public class UserFriends
{
    public virtual AppUser AppUser { get; set; }
    public int? AppUserId { get; set; }

    public virtual AppUser AppUserFriend { get; set; }
    public int? AppUserFriendId { get; set; }
    
    public bool IsConfirmed { get; set; }
}