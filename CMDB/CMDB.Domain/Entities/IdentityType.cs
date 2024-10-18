using System.Collections.Generic;

namespace CMDB.Domain.Entities
{
    public class IdentityType : GeneralType
    {
        public virtual ICollection<Identity> Identities { get; set; }
    }
}