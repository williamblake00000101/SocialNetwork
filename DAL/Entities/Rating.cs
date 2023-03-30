namespace DAL.Entities;

public class Rating : BaseEntity
{
    public int Rate { get; set; }
    
    public int? PhotoId { get; set; }
    public Photo Photo { get; set; }
    
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}