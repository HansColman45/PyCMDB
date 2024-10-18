using CMDB.Domain.Entities;

namespace CMDB.Infrastructure
{
    public interface ITokenStore
    {
        static string Token { get; set; }
        static Admin Admin { get; set; }
        static int AdminId { get; set; }
        static string UserName { get; set; }
        static string Password { get; set; }
    }
    public class TokenStore : ITokenStore
    {
        public static string Token { get; set; }
        public static int AdminId { get; set; }
        public static Admin Admin { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
    }
}
