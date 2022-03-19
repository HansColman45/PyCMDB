namespace CMDB.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class RoleType : GeneralType
    {
        public ICollection<Role> Roles { get; set; }
    }
}