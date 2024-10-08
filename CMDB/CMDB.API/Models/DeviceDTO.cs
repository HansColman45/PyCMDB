using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class DeviceDTO : ModelDTO
    {
        [Required(ErrorMessage = "Please enter a Assettag")]
        public required string AssetTag { get; set; }
        [Required(ErrorMessage = "Please fill in a Serial number")]
        public required string SerialNumber { get; set; }
        public required AssetCategoryDTO Category { get; set; }
        public required AssetTypeDTO AssetType { get; set; }
    }
}
