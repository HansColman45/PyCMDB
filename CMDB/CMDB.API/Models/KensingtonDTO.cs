using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class KensingtonDTO: ModelDTO
    {
        public int KeyID { get; set; }
        [Required(ErrorMessage = "Please select a type")]
        public AssetTypeDTO Type { get; set; }
        [Required(ErrorMessage = "Please enter a serial number")]
        public string SerialNumber { get; set; }
        public AssetCategoryDTO Category { get; set; }
        public DeviceDTO Device { get; set; }
        public int AmountOfKeys { get; set; }
        public bool HasLock { get; set; }
    }
}
