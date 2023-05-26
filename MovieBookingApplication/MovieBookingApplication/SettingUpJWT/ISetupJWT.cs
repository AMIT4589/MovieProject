namespace MovieBookingApplication.Configurations
{
    public interface ISetupJWT
    {
        string Audience { get; set; }
        string Issuer { get; set; }
        string Secret { get; set; }
    }
}
