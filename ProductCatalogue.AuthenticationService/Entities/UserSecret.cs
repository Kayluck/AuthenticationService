namespace ProductCatalogue.AuthenticationService.Entities
{
    public class UserSecret : BaseEntity
    {
        public int UserId { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }

        public User User { get; set; }
    }
}
