using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Models
{
    public class Permissions
    {
        [Key]
        [Column("perm_id")]
        public int PermId { get; set; }
        [Required(ErrorMessage = "Please fill in a permision")]
        public string Permistion { get; set; }
        public string Description { get; set; }
    }
}
