using System.Collections.Generic;

namespace CMDB.Domain.Requests
{
    public class AssignDeviceRequest
    {
        public required int IdentityId { get; set; }
        public List<string> AssetTags { get; set;}
    }
}
