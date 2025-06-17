using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// DTO of the Kensington
    /// </summary>
    public class KensingtonDTO: ModelDTO
    {
        /// <summary>
        /// The ID of the Kensington
        /// </summary>
        public int KeyID { get; set; }
        /// <summary>
        /// The AssetType of the Kensington
        /// </summary>
        [Required(ErrorMessage = "Please select a type")]
        public AssetTypeDTO Type { get; set; }
        /// <summary>
        /// The serialNumber of the Kensington
        /// </summary>
        [Required(ErrorMessage = "Please enter a serial number")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// The linked device
        /// </summary>
        public DeviceDTO Device { get; set; }
        /// <summary>
        /// The amount of keys of the Kensington
        /// </summary>
        public int AmountOfKeys { get; set; }
        /// <summary>
        /// Indicates if the Kensington has a lock
        /// </summary>
        public bool HasLock { get; set; }
        /// <summary>
        /// The asset tag of the linked device
        /// </summary>
        public string AssetTag { get; set; }
    }
}
