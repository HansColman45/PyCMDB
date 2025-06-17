namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// DTO of Subscription
    /// </summary>
    public class SubscriptionDTO : ModelDTO
    {
        /// <summary>
        /// The Id of the Subscription
        /// </summary>
        public int SubscriptionId { get; set; }
        /// <summary>
        /// The SubbscriptionType of the Subscription
        /// </summary>
        public SubscriptionTypeDTO SubscriptionType { get; set; }
        /// <summary>
        /// The phone number of the Subscription
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// The linked Identity of the Subscription
        /// </summary>
        public IdentityDTO Identity { get; set; }
        /// <summary>
        /// The linked mobile of the Subscription
        /// </summary>
        public MobileDTO Mobile { get; set; }
        /// <summary>
        /// The Id of the linked Identity of the Subscription
        /// </summary>
        public int? IdentityId { get; set; }
        /// <summary>
        /// The Id of the linked Mobile of the Subscription
        /// </summary>
        public int? MobileId { get; set; }
    }
}
