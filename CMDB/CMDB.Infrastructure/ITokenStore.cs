using CMDB.Domain.Entities;

namespace CMDB.Infrastructure
{
    public interface ITokenStore
    {
        static string Token { get; set; }
        static Admin Admin { get; set; }
        static int AdminId { get; set; }
    }
    public class TokenStore : ITokenStore
    {
        public static string Token { get; set; }
        public static int AdminId { get; set; }
        public static Admin Admin { get; set; }
    }
}
