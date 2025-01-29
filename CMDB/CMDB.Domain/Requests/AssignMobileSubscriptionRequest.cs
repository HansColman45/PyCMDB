using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Domain.Requests
{
    public class AssignMobileSubscriptionRequest
    {
        public required int MobileId { get; set; }
        public int SubscriptionId { get; set; }
    }
}
