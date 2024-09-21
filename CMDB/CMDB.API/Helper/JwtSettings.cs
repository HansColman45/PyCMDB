namespace CMDB.API.Helper
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int TokenExpiryInMinutes { get; set; }

        public JwtSettings()
        {
            Secret = "";
            Issuer = "";
            Audience = "";
            TokenExpiryInMinutes = 1;
        }
    }
}
