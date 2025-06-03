using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    [Table("role_perm")]
    public class RolePerm
    {
        public RolePerm()
        {
            Logs = new List<Log>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select a level")]
        public int Level { get; set; }
        [Required(ErrorMessage = "Please select a Permision")]
        public Permission Permission { get; set; }
        [Required(ErrorMessage = "Please select a Menu item")]
        public Menu Menu { get; set; }
        public Admin LastModifiedAdmin { get; set; }
        public int LastModifiedAdminId { get; set; }

        public int? MenuId { get; set; }
        public int? PermissionId { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
