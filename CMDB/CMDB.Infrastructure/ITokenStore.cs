namespace CMDB.Infrastructure
{
    public interface ITokenStore
    {
        static string Token { get; set; }
    }
    public class TokenStore : ITokenStore
    {
        public static string Token { get; set; }
        public static int AdminId { get; set; }
    }
}
