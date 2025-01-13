using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class LanguageDTO
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
    }
}