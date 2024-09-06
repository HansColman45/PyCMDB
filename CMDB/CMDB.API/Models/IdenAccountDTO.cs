namespace CMDB.API.Models
{
    public class IdenAccountDTO
    {
        public int Id { get; set; }
        public required IdentityDTO Identity { get; set; }
        public required AccountDTO Account { get; set; }
        public required DateTime ValidFrom { get; set; }
        public required DateTime ValidUntil { get; set; }
    }
}
