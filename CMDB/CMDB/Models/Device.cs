using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    [Table("asset")]
    public class Device: Model
    {
        [Key]
        [Required(ErrorMessage = "Please enter a Assettag")]
        public string AssetTag { get; set; }
        [Required(ErrorMessage = "Please fill in a Serial number")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "Please select an type")]
        public AssetType Type { get; set; }
        public AssetCategory Category { get; set; }
        public Identity Identity { get; set; }
    }
    public class Laptop : Device
    {
        public string MAC { get; set; }
        [Required(ErrorMessage = "Please select the amount of RAM")]
        public string RAM { get; set; }
    }
    public class Desktop : Device
    {
        public string MAC { get; set; }
        [Required(ErrorMessage = "Please select the amount of RAM")]
        public string RAM { get; set; }
    }
    public class Monitor : Device
    {
        
    }
    public class Docking : Device
    {

    }
    public class Token: Device
    {

    }
}