using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Models
{
    [Table("role_perm")]
    public class RolePerm
    {
        [Key]
        [Column("role_perm_id")]
        public int RolePermId { get; set; }
        [Required(ErrorMessage = "Please select a level")]
        public int Level { get; set; }
        [Required(ErrorMessage = "Please select a Permision")]
        public Permissions Permission { get; set; }
        [Required(ErrorMessage = "Please select a Menu item")]
        public Menu Menu { get; set; }
    }
}
