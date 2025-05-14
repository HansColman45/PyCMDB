using CMDB.API.Models;

namespace CMDB.API.Services
{
    /// <summary>
    /// This is the interface of the Kensington repo
    /// </summary>
    public interface IKensingtonRepository
    {
        /// <summary>
        /// This will return a list with all Kensingtons
        /// </summary>
        /// <returns>list of <see cref="KensingtonDTO"/></returns>
        Task<List<KensingtonDTO>> ListAll();
        /// <summary>
        /// This will return a list with all Kensingtons matching the search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns>List of <see cref="KensingtonDTO"/></returns>
        Task<List<KensingtonDTO>> ListAll(string search);
        /// <summary>
        /// This will return the Kensington with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="KensingtonDTO"/></returns>
        Task<KensingtonDTO> GetById(int id);
        /// <summary>
        /// This will create a new Kensington
        /// </summary>
        /// <param name="key"><see cref="KensingtonDTO"/></param>
        /// <returns></returns>
        KensingtonDTO Create(KensingtonDTO key);
        /// <summary>
        /// This will update an existing kensington
        /// </summary>
        /// <param name="key"><see cref="KensingtonDTO"/></param>
        /// <returns></returns>
        Task<KensingtonDTO> Update(KensingtonDTO key);
        /// <summary>
        /// This will deactivate a Kensington
        /// </summary>
        /// <param name="key"><see cref="KensingtonDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<KensingtonDTO> DeActivate(KensingtonDTO key, string reason);
        /// <summary>
        /// This will activate a Kensington
        /// </summary>
        /// <param name="key"><see cref="KensingtonDTO"/></param>
        /// <returns></returns>
        Task<KensingtonDTO> Activate(KensingtonDTO key);
        /// <summary>
        /// This will assign a Kensington to a device
        /// </summary>
        /// <param name="key"><see cref="KensingtonDTO"/></param>
        /// <returns></returns>
        Task AssignDevice(KensingtonDTO key);
        /// <summary>
        /// This will log the creation of a PDF file
        /// </summary>
        /// <param name="pdfFile"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task LogPdfFile(string pdfFile, int id);
        /// <summary>
        /// This will release a Kensington from a device
        /// </summary>
        /// <param name="key"><see cref="KensingtonDTO"/></param>
        /// <returns></returns>
        Task ReleaseDevice(KensingtonDTO key);
        /// <summary>
        /// This will return a list of all Kensingtons that are not assigned to a device
        /// </summary>
        /// <returns></returns>
        Task<List<KensingtonDTO>> ListAllFreeKeys();
    }
}