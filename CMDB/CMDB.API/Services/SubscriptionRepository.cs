using CMDB.API.Models;
using CMDB.Infrastructure;

namespace CMDB.API.Services
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<SubscriptionDTO>> GetAll();
        Task<IEnumerable<SubscriptionDTO>> GetAll(string search);
        Task<SubscriptionDTO?> GetById(int id);
        SubscriptionDTO Create(SubscriptionDTO subscription);
        Task<SubscriptionDTO> Update(SubscriptionDTO subscription);
        Task<SubscriptionDTO> Delete(SubscriptionDTO subscription, string reason);
        Task<SubscriptionDTO> Activate(SubscriptionDTO subscription);
    }
    public class SubscriptionRepository : GenericRepository, ISubscriptionRepository
    {
        private readonly string table = "subscription";
        public SubscriptionRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public Task<IEnumerable<SubscriptionDTO>> GetAll()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<SubscriptionDTO>> GetAll(string search)
        {
            throw new NotImplementedException();
        }
        public Task<SubscriptionDTO?> GetById(int id)
        {
            throw new NotImplementedException();
        }
        public Task<SubscriptionDTO> Activate(SubscriptionDTO subscription)
        {
            throw new NotImplementedException();
        }
        public SubscriptionDTO Create(SubscriptionDTO subscription)
        {
            throw new NotImplementedException();
        }
        public Task<SubscriptionDTO> Delete(SubscriptionDTO subscription, string reason)
        {
            throw new NotImplementedException();
        }
        public Task<SubscriptionDTO> Update(SubscriptionDTO subscription)
        {
            throw new NotImplementedException();
        }
    }
}
