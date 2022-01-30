using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    [Table("category")]
    public class AssetCategory : Model
    {
        public AssetCategory()
        {
            Devices = new List<Device>();
            Types = new List<AssetType>();
            Mobiles = new List<Mobile>();
            Subscriptions = new List<Subscription>();
            SubscriptionTypes = new List<SubscriptionType>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill in a category")]
        public string Category { get; set; }
        public string Prefix { get; set; }

        public virtual ICollection<AssetType> Types { get; set; }
        public ICollection<Device> Devices { get; set; }
        public ICollection<Kensington> Kensingtons { get; set; }
        public ICollection<Mobile> Mobiles { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<SubscriptionType> SubscriptionTypes { get; set; }
    }
}