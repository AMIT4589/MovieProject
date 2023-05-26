namespace MovieBookingApplication.Configurations
{
    public class SetupJWT : ISetupJWT
    {
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
