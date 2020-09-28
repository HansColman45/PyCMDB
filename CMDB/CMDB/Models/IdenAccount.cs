using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Models
{
    [Table("IdenAccount")]
    public class IdenAccount
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public Identity Identity { get; set; }
        [Required]
        public Account Account { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
