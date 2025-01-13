using CMDB.API.Models;

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
        Task LogPdfFile(string pdfFile, int id);
        Task<bool> IsExisting(SubscriptionDTO subscription);
        Task<IEnumerable<SubscriptionDTO>> ListAllFreeSubscriptions(string category);
    }
}
