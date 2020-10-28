using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Models
{
    public class Subscription:Model
    {
        [Key]
        [Column("Sub_ID")]
        public int SubID { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string PhoneNumber { get; set; }
        public Identity Identity { get; set; }
        public Mobile Mobile { get; set; }
        public AssetCategory Category { get; set; }
    }
}
