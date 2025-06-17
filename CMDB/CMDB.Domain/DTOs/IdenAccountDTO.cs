using System;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// The denAccountDTO class is used to represent the link between an Identity and an Account in the system.
    /// </summary>
    public class IdenAccountDTO
    {
        /// <summary>
        /// The Id of the IdenAccountDTO
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The linked IdentityId of the IdenAccountDTO
        /// </summary>
        public IdentityDTO Identity { get; set; }
        /// <summary>
        /// The linked AccountId of the IdenAccountDTO
        /// </summary>
        public AccountDTO Account { get; set; }
        /// <summary>
        /// The From date the account is linked to the identity
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public required DateTime ValidFrom { get; set; }
        /// <summary>
        /// The date until the account is linked to the identity
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public required DateTime ValidUntil { get; set; }
    }
}
