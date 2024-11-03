using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class MobileDTO : ModelDTO
    {
        public int MobileId { get; set; }
        [Required,MaxLength(15)]
        public long IMEI { get; set; }
        [Required]
        public AssetTypeDTO MobileType { get; set; }
        public IdentityDTO Identity { get; set; }
        public AssetCategoryDTO Category { get; set; }

        public ICollection<SubscriptionDTO> Subscriptions { get; set; }

        public MobileDTO()
        {
            Subscriptions = new List<SubscriptionDTO>();
        }
    }
}
