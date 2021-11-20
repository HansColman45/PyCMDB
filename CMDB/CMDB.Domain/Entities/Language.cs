using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CMDB.Domain.Entities
{
    public class Language : Model
    {
        [Key]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please fill a description")]
        public string Description { get; set; }

        public virtual ICollection<Identity> Identities { get; set; }

    }
}