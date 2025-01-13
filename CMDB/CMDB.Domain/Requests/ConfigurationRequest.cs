namespace CMDB.Domain.Requests
{
    public class ConfigurationRequest
    {
        public required string Code { get; set; }
        public required string SubCode { get; set; }
    }
}
