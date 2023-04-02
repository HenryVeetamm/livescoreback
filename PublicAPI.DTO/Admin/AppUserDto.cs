namespace PublicAPI.DTO.Admin;

public class AppUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    
   
}