using System.Text.Json.Serialization;

namespace CMDB.API.Models
{
    public class ModelDTO
    {
        public int Active { get; set; }
        public string? DeactivateReason { get; set; }
        public int? LastModifiedAdminId { get; set; }
    }
}
