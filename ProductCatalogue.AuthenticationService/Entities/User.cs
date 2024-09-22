namespace ProductCatalogue.AuthenticationService.Entities
{
    public class User : BaseEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public UserSecret UserSecret { get; set; }
    }
}
