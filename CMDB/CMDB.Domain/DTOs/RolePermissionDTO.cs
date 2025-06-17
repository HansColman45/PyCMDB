using CMDB.Domain.Entities;
using System.Collections.Generic;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// DTO for RolePermission model.
    /// </summary>
    public class RolePermissionDTO
    {
        /// <summary>
        /// The Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The level 
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// The linked permission.
        /// </summary>
        public PermissionDTO Permission { get; set; }
        /// <summary>
        /// The linked menu item.
        /// </summary>
        public Menu Menu { get; set; }
        /// <summary>
        /// The AdminId that did the last modification of the model.
        /// </summary>
        public int LastModifiedAdminId { get; set; }
        /// <summary>
        /// The linked logs of the model.
        /// </summary>
        public List<LogDTO> Logs { get; set; }
        /// <summary>
        /// General constructor for the ModelDTO class.
        /// </summary>
        public RolePermissionDTO()
        {
            Logs = new List<LogDTO>();
        }
    }
}
