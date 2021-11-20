using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    public class IdenAccount
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please select an Identity")]
        [DisplayName("Identity")]
        public Identity Identity { get; set; }
        [Required(ErrorMessage = "Please select an Account")]
        [DisplayName("Account")]
        public Account Account { get; set; }
        [Required(ErrorMessage = "Please fill in a Valid From date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("From")]
        public DateTime ValidFrom { get; set; }
        [Required(ErrorMessage = "Please fill in a Valid until date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Until")]
        public DateTime ValidUntil { get; set; }

        public int? IdentityId { get; set; }
        public int? AccountId { get; set; }
    }
}