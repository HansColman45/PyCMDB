using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class AccountType : Model
    {
        public AccountType()
        {
            Accounts = new List<Account>();
        }
        public int TypeID { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please fill in a Description")]
        public string Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}