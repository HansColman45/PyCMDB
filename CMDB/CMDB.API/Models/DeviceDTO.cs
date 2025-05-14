using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    /// <summary>
    /// DTO of the Device
    /// </summary>
    public class DeviceDTO : ModelDTO
    {
        /// <summary>
        /// The AssetTag of the device
        /// </summary>
        [Required(ErrorMessage = "Please enter a Assettag")]
        public string AssetTag { get; set; }
        /// <summary>
        /// The serialNumber of the device
        /// </summary>
        [Required(ErrorMessage = "Please fill in a Serial number")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// The AssetCategory of the device
        /// </summary>
        public AssetCategoryDTO Category { get; set; }
        /// <summary>
        /// The AssetType of the device
        /// </summary>
        public AssetTypeDTO AssetType { get; set; }
        /// <summary>
        /// The MAC address of the device
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// The amount of RAM of the device
        /// </summary>
        public string RAM { get; set; }
        /// <summary>
        /// The linked Identity
        /// </summary>
        public IdentityDTO Identity { get; set; }
        /// <summary>
        /// The linked Kensington
        /// </summary>
        public KensingtonDTO Kensington { get; set; }
    }
}
