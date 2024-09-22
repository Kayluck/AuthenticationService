using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ProductCatalogue.AuthenticationService.DTOs
{
    public class SignUpRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
