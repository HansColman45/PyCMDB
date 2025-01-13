using System.Collections.Generic;

namespace CMDB.Domain.Requests
{
    public class AssignMobileRequest
    {
        public required int IdentityId { get; set; }
        public List<int> MobileIds { get; set; }
    }
}
