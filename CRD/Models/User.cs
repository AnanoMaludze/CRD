namespace CRD.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name{ get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } 
    }
}
