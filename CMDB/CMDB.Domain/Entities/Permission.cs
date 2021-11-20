using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Domain.Entities
{
    public class Permission
    {
        public Permission()
        {
            Logs = new List<Log>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill in a permision")]
        public string Rights { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<RolePerm> Roles { get; set; }
    }
}
