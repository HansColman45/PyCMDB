using CMDB.Domain.Entities;

namespace CMDB.Domain.Responses
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Level { get; set; }
        public string Token { get; set; }
        public AuthenticateResponse(Admin admin, string token)
        {
            Id = admin.AdminId;
            UserId = admin.Account.UserID;
            Level = admin.Level;
            Token = token;
        }
    }
}
