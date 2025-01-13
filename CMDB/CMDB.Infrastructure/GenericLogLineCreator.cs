namespace CMDB.Infrastructure
{
    public static class GenericLogLineCreator
    {
        /// <summary>
        /// This function will generate the Log line for the update operation
        /// </summary>
        /// <param name="field">The field we are updating</param>
        /// <param name="oldValue">The Old value of that field</param>
        /// <param name="newValue">The new value of that field</param>
        /// <param name="updator">The person that did the update</param>
        /// <param name="table">The table where the ypdate is done</param>
        /// <returns></returns>
        public static string UpdateLogLine(string field, string oldValue, string newValue, string updator, string table)
        {
            return $"The {field} has been changed from {oldValue} to {newValue} by {updator} in table {table}";
        }
        /// <summary>
        /// This function will generate the Log line for the creation operation
        /// </summary>
        /// <param name="value">The info about the thing that will be created</param>
        /// <param name="creator">The person that did the creation</param>
        /// <param name="table">The table where the creation took place</param>
        /// <returns></returns>
        public static string CreateLogLine(string value, string creator, string table)
        {
            return $"The {value} is created by {creator} in table {table}";
        }
        /// <summary>
        /// This function will generate the Log line for the deletion operation
        /// </summary>
        /// <param name="value">The info about the thing that will be deleted</param>
        /// <param name="deleter">The person that did the delete operation</param>
        /// <param name="reason">The Reason that the deleter entered</param>
        /// <param name="table">The table where the deletion took place</param>
        /// <returns></returns>
        public static string DeleteLogLine(string value, string deleter, string reason, string table)
        {
            return $"The {value} is deleted due to {reason} by {deleter} in table {table}";
        }
        /// <summary>
        /// This function will generate the Log line for the activation operation
        /// </summary>
        /// <param name="value">The info about the thing that will be activated</param>
        /// <param name="activator">The person that did the activation</param>
        /// <param name="table">The table where the activation took place</param>
        /// <returns></returns>
        public static string ActivateLogLine(string value, string activator, string table)
        {
            return $"The {value} is activated by {activator} in table {table}";
        }
        /// <summary>
        /// This function will generate the Log line for when an account is assigned to an Identity
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <param name="IdentityInfo"></param>
        /// <param name="assingee"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string AssingAccount2IdenityLogLine(string accountInfo, string IdentityInfo, string assingee, string table)
        {
            return $"The {accountInfo} is assigned to {IdentityInfo} by {assingee} in table {table}";
        }
        /// <summary>
        /// This function will generate the Log line for when an account is released from an Identity
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <param name="IdentityInfo"></param>
        /// <param name="releaser"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string ReleaseAccountFromIdentityLogLine(string accountInfo, string IdentityInfo, string releaser, string table)
        {
            return $"The {accountInfo} is released from {IdentityInfo} by {releaser} in table {table}";
        }
        /// <summary>
        /// This function will generate the Log line for when a device is assigned to an Identity
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="IdentityInfo"></param>
        /// <param name="assignee"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string AssingDevice2IdenityLogLine(string deviceInfo, string IdentityInfo, string assignee, string table)
        {
            return $"The {deviceInfo} is assigned to {IdentityInfo} by {assignee} in table {table}";
        }
        /// <summary>
        /// This function will generate the Log line for when a device is released from an Identity
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="IdentityInfo"></param>
        /// <param name="releaser"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string ReleaseDeviceFromIdentityLogLine(string deviceInfo, string IdentityInfo, string releaser, string table)
        {
            return $"The {deviceInfo} is released from {IdentityInfo} by {releaser} in table {table}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdentityInfo"></param>
        /// <param name="deviceInfo"></param>
        /// <param name="releaser"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string ReleaseIdentityFromDeviceLogLine(string IdentityInfo, string deviceInfo, string releaser, string table)
        {
            return $"The {IdentityInfo} is released from {deviceInfo} by {releaser} in table {table}";
        }
        /// <summary>
        /// This function will generate the PDF log line
        /// </summary>
        /// <param name="pdfFile">The pdf File Path</param>
        /// <returns></returns>
        public static string LogPDFFileLine(string pdfFile)
        {
            pdfFile = pdfFile[36..];
            pdfFile = pdfFile.Replace('\\', '/');
            pdfFile = "../.." + pdfFile;
            return $"Please find the PDFFile <a href='{pdfFile}' target='_blank'>here</a>";
        }
    }
}
