using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class IdentityService : CMDBService, IIdentityService
    {
        private ILogService _logService;
        private readonly ILogger<IdentityService> _logger;
        public IdentityService(CMDBContext context, ILogService logService, ILogger<IdentityService> logger) : base(context)
        {
            _logService = logService;
            _logger = logger;
        }
        public async Task<IEnumerable<IdentityDTO>> GetAll()
        {
            return await _context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Select(x => new IdentityDTO()
                {
                    Active = x.active,
                    Company = x.Company,
                    DeactivateReason = x.DeactivateReason,
                    EMail = x.EMail,
                    IdenId = x.IdenId,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    Name = x.Name,
                    UserID = x.UserID,
                    Language = new LanguageDTO() { 
                        Code = x.Language.Code,
                        Description = x.Language.Description
                    },
                    Type = new TypeDTO() { 
                        Description = x.Type.Description,
                        Active = x.Type.active,
                        LastModifiedAdminId= x.Type.LastModifiedAdminId,
                        DeactivateReason = x.Type.DeactivateReason,
                        Type = x.Type.Type,
                        TypeId = x.Type.TypeId,
                    }
                }).ToListAsync();
        }
        public async Task<IEnumerable<IdentityDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            return await _context.Identities
                .Include(x => x.Type)
                .Where(x => EF.Functions.Like(x.Name, searhterm) || EF.Functions.Like(x.UserID, searhterm)
                    || EF.Functions.Like(x.EMail, searhterm) || EF.Functions.Like(x.Type.Type, searhterm))
                .Select(x => new IdentityDTO()
                {
                    Active = x.active,
                    Company = x.Company,
                    DeactivateReason = x.DeactivateReason,
                    EMail = x.EMail,
                    IdenId = x.IdenId,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    Name = x.Name,
                    UserID = x.UserID,
                    Type = new TypeDTO()
                    {
                        Description = x.Type.Description,
                        Active = x.Type.active,
                        LastModifiedAdminId = x.Type.LastModifiedAdminId,
                        DeactivateReason = x.Type.DeactivateReason,
                        Type = x.Type.Type,
                        TypeId = x.Type.TypeId,
                    }
                }).ToListAsync();
        }
        public async Task<IdentityDTO?> GetById(int id)
        {
            return await _context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Select(x => new IdentityDTO()
                {
                    Active = x.active,
                    Company = x.Company,
                    DeactivateReason = x.DeactivateReason,
                    EMail = x.EMail,
                    IdenId = x.IdenId,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    Name = x.Name,
                    UserID = x.UserID,
                    Language = new LanguageDTO()
                    {
                        Code = x.Language.Code,
                        Description = x.Language.Description
                    },
                    Type = new TypeDTO()
                    {
                        Description = x.Type.Description,
                        Active = x.Type.active,
                        LastModifiedAdminId = x.Type.LastModifiedAdminId,
                        DeactivateReason = x.Type.DeactivateReason,
                        Type = x.Type.Type,
                        TypeId = x.Type.TypeId,
                    }
                }).FirstOrDefaultAsync(x => x.IdenId == id);
        }
        public async Task<List<IdentityDTO>> ListAllFreeIdentities()
        {
            var identities = await _context.Identities
                .Include(x => x.Accounts)
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.active == 1 && x.IdenId != 1)
                .Where(x => !x.Accounts.Any(y => y.ValidFrom <= DateTime.Now && y.ValidUntil >= DateTime.Now))
                .Select(x => new IdentityDTO()
                {
                    Active = x.active,
                    Company = x.Company,
                    DeactivateReason = x.DeactivateReason,
                    EMail = x.EMail,
                    IdenId = x.IdenId,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    Name = x.Name,
                    UserID = x.UserID,
                    Language = new LanguageDTO()
                    {
                        Code = x.Language.Code,
                        Description = x.Language.Description
                    },
                    Type = new TypeDTO()
                    {
                        Description = x.Type.Description,
                        Active = x.Type.active,
                        LastModifiedAdminId = x.Type.LastModifiedAdminId,
                        DeactivateReason = x.Type.DeactivateReason,
                        Type = x.Type.Type,
                        TypeId = x.Type.TypeId,
                    }
                }).ToListAsync();
            return identities;
        }
        public static Identity ConvertDTO(IdentityDTO dto)
        {
            var iden = new Identity()
            {
                active = dto.Active,
                Company = dto.Company,
                DeactivateReason = dto.DeactivateReason,
                EMail = dto.EMail,
                IdenId = dto.IdenId,
                Name= dto.Name,
                LastModifiedAdminId= dto.LastModifiedAdminId,
                UserID= dto.UserID,
                TypeId= dto.Type.TypeId,
                Type = new IdentityType()
                {
                    DeactivateReason = dto.Type.DeactivateReason,
                    Type = dto.Type.Type,
                    Description = dto.Type.Description,
                    LastModifiedAdminId= dto.Type.LastModifiedAdminId,
                    active = dto.Type.Active,
                    TypeId= dto.Type.TypeId
                },
                Language = new Language()
                {
                    Code = dto.Language.Code,
                    Description = dto.Language.Description
                }
            };
            return iden;
        }
    }
}
