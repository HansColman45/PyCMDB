using System;
using System.Net;

namespace CMDB.Domain.CustomExeptions
{
    /// <summary>
    /// This exception is thrown when a request to an API does not return a success code.
    /// </summary>
    public class NotAValidSuccessCode : Exception
    {
        private NotAValidSuccessCode() : base()
        {
        }
        /// <summary>
        /// This exception is thrown when a request to an API does not return a success code.
        /// </summary>
        /// <param name="url">The url we are trying to access</param>
        /// <param name="statusCode">The error code</param>
        public NotAValidSuccessCode(string url, HttpStatusCode statusCode) : base($"The {url} did not respond with a succes code, the code was {statusCode}")
        {
        }
    }
}
