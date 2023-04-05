namespace BLL.DTOs;

public class AppUserDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public string Token { get; set; }
    public string PhotoUrl { get; set; }
    public string Gender { get; set; }
    
    public virtual ICollection<int> ThisUserFriendIds { get; set; }
}