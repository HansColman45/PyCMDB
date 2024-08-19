using System;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Requests
{
    public class AssignAccountRequest
    {
        public int IdenityId { get; set; }
        public int AccountId { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
