using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Domain.Requests
{
    public class AssignSubscriptionRequest
    {
        public required int IdentityId { get; set; }
        public List<int> SubscriptionIds { get; set; }
    }
}
