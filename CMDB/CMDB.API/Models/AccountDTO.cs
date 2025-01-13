using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class AccountDTO : ModelDTO
    {
        public int AccID { get; set; }
        public int? TypeId { get; set; }
        public int? ApplicationId { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public required string UserID { get; set; }
        public required ApplicationDTO Application { get; set; }
        public required TypeDTO Type { get; set; }
        public ICollection<IdenAccountDTO> Identities { get; set; }
        
        public AccountDTO()
        {
            Identities = new List<IdenAccountDTO>();
        }
    }
}
