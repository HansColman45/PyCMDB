using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders
{
    class LogBuilder : GenericBogusEntityBuilder<Log>
    {
        public LogBuilder()
        {
            SetDefaultRules((f, l) =>
            {
                l.LogDate = DateTime.Now;
            });
        }
    }
}
