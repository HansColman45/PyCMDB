using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    [Table("asset")]
    public class Device: Model
    {
        [Key]
        public string AssetTag { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public AssetType Type { get; set; }
        [Required]
        public AssetCategory Category { get; set; }
        public Identity Identity { get; set; }
    }
    public class Laptop : Device
    {
        public string MAC { get; set; }
        [Required]
        public string RAM { get; set; }
    }
    public class Desktop : Device
    {
        public string MAC { get; set; }
        [Required]
        public string RAM { get; set; }
    }
    public class Monitor : Device
    {
        
    }
    public class Docking : Device
    {

    }
}