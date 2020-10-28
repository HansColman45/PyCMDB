using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    public class Application: Model
    {
        [Column("App_ID")]
        [Key]
        public int AppID { get; set; }
        [Required(ErrorMessage = "Please fill in a name")]
        public string Name { get; set; }
    }
}