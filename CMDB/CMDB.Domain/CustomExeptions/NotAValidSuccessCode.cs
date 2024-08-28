using System;
using System.Net;

namespace CMDB.Domain.CustomExeptions
{
    public class NotAValidSuccessCode : Exception
    {
        public NotAValidSuccessCode(string url, HttpStatusCode statusCode) : base($"The {url} did not respond with a succes code, the code was {statusCode}")
        {
        }
    }
}
