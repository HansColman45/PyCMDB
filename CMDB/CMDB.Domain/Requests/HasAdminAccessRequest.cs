namespace CMDB.Domain.Requests
{
    /// <summary>
    /// The request to check if the admin has access to a site
    /// </summary>
    public class HasAdminAccessRequest
    {
        /// <summary>
        /// The admin ID
        /// </summary>
        public int AdminId { get; set; }
        /// <summary>
        /// The part of the site requesting acces to
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// The action the admin is trying to perform
        /// </summary>
        public string Action { get; set; }
    }
}
