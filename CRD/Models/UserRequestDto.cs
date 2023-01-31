namespace CRD.Models
{
    public class UserRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; } 
        public required string Surname { get; set; } 
        public required string IdentityNumber { get; set; }
        public required DateTime BirthDate { get; set; }
    }
    public class UserLoginRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
