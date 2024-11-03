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
        Task<bool> IsExisting(IdentityDTO identity);
        Task AssignDevices(IdentityDTO identity, List<string> assetTags);
        Task ReleaseDevices(IdentityDTO identity, List<string> assetTags);
        Task AssignAccount(IdenAccountDTO idenAccount);
        Task ReleaseAccount(IdenAccountDTO idenAccount);
    }
}
