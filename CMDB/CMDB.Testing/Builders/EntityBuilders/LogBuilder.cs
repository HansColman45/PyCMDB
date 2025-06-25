using CMDB.Domain.Entities;
using System;

namespace CMDB.Testing.Builders.EntityBuilders
{
    class LogBuilder : GenericBogusEntityBuilder<Log>
    {
        public LogBuilder()
        {
            SetDefaultRules((f, l) =>
            {
                l.LogDate = DateTime.UtcNow;
            });
        }
    }
}
