using System.Text.Json;

namespace ProductCatalogue.AuthenticationService.DTOs
{
    public class SignUpResponse
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
