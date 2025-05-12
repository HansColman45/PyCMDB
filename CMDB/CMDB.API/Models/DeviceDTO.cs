using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    public class DeviceDTO : ModelDTO
    {
        [Required(ErrorMessage = "Please enter a Assettag")]
        public string AssetTag { get; set; }
        [Required(ErrorMessage = "Please fill in a Serial number")]
        public string SerialNumber { get; set; }
        public AssetCategoryDTO Category { get; set; }
        public AssetTypeDTO AssetType { get; set; }
        public string MAC { get; set; }
        public string RAM { get; set; }
        public IdentityDTO Identity { get; set; }
        public KensingtonDTO Kensington { get; set; }
    }
}
