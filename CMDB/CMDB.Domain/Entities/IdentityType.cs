using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CMDB.Domain.Entities
{
    [Table("identitytype")]
    public class IdentityType : Model
    {
        [Key]
        public int TypeID { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please fill in a Description")]
        public string Description { get; set; }

        public virtual ICollection<Identity> Identities { get; set; }
    }
}