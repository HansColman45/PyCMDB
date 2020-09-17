using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    [Table("category")]
    public class AssetCategory: Model
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Category { get; set; }
        public string Prefix { get; set; }
    }
}