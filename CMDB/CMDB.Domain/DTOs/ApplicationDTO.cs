using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// The ApplicationDTO class is used to represent an application in the system.
    /// </summary>
    public class ApplicationDTO : ModelDTO
    {
        /// <summary>
        /// The Id of the application
        /// </summary>
        public int AppID { get; set; }
        /// <summary>
        /// The name of that application
        /// </summary>
        [Required(ErrorMessage = "Please fill in a name")]
        public string Name { get; set; }
    }
}
