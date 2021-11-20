using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class Application : Model
    {
        public int AppID { get; set; }
        [Required(ErrorMessage = "Please fill in a name")]
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}