using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    public class Kensington : Model
    {
        [Key]
        public int KeyID { get; set; }
        [Required(ErrorMessage = "Please select a type")]
        public AssetType Type { get; set; }
        [Required(ErrorMessage = "Please enter a serial number")]
        public string SerialNumber { get; set; }
        public AssetCategory Category { get; set; }
        public Laptop Laptop { get; set; }
        public Desktop Desktop { get; set; }
        public Docking Docking { get; set; }
        public Screen Screen { get; set; }
        public int AmountOfKeys { get; set; }
        public bool HasLock { get; set; }
        public int? TypeId { get; set; }
        public int? CategoryId { get; set; }
    }
}