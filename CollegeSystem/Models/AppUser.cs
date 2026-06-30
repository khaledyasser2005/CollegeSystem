namespace CollegeSystem.Models
{
    public class AppUser
    {
        public int ID{ get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}