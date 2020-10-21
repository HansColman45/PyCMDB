using System.ComponentModel.DataAnnotations;

namespace CMDB.Models
{
    public class Language:Model
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
    }
}