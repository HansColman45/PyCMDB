using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class IdentityDTO : ModelDTO
    {
        public int IdenId { get; set; }
        public required string Name { get; set; }
        [EmailAddress]
        public required string EMail { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public required string UserID { get; set; }
        [Required(ErrorMessage = "Please fill in a Company")]
        public string? Company { get; set; }
        [Required(ErrorMessage = "Please select a Language")]
        public LanguageDTO? Language { get; set; }
        [Required(ErrorMessage = "Please select a Type")]
        public TypeDTO? Type { get; set; }
        public ICollection<IdenAccountDTO> Accounts { get; set; }

        public IdentityDTO()
        {
            Accounts = new List<IdenAccountDTO>();
        }
    }
}