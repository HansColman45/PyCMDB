using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    /// <summary>
    /// DTO of SubscriptionType
    /// </summary>
    public class SubscriptionTypeDTO : ModelDTO
    {
        /// <summary>
        /// The Id of the SubscriptionType
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The type of the SubscriptionType
        /// </summary>
        [Required(ErrorMessage = "Please enter a Type")]
        public string Type { get; set; }
        /// <summary>
        /// The description of the SubscriptionType
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The provider of the SubscriptionType
        /// </summary>
        [Required(ErrorMessage = "Please enter a provider")]
        public string Provider { get; set; }
        /// <summary>
        /// The AssetCategory of the SubscriptionType
        /// </summary>
        [Required(ErrorMessage = "Please select a Category")]
        public AssetCategoryDTO AssetCategory { get; set; }
        /// <summary>
        /// generates a readable string of the SubscriptionType
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public override string ToString()
        {
            return $"{Provider} {Type} {Description}";
        }
    }
}
