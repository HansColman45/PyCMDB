namespace CMDB.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class RoleType : Model
    {
        [Key]
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}