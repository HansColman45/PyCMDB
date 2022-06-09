using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class Mobile : Model
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IMEI { get; set; }
        [Required]
        public AssetType MobileType { get; set; }
        public Identity Identity { get; set; }
        public AssetCategory Category { get; set; }
        public int? TypeId { get; set; }
        public int? IdentityId { get; set; }
        public int? CategoryId { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}