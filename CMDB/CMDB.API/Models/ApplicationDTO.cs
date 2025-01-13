using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class ApplicationDTO : ModelDTO
    {
        public int AppID { get; set; }
        [Required(ErrorMessage = "Please fill in a name")]
        public string Name { get; set; }
    }
}
