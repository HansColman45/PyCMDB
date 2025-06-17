using System;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// The LogDTO class is used to represent a log entry in the system.
    /// </summary>
    public class LogDTO
    {
        /// <summary>
        /// The Id of the log entry
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// The date the log entry was created
        /// </summary>
        public DateTime LogDate { get; set; }
        /// <summary>
        /// The text of the log entry
        /// </summary>
        public string LogText { get; set; }
    }
}
