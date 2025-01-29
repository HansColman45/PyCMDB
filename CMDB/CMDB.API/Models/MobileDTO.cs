using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class MobileDTO : ModelDTO
    {
        public int MobileId { get; set; }
        [Required]
        public long IMEI { get; set; }
        [Required]
        public AssetTypeDTO MobileType { get; set; }
        public IdentityDTO Identity { get; set; }

        public SubscriptionDTO? Subscription { get; set; }
        public int IdentityId { get; set; }

        public MobileDTO()
        {
        }
    }
}
