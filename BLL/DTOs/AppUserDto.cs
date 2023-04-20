namespace BLL.DTOs;

public class AppUserDto
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Token { get; set; }
    public string PhotoUrl { get; set; }
    public string Gender { get; set; }
    
    public virtual ICollection<int> ThisUserFriendIds { get; set; }
}