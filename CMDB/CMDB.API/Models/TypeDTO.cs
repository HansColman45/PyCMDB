using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    /// <summary>
    /// Generic Type used for AccountType and IdentityType
    /// </summary>
    public class TypeDTO : ModelDTO
    {
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please fill in a Description")]
        public string Description { get; set; }
    }
}
