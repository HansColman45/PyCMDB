using System.ComponentModel.DataAnnotations;

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
