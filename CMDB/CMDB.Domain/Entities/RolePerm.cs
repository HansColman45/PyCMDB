using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Domain.Entities
{
    [Table("role_perm")]
    public class RolePerm
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select a level")]
        public int Level { get; set; }
        [Required(ErrorMessage = "Please select a Permision")]
        public Permission Permission { get; set; }
        [Required(ErrorMessage = "Please select a Menu item")]
        public Menu Menu { get; set; }

        public int? MenuId { get; set; }
        public int? PermissionId { get; set; }
    }
}
