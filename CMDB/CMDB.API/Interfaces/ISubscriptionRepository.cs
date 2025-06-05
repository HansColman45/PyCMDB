using CMDB.API.Models;
using CMDB.Domain.Requests;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Interface for Subscription Repository
    /// </summary>
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// This will return a list of all subscriptions
        /// </summary>
        /// <returns>List of <see cref="SubscriptionDTO"/></returns>
        Task<IEnumerable<SubscriptionDTO>> GetAll();
        /// <summary>
        /// This will return a list of all subscriptions matching the search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns>list of <see cref="SubscriptionDTO"/></returns>
        Task<IEnumerable<SubscriptionDTO>> GetAll(string search);
        /// <summary>
        /// This function will return a subscription by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="SubscriptionDTO"/></returns>
        Task<SubscriptionDTO> GetById(int id);
        /// <summary>
        /// This will create a subscription
        /// </summary>
        /// <param name="subscription"><see cref="SubscriptionDTO"/></param>
        /// <returns></returns>
        SubscriptionDTO Create(SubscriptionDTO subscription);
        /// <summary>
        /// This will update an existing subscription
        /// </summary>
        /// <param name="subscription"><see cref="SubscriptionDTO"/></param>
        /// <returns></returns>
        Task<SubscriptionDTO> Update(SubscriptionDTO subscription);
        /// <summary>
        /// This will delete a subscription
        /// </summary>
        /// <param name="subscription"><see cref="SubscriptionDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<SubscriptionDTO> Delete(SubscriptionDTO subscription, string reason);
        /// <summary>
        /// This will activate a subscription
        /// </summary>
        /// <param name="subscription"><see cref="SubscriptionDTO"/></param>
        /// <returns></returns>
        Task<SubscriptionDTO> Activate(SubscriptionDTO subscription);
        /// <summary>
        /// This will log the creation of a PDF file
        /// </summary>
        /// <param name="pdfFile"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task LogPdfFile(string pdfFile, int id);
        /// <summary>
        /// This function will check if a subscription already exists
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        Task<bool> IsExisting(SubscriptionDTO subscription);
        /// <summary>
        /// This function will return all free Subscriptions
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<IEnumerable<SubscriptionDTO>> ListAllFreeSubscriptions(string category);
        /// <summary>
        /// This function will assign an Identity to the subscription
        /// </summary>
        /// <param name="subscriptionRequest"></param>
        /// <returns></returns>
        Task AssignIdentity(AssignInternetSubscriptionRequest subscriptionRequest);
        /// <summary>
        /// This function will release an Identity from the subscription
        /// </summary>
        /// <param name="subscriptionRequest"></param>
        /// <returns></returns>
        Task ReleaseIdentity(AssignInternetSubscriptionRequest subscriptionRequest);
        /// <summary>
        /// This function will assign a moblie to the subscription
        /// </summary>
        /// <param name="assignMobileRequest"></param>
        /// <returns></returns>
        Task AssignMobile(AssignMobileSubscriptionRequest assignMobileRequest);
        /// <summary>
        /// This function will release a mobile from the subscription
        /// </summary>
        /// <param name="releaseMobileRequest"></param>
        /// <returns></returns>
        Task ReleaseMobile(AssignMobileSubscriptionRequest releaseMobileRequest);
    }
}
