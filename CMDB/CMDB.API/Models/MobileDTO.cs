using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    /// <summary>
    /// The DTO file for Mobile <see cref="CMDB.Domain.Entities.Mobile"/>
    /// </summary>
    public class MobileDTO : ModelDTO
    {
        /// <summary>
        /// The primary key for the Mobile
        /// </summary>
        public int MobileId { get; set; }
        /// <summary>
        /// The IMei number of the Mobile
        /// </summary>
        [Required]
        public long IMEI { get; set; }
        /// <summary>
        /// The AssetType of the Mobile
        /// </summary>
        [Required]
        public AssetTypeDTO MobileType { get; set; }
        /// <summary>
        /// The linked Identity of the Mobile
        /// </summary>
        public IdentityDTO Identity { get; set; }
        /// <summary>
        /// The linked Subscription of the Mobile
        /// </summary>
        public SubscriptionDTO Subscription { get; set; }
        /// <summary>
        /// The Id of the linked Identity of the Mobile
        /// </summary>
        public int IdentityId { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public MobileDTO()
        {
        }
    }
}
