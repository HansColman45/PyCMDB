using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<AssetType> Types { get; set; }
        [JsonIgnore]
        public virtual ICollection<Device> Devices { get; set; }
        [JsonIgnore]
        public virtual ICollection<Kensington> Kensingtons { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mobile> Mobiles { get; set; }
        [JsonIgnore]
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        [JsonIgnore]
        public virtual ICollection<SubscriptionType> SubscriptionTypes { get; set; }
    }
}