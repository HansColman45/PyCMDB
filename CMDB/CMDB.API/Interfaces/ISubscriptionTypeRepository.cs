using CMDB.Domain.DTOs;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Repository interface for SubscriptionType
    /// </summary>
    public interface ISubscriptionTypeRepository
    {
        /// <summary>
        /// This will list all SubscriptionTypes
        /// </summary>
        /// <returns>List of <see cref="SubscriptionTypeDTO"/></returns>
        Task<IEnumerable<SubscriptionTypeDTO>> GetAll();
        /// <summary>
        /// This will list all SubscriptionTypes matching the search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns>List of <see cref="SubscriptionTypeDTO"/></returns>
        Task<IEnumerable<SubscriptionTypeDTO>> GetAll(string search);
        /// <summary>
        /// This will return a SubscriptionType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="SubscriptionTypeDTO"/></returns>
        Task<SubscriptionTypeDTO> GetById(int id);
        /// <summary>
        /// This will create a new SubscriptionType
        /// </summary>
        /// <param name="subscriptionDTO"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns></returns>
        SubscriptionTypeDTO Create(SubscriptionTypeDTO subscriptionDTO);
        /// <summary>
        /// This will update a SubscriptionType
        /// </summary>
        /// <param name="subscriptionDTO"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns></returns>
        Task<SubscriptionTypeDTO> Update(SubscriptionTypeDTO subscriptionDTO);
        /// <summary>
        /// This will delete a SubscriptionType
        /// </summary>
        /// <param name="subscriptionDTO"><see cref="SubscriptionTypeDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<SubscriptionTypeDTO> Deactivate(SubscriptionTypeDTO subscriptionDTO, string reason);
        /// <summary>
        /// This will activate a SubscriptionType
        /// </summary>
        /// <param name="subscriptionDTO"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns></returns>
        Task<SubscriptionTypeDTO> Activate(SubscriptionTypeDTO subscriptionDTO);
        /// <summary>
        /// This will check if a SubscriptionType already exists
        /// </summary>
        /// <param name="subscriptionType"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns></returns>
        Task<bool> IsExisting(SubscriptionTypeDTO subscriptionType);
    }
}
