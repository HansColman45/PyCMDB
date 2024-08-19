using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Domain.Responses
{
    public class AdminDetailResponce
    {
        public int AdminId { get; set; }
        public string UserId { get; set; }
        public string Active { get; set; }
        public int Level { get; set; }
    }
}
