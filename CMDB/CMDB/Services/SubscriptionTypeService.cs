using CMDB.Infrastructure;

namespace CMDB.Services
{
    public class SubscriptionTypeService : LogService
    {
        public SubscriptionTypeService(CMDBContext context) : base(context)
        {
        }
    }
}
