using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    public class SubscriptionType: Model
    {
        [Key]
        [Column("Type_ID")]
        public int TypeID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Provider { get; set; }
        public AssetCategory Category { get; set; }
    }
}