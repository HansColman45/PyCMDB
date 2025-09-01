using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// The AccountDTO class is used to represent an account in the system.
    /// </summary>
    public class AccountDTO : ModelDTO
    {
        /// <summary>
        /// The Id of the account
        /// </summary>
        public int AccID { get; set; }
        /// <summary>
        /// The TypeId of the account
        /// </summary>
        public int? TypeId { get; set; }
        /// <summary>
        /// The linked ApplicationId of the account
        /// </summary>
        public int? ApplicationId { get; set; }
        /// <summary>
        /// The UserID of the account
        /// </summary>
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        /// <summary>
        /// The linked Application of the account
        /// </summary>
        public ApplicationDTO Application { get; set; }
        /// <summary>
        /// The linked AccountType of the account
        /// </summary>
        public TypeDTO Type { get; set; }
        /// <summary>
        /// The linked Identities of the account
        /// </summary>
        public ICollection<IdenAccountDTO> Identities { get; set; }
        /// <summary>
        /// Constructor for the AccountDTO class.
        /// </summary>
        public AccountDTO()
        {
            Identities = new List<IdenAccountDTO>();
        }
    }
}
