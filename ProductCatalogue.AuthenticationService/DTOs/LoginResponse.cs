namespace ProductCatalogue.AuthenticationService.DTOs
{
    public class LoginResponse : UserDto
    {
        public string Token { get; set; }
    }
}
