using System;

namespace CMDB.Models
{
    /// <summary>
    /// ErrorViewModel is used to show the error page
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// The request ID
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// bool to show the request ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
