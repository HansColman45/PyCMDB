using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IIdenAccountRepository
    {
        Task<IdenAccountDTO> GetById(int id);
        Task<IdenAccount> GetIdenAccountById(int id);
    }
    public class IdenAccountRepository : GenericRepository, IIdenAccountRepository
    {
        public IdenAccountRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<IdenAccountDTO> GetById(int id) 
        {
            _logger.LogInformation("Get by Id");
            var iden = await _context.IdenAccounts.AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Application).AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Where(x => x.ID == id).AsNoTracking()
                .Select(x => new IdenAccountDTO()
                {
                    ValidFrom = x.ValidFrom,
                    ValidUntil = x.ValidUntil,
                    Id = id,
                    Account = new AccountDTO() {
                        AccID = x.Account.AccID,
                        Active = x.Account.active,
                        DeactivateReason = x.Account.DeactivateReason,
                        LastModifiedAdminId = x.Account.LastModifiedAdminId,
                        ApplicationId = x.Account.ApplicationId,
                        TypeId = x.Account.TypeId,
                        UserID = x.Account.UserID,
                        Application = new ApplicationDTO()
                        {
                            AppID = x.Account.Application.AppID,
                            Active = x.Account.Application.active,
                            Name = x.Account.Application.Name,
                            DeactivateReason = x.Account.Application.DeactivateReason,
                            LastModifiedAdminId = x.Account.Application.LastModifiedAdminId,
                        },
                        Type = new TypeDTO()
                        {
                            Active = x.Account.Type.active,
                            DeactivateReason = x.Account.Type.DeactivateReason,
                            Description = x.Account.Type.Description,
                            Type = x.Account.Type.Type,
                            TypeId = x.Account.Type.TypeId,
                            LastModifiedAdminId = x.Account.Type.LastModifiedAdminId
                        }
                    },
                    Identity = new IdentityDTO()
                    {
                        IdenId = x.Identity.IdenId,
                        Active = x.Identity.active,
                        Company = x.Identity.Company,
                        EMail = x.Identity.EMail,
                        Name = x.Identity.Name,
                        DeactivateReason = x.Identity.DeactivateReason,
                        UserID = x.Identity.UserID,
                        Type = new TypeDTO()
                        {
                            Description = x.Identity.Type.Description,
                            Active = x.Identity.Type.active,
                            DeactivateReason = x.Identity.Type.DeactivateReason,
                            LastModifiedAdminId = x.Identity.Type.LastModifiedAdminId,
                            Type = x.Identity.Type.Type,
                            TypeId = x.Identity.Type.TypeId,
                        },
                        Language = new() {
                            Code = x.Identity.Language.Code,
                            Description = x.Identity.Language.Description
                        }
                    }
                })
                .FirstAsync();
            return iden;
        }
        public async Task<IdenAccount> GetIdenAccountById(int id)
        {
            var iden = await _context.IdenAccounts.AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Application).AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .FirstAsync();
            return iden;
        }
    }
}
