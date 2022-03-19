using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class AccountType : GeneralType
    {
        public AccountType()
        {
            Accounts = new List<Account>();
        }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}