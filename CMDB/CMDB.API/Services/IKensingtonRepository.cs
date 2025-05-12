using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IKensingtonRepository
    {
        Task<List<KensingtonDTO>> ListAll();
        Task<List<KensingtonDTO>> ListAll(string search);
        Task<KensingtonDTO> GetById(int id);
        KensingtonDTO Create(KensingtonDTO key);
        Task<KensingtonDTO> Update(KensingtonDTO key);
        Task<KensingtonDTO> DeActivate(KensingtonDTO key, string reason);
        Task<KensingtonDTO> Activate(KensingtonDTO key);
        Task AssignDevice(KensingtonDTO key);
        Task LogPdfFile(string pdfFile, int id);
        Task ReleaseDevice(KensingtonDTO key);
        Task<List<KensingtonDTO>> ListAllFreeKeys();
    }
}