using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class TypeDTO : ModelDTO
    {
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public required string Type { get; set; }
        [Required(ErrorMessage = "Please fill in a Description")]
        public required string Description { get; set; }
    }
}
