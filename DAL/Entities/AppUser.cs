﻿using Microsoft.AspNetCore.Identity;

namespace DAL.Entities;

public class AppUser : IdentityUser<int>
{
    public DateOnly DateOfBirth { get; set; }
    
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Gender { get; set; }
    public string Introduction { get; set; }
    public string LookingFor { get; set; }
    public string Interests { get; set; }
    
    public virtual Specialization Specialization { get; set; }
    public int? SpecializationId { get; set; }
    
    public List<Photo> Photos { get; set; } = new();

    public List<UserLike> LikedByUsers { get; set; }
    public List<UserLike> LikedUsers { get; set; }

    public List<Message> MessagesSent { get; set; }
    public List<Message> MessagesReceived { get; set; }
    
    public virtual ICollection<UserFriends> UserIsFriend { get; set; }
    public virtual ICollection<UserFriends> ThisUserFriends { get; set; }

    public ICollection<AppUserRole> UserRoles { get; set; }
    
    
    public AppUser()
    {
        UserIsFriend ??= new HashSet<UserFriends>();
        ThisUserFriends ??= new HashSet<UserFriends>();
    }
}