namespace ProductCatalogue.AuthenticationService.DTOs
{
    public class CreateUserDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
    }
}
