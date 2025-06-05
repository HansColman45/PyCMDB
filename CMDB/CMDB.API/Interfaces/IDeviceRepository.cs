using CMDB.API.Models;
using CMDB.Domain.Entities;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// The interface for the Device Repository
    /// </summary>
    public interface IDeviceRepository
    {
        /// <summary>
        /// This will return a list of devices that are not linked to a identity
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DeviceDTO>> ListAllFreeDevices();
        /// <summary>
        /// This will return a list of devices that are not linked to a site part
        /// </summary>
        /// <param name="sitePart"></param>
        /// <returns></returns>
        Task<IEnumerable<DeviceDTO>> ListAllFreeDevices(string sitePart);
        /// <summary>
        /// This will give a list of devices
        /// </summary>
        /// <param name="category">The category of the device</param>
        /// <returns></returns>
        Task<IEnumerable<DeviceDTO>> GetAll(string category);
        /// <summary>
        /// This will give a list of devices matching the search string
        /// </summary>
        /// <param name="category">The category of the device</param>
        /// <param name="searchString">The search string</param>
        /// <returns></returns>
        Task<IEnumerable<DeviceDTO>> GetAll(string category,string searchString);
        /// <summary>
        /// This function will get the details of a Device
        /// </summary>
        /// <param name="category">The category of the device</param>
        /// <param name="assetTag">The AssetTag of the device</param>
        /// <returns></returns>
        Task<DeviceDTO> GetByAssetTag(string category, string assetTag);
        /// <summary>
        /// This will return the detail of a Device using only the AssetTag
        /// </summary>
        /// <param name="assetTag"></param>
        /// <returns></returns>
        Task<DeviceDTO> GetByAssetTag(string assetTag);
        /// <summary>
        /// This will create a new device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <returns></returns>
        DeviceDTO Create(DeviceDTO deviceDTO);
        /// <summary>
        /// This will update an existing Device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <returns></returns>
        Task<DeviceDTO> Update(DeviceDTO deviceDTO);
        /// <summary>
        /// This will deactivate an existing device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<DeviceDTO> Deactivate(DeviceDTO deviceDTO, string reason);
        /// <summary>
        /// This will activate an existing device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <returns></returns>
        Task<DeviceDTO> Activate(DeviceDTO deviceDTO);
        /// <summary>
        /// This will return a list of RAMS
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RAM>> GetAllRams();
        /// <summary>
        /// This will log the generation of a PDF File
        /// </summary>
        /// <param name="category"></param>
        /// <param name="pdfFile"></param>
        /// <param name="asassetTag"></param>
        /// <returns></returns>
        Task LogPdfFile(string category, string pdfFile, string asassetTag);
        /// <summary>
        /// This function will assign an <see cref="Identity"/> to a <see cref="Device"/>
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Task<DeviceDTO> AssignIdentity(DeviceDTO device);
        /// <summary>
        /// This function will release the <see cref="Identity"/> from a <see cref="Device"/>
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Task<DeviceDTO> ReleaseIdentity(DeviceDTO device);
        /// <summary>
        /// This function will check if the AssetTag is already used or not
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Task<bool> IsDeviceExising(DeviceDTO device);
        /// <summary>
        /// This function will assign a Kensington lock to a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Task AssignKensington(DeviceDTO device);
        /// <summary>
        /// This function will release the Kensington lock from a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Task ReleaseKensington(DeviceDTO device);
    }
}
