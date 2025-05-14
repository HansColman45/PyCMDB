namespace CMDB.API.Helper
{
    /// <summary>
    /// JWT settings
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Secret key for JWT
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Issuer of the token
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Audience of the token
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Token expiry time in minutes
        /// </summary>
        public int TokenExpiryInMinutes { get; set; }
        /// <summary>
        /// Default constructor for JwtSettings
        /// </summary>
        public JwtSettings()
        {
            Secret = "";
            Issuer = "";
            Audience = "";
            TokenExpiryInMinutes = 1;
        }
    }
}
