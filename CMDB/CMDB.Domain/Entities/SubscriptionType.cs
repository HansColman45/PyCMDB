using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    public class SubscriptionType : Model
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a Type")]
        public string Type { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter a provider")]
        public string Provider { get; set; }
        [Required(ErrorMessage = "Please select a Category")]
        public AssetCategory Category { get; set; }
        public int? AssetCategoryId { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public override string ToString()
        {
            return $"{Provider} {Type} {Description}";
        }
    }
}