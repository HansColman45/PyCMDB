using System.Collections.Generic;

namespace CMDB.Domain.Entities
{
    public class RoleType : GeneralType
    {
        public ICollection<Role> Roles { get; set; }
    }
}