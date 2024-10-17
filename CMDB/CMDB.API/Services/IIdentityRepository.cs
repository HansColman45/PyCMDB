using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IIdentityRepository
    {
        Task<IdentityDTO?> GetById(int id);
        Task<IEnumerable<IdentityDTO>> GetAll();
        Task<IEnumerable<IdentityDTO>> GetAll(string searchStr);
        Task<IEnumerable<IdentityDTO>> ListAllFreeIdentities();
        IdentityDTO Create(IdentityDTO identityDTO);
        Task LogPdfFile(string pdfFile, int id);
        Task<IdentityDTO> Deactivate(IdentityDTO identity, string reason);
        Task<IdentityDTO> Activate(IdentityDTO identity);
        Task<IdentityDTO> Update(IdentityDTO identity);
    }
}
