using System.Collections.Generic;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// General ModelDTO class for all DTOs in the system.
    /// </summary>
    public class ModelDTO
    {
        /// <summary>
        /// Indicates if the model is active or inactive.
        /// </summary>
        public int Active { get; set; }
        /// <summary>
        /// The reason for deactivation of the model.
        /// </summary>
        public string DeactivateReason { get; set; }
        /// <summary>
        /// The AdminId that did the last modification of the model.
        /// </summary>
        public int? LastModifiedAdminId { get; set; }
        /// <summary>
        /// The state of the model, either "Active" or "Inactive".
        /// </summary>
        public string State => Active == 1 ? "Active" : "Inactive";
        /// <summary>
        /// The linked logs of the model.
        /// </summary>
        public List<LogDTO> Logs { get; set; }
        /// <summary>
        /// General constructor for the ModelDTO class.
        /// </summary>
        public ModelDTO()
        {
            Logs = new List<LogDTO>();
        }
    }
}
