namespace CMDB.Domain.Requests
{
    public class HasAdminAccessRequest
    {
        public int AdminId { get; set; }
        public string Site { get; set; }
        public string Action { get; set; }
    }
}
