namespace CMDB.API.Helper
{
    public class AppSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public AppSettings()
        {
            Key = string.Empty;
            Issuer = string.Empty;
            Audience = string.Empty;
        }
    }
}