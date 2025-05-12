using System.Collections.Generic;

namespace CMDB.Models
{
    /// <summary>
    /// ValidationError is a custom exception class that inherits from System.Exception.
    /// </summary>
    public class ValidationError: System.Exception
    {
        /// <summary>
        /// List of error messages.
        /// </summary>
        public List<string> Errors;
        /// <summary>
        /// ValidationError constructor initializes the Errors property with a list of error messages.
        /// </summary>
        /// <param name="errors"></param>
        public ValidationError(List<string> errors)
        {
            this.Errors = errors;
        }
    }
}
