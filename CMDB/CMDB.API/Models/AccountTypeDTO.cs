using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class AccountTypeDTO : ModelDTO
    {
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please fill in a Description")]
        public string Description { get; set; }
    }
}
