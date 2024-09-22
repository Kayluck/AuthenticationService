namespace ProductCatalogue.AuthenticationService.Utilities
{
    public static class CommonResponseCodes
    {
        public const string Success = "00";
        public const string Failure = "99";
    }

    public static class CommonResponseMessages
    {
        public const string SuccessfulLogin = "Logged in successfully";
        public const string LoginFailure = "Login Failure";
        public const string InvalidLoginCredentials = "Invalid login credentials";
    }
}
