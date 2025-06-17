using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// Generic Type used for AccountType and IdentityType
    /// </summary>
    public class TypeDTO : ModelDTO
    {
        /// <summary>
        /// The Id of the type
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// The type of the type
        /// </summary>
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        /// <summary>
        /// The description of the type
        /// </summary>
        [Required(ErrorMessage = "Please fill in a Description")]
        public string Description { get; set; }
    }
}
