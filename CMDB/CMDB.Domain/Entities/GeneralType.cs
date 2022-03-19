using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Domain.Entities
{
    public class GeneralType :Model
    {
        [Key]
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please fill in a Description")]
        public string Description { get; set; }

    }
}
