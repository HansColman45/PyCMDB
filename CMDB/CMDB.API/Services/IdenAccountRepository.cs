using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// Interface for the IdenAccountRepository
    /// </summary>
    public interface IIdenAccountRepository
    {
        /// <summary>
        /// This will return the details of an identity account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdenAccountDTO> GetById(int id);
        /// <summary>
        /// This will return the details of an identity account by using the Account ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdenAccount> GetIdenAccountById(int id);
        /// <summary>
        /// This will check if the period is overlapping
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> IsPeriodOverlapping(IsPeriodOverlappingRequest request);
    }
    /// <summary>
    /// Repository for managing identity accounts
    /// </summary>
    public class IdenAccountRepository : GenericRepository, IIdenAccountRepository
    {
        /// <summary>
        /// Constructor for the IdenAccountRepository
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public IdenAccountRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc/>
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
                .Select(x => ConvertIdenenty(x))
                .FirstAsync();
            return iden;
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public async Task<bool> IsPeriodOverlapping(IsPeriodOverlappingRequest request)
        {
            var Identity = await _context.IdenAccounts
                        .Include(x => x.Identity)
                        .Where(x => x.Identity.IdenId == request.IdentityId && request.StartDate <= x.ValidFrom && x.ValidUntil >= request.EndDate).AsNoTracking()
                        .ToListAsync();
            if (Identity.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This will convert the identity account to a DTO
        /// </summary>
        /// <param name="idenAccount"></param>
        /// <returns></returns>
        public static IdenAccountDTO ConvertIdenenty(IdenAccount idenAccount)
        {
            return new IdenAccountDTO()
            {
                ValidFrom = idenAccount.ValidFrom,
                ValidUntil = idenAccount.ValidUntil,
                Id = idenAccount.ID,
                Account = new AccountDTO()
                {
                    AccID = idenAccount.Account.AccID,
                    Active = idenAccount.Account.active,
                    DeactivateReason = idenAccount.Account.DeactivateReason,
                    LastModifiedAdminId = idenAccount.Account.LastModifiedAdminId,
                    ApplicationId = idenAccount.Account.ApplicationId,
                    TypeId = idenAccount.Account.TypeId,
                    UserID = idenAccount.Account.UserID,
                    Application = new ApplicationDTO()
                    {
                        AppID = idenAccount.Account.Application.AppID,
                        Active = idenAccount.Account.Application.active,
                        Name = idenAccount.Account.Application.Name,
                        DeactivateReason = idenAccount.Account.Application.DeactivateReason,
                        LastModifiedAdminId = idenAccount.Account.Application.LastModifiedAdminId,
                    },
                    Type = new TypeDTO()
                    {
                        Active = idenAccount.Account.Type.active,
                        DeactivateReason = idenAccount.Account.Type.DeactivateReason,
                        Description = idenAccount.Account.Type.Description,
                        Type = idenAccount.Account.Type.Type,
                        TypeId = idenAccount.Account.Type.TypeId,
                        LastModifiedAdminId = idenAccount.Account.Type.LastModifiedAdminId
                    }
                },
                Identity = new IdentityDTO()
                {
                    IdenId = idenAccount.Identity.IdenId,
                    Active = idenAccount.Identity.active,
                    Company = idenAccount.Identity.Company,
                    EMail = idenAccount.Identity.EMail,
                    Name = idenAccount.Identity.Name,
                    DeactivateReason = idenAccount.Identity.DeactivateReason,
                    UserID = idenAccount.Identity.UserID,
                    Type = new TypeDTO()
                    {
                        Description = idenAccount.Identity.Type.Description,
                        Active = idenAccount.Identity.Type.active,
                        DeactivateReason = idenAccount.Identity.Type.DeactivateReason,
                        LastModifiedAdminId = idenAccount.Identity.Type.LastModifiedAdminId,
                        Type = idenAccount.Identity.Type.Type,
                        TypeId = idenAccount.Identity.Type.TypeId,
                    },
                    Language = new()
                    {
                        Code = idenAccount.Identity.Language.Code,
                        Description = idenAccount.Identity.Language.Description
                    }
                }
            };
        }
    }
}
