using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class AssetType : Model
    {
        public AssetType()
        {
            Devices = new List<Device>();
            Kensingtons = new List<Kensington>();
            Mobiles = new List<Mobile>();
        }
        [Key]
        public int TypeID { get; set; }
        [Required(ErrorMessage = "Please fill in a vendor")]
        public string Vendor { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required]
        public AssetCategory Category { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<Device> Devices { get; set; }
        public ICollection<Kensington> Kensingtons { get; set; }
        public ICollection<Mobile> Mobiles { get; set; }

        public override string ToString()
        {
            return Vendor + " " + Type;
        }
    }
}