using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Domain.Entities
{
    public class Account : Model
    {
        public Account()
        {
            this.Identities = new List<IdenAccount>();
            Admins = new List<Admin>();
        }
        public int AccID { get; set; }
        public int? TypeId { get; set; }
        public int? ApplicationId { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        [Required(ErrorMessage = "Please select a type")]
        public AccountType Type { get; set; }
        [Required(ErrorMessage = "Please select an application")]
        public Application Application { get; set; }

        public virtual ICollection<IdenAccount> Identities { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
    }
}
