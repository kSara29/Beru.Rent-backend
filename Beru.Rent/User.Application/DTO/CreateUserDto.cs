namespace User.Application.DTO;

public class CreateUserDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string IIN { get; set; }
    public required string Mail { get; set; }
    public required string Phone { get; set; }
    public required string Password { get; set; }
}