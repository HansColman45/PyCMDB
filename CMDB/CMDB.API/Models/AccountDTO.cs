using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class AccountDTO : ModelDTO
    {
        public int AccID { get; set; }
        public int? TypeId { get; set; }
        public int? ApplicationId { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        public ApplicationDTO Application { get; set; }
        public AccountTypeDTO Type { get; set; }
    }
}
