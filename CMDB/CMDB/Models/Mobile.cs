using System.ComponentModel.DataAnnotations;

namespace CMDB.Models
{
    public class Mobile: Model
    {
        [Key]
        [Required]
        public int IMEI { get; set; }
        [Required]
        public AssetType MobileType { get; set; }
        public Identity Identity { get; set; }
    }
}