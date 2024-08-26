using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class AdminDTO : ModelDTO
    {
        public int AdminId { get; set; }
        public int? AccountId { get; set; }
        [Required(ErrorMessage = "Please select an level")]
        public int Level { get; set; }
        public string Password { get; set; }
        public DateTime DateSet { get; set; }
    }
}
