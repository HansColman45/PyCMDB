using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface ISubscriptionTypeRepository
    {
        Task<IEnumerable<SubscriptionTypeDTO>> GetAll();
        Task<IEnumerable<SubscriptionTypeDTO>> GetAll(string search);
        Task<SubscriptionTypeDTO> GetById(int id);
        SubscriptionTypeDTO Create(SubscriptionTypeDTO subscriptionDTO);
        Task<SubscriptionTypeDTO> Update(SubscriptionTypeDTO subscriptionDTO);
        Task<SubscriptionTypeDTO> Deactivate(SubscriptionTypeDTO subscriptionDTO, string reason);
        Task<SubscriptionTypeDTO> Activate(SubscriptionTypeDTO subscriptionDTO);
        Task<bool> IsExisting(SubscriptionTypeDTO subscriptionType);
    }
}
