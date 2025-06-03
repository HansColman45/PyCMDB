using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    /// <summary>
    /// The DTO for the <see cref="Domain.Entities.Permission"/> model."/>
    /// </summary>
    public class PermissionDTO
    {
        /// <summary>
        /// The Id of the permission
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The granted right of the permission
        /// </summary>
        [Required(ErrorMessage = "Please fill in a permision")]
        public string Right { get; set; }
        /// <summary>
        /// The description of the permission
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The AdminId that did the last modification of the model.
        /// </summary>
        public int? LastModifiedAdminId { get; set; }
        /// <summary>
        /// The linked logs of the model.
        /// </summary>
        public List<LogDTO> Logs { get; set; }
        /// <summary>
        /// General constructor for the ModelDTO class.
        /// </summary>
        public PermissionDTO()
        {
            Logs = new List<LogDTO>();
        }
    }
}
