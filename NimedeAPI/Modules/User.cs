namespace NimedeAPI.Modules
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Hashed password
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
