using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CMDB.Domain.Entities
{
    public class AccountType : GeneralType
    {
        public AccountType()
        {
            Accounts = new List<Account>();
        }
        [JsonIgnore]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}