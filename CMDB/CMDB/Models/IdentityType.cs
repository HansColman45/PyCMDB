using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    [Table("identitytype")]
    public class IdentityType: Model
    {
        [Column("Type_ID")]
        [Key]
        public int TypeID{ get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please fill in a Description")]
        public string Description { get; set; }
    }
}