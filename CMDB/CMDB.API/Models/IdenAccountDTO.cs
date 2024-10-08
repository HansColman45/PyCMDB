namespace CMDB.API.Models
{
    public class IdenAccountDTO
    {
        public int Id { get; set; }
        public IdentityDTO Identity { get; set; }
        public AccountDTO Account { get; set; }
        public required DateTime ValidFrom { get; set; }
        public required DateTime ValidUntil { get; set; }
    }
}
