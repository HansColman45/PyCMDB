using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class Role : Model
    {
        [Key]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Please fill in a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please fill in a description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select a type")]
        public RoleType Type { get; set; }
    }
}
