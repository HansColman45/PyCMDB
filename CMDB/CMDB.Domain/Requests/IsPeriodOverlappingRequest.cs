using System;

namespace CMDB.Domain.Requests
{
    public class IsPeriodOverlappingRequest
    {
        public required int IdentityId { get; set; }
        public required int AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
