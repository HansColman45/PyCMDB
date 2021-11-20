using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    public class AssetType : Model
    {
        [Key]
        public int TypeID { get; set; }
        [Required(ErrorMessage = "Please fill in a vendor")]
        public string Vendor { get; set; }
        [Required(ErrorMessage = "Please fill in a Type")]
        public string Type { get; set; }
        [Required]
        public AssetCategory Category { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<Laptop> Laptops { get; set; }
        public ICollection<Desktop> Desktops { get; set; }
        public ICollection<Docking> Dockings { get; set; }
        public ICollection<Kensington> Kensingtons { get; set; }
        public ICollection<Screen> Screens { get; set; }
        public ICollection<Mobile> Mobiles { get; set; }
        public ICollection<Token> Tokens { get; set; }

        public override string ToString()
        {
            return Vendor + " " + Type;
        }
    }
}