using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    public class Kensington: Model
    {
        [Key]
        [Column("Key_ID")]
        public int KeyID { get; set; }
        [Required(ErrorMessage = "Please select a type")]
        public AssetType Type { get; set; }
        [Required(ErrorMessage = "Please enter a serial number")]
        public string SerialNumber { get; set; }
        public Device Asset { get; set; }
        public int AmountOfKeys { get; set; }
        public bool HasLock { get; set; }

    }
}