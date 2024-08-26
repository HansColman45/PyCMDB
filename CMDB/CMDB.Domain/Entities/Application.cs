using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CMDB.Domain.Entities
{
    public class Application : Model
    {
        public Application()
        {
            Accounts = new List<Account>();
        }
        public int AppID { get; set; }
        [Required(ErrorMessage = "Please fill in a name")]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}