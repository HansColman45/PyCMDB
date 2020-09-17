using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Models
{
    public class ValidationError: System.Exception
    {
        public List<string> Errors;
        public ValidationError(List<string> errors)
        {
            this.Errors = errors;
        }
    }
}
