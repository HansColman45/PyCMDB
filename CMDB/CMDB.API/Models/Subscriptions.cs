namespace CMDB.API.Models
{
    public class SubscriptionDTO : ModelDTO
    {
        public int SubscriptionId { get; set; }
        public SubscriptionTypeDTO SubscriptionType { get; set; }
        public string PhoneNumber { get; set; }
        public IdentityDTO? Identity { get; set; }
        public MobileDTO? Mobile { get; set; }
        public int IdentityId { get; set; }
    }
}
