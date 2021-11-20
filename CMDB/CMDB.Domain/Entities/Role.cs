using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class Role : Model
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RoleType Type { get; set; }
    }
}
