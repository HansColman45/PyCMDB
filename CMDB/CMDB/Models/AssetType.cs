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
        [Required(ErrorMessage = "Please fill in a vendor")]
        public string Vendor { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required]
        public AssetCategory Cateory { get; set; }

        public override string ToString()
        {
            return Vendor + " " + Type;
        }
    }
}