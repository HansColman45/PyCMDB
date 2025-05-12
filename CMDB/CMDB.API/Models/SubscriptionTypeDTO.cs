using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class SubscriptionTypeDTO : ModelDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a Type")]
        public string Type { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter a provider")]
        public string Provider { get; set; }
        [Required(ErrorMessage = "Please select a Category")]
        public AssetCategoryDTO AssetCategory { get; set; }
        public override string ToString()
        {
            return $"{Provider} {Type} {Description}";
        }
    }
}
