using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    [Table("asset")]
    public class Device : Model
    {
        public Device()
        {
            Keys = new List<Kensington>();
        }
        [Key]
        [Required(ErrorMessage = "Please enter a Assettag")]
        public string AssetTag { get; set; }
        [Required(ErrorMessage = "Please fill in a Serial number")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "Please select an type")]
        public AssetType Type { get; set; }
        public AssetCategory Category { get; set; }
        public Identity Identity { get; set; }

        public int? TypeId { get; set; }
        public int? CategoryId { get; set; }
        public int? IdentityId { get; set; }

        public virtual ICollection<Kensington> Keys { get; set; }
    }
}