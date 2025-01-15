using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class IdenAccountDTO
    {
        public int Id { get; set; }
        public IdentityDTO? Identity { get; set; }
        public AccountDTO? Account { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public required DateTime ValidFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public required DateTime ValidUntil { get; set; }
    }
}
