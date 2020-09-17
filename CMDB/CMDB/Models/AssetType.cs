using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    [Table("assettype")]
    public class AssetType: Model
    {
        [Column("Type_ID")]
        [Key]
        public int TypeID { get; set; }
        [Required]
        public string Vendor { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public AssetCategory Cateory { get; set; }
    }
}