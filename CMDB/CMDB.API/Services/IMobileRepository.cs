using CMDB.API.Models;
using CMDB.Domain.Requests;

namespace CMDB.API.Services
{
    /// <summary>
    /// The interface for the Mobile repository
    /// </summary>
    public interface IMobileRepository 
    {
        /// <summary>
        /// This will reurn a list of all mobiles
        /// </summary>
        /// <returns>List of <see cref="MobileDTO"/></returns>
        Task<IEnumerable<MobileDTO>> GetAll();
        /// <summary>
        /// This will return a list of all mobiles matching the search string
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns>List of <see cref="MobileDTO"/></returns>
        Task<IEnumerable<MobileDTO>> GetAll(string searchStr);
        /// <summary>
        /// This will retunr the mobile with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="MobileDTO"/></returns>
        Task<MobileDTO> GetById(int id);
        /// <summary>
        /// This will create a new mobile
        /// </summary>
        /// <param name="mobileDTO"><see cref="MobileDTO"/></param>
        /// <returns></returns>
        MobileDTO Create(MobileDTO mobileDTO);
        /// <summary>
        /// This will update the existing mobile
        /// </summary>
        /// <param name="mobileDTO"><see cref="MobileDTO"/></param>
        /// <returns></returns>
        Task<MobileDTO> Update(MobileDTO mobileDTO);
        /// <summary>
        /// This will deactivate the given mobile
        /// </summary>
        /// <param name="mobileDTO"><see cref="MobileDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<MobileDTO> Delete(MobileDTO mobileDTO, string reason);
        /// <summary>
        /// This will activate the given mobile
        /// </summary>
        /// <param name="mobileDTO"><see cref="MobileDTO"/></param>
        /// <returns></returns>
        Task<MobileDTO> Activate(MobileDTO mobileDTO);
        /// <summary>
        /// This will return a list of all mobiles that are free
        /// </summary>
        /// <param name="sitePart"></param>
        /// <returns>List of <see cref="MobileDTO"/></returns>
        Task<IEnumerable<MobileDTO>> ListAllFreeMobiles(string sitePart);
        /// <summary>
        /// This will check if the given mobile is existing
        /// </summary>
        /// <param name="mobileDTO"><see cref="MobileDTO"/></param>
        /// <returns></returns>
        Task<bool> IsMobileExisting(MobileDTO mobileDTO);
        /// <summary>
        /// This will assign the given mobile to the given identity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AssignIdentity(AssignMobileRequest request);
        /// <summary>
        /// This will release the given mobile from the given identity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task ReleaseIdentity(AssignMobileRequest request);
        /// <summary>
        /// This will assign the given mobile to the given subscription
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AssignSubscription(AssignMobileSubscriptionRequest request);
        /// <summary>
        /// This will log the creation of a PDF file
        /// </summary>
        /// <param name="pdfFile"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task LogPdfFile(string pdfFile, int id);
        /// <summary>
        /// This will release the given mobile from the given subscription
        /// </summary>
        /// <param name="assignMobileSubscription"></param>
        /// <returns></returns>
        Task ReleaseSubscription(AssignMobileSubscriptionRequest assignMobileSubscription);
    }
}
