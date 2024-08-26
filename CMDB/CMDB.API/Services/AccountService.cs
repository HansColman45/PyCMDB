using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class AccountService : LogService, IAccountService
    {
        private readonly string Table = "account";
        private AutoMapper.Mapper Mapper;
        public AccountService(CMDBContext context) : base(context)
        {
            Mapper = MapperConfig.InitializeAutomapper();
        }
        public async Task<List<AccountDTO>> ListAll()
        {
            List<AccountDTO> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Select(x => new AccountDTO()
                {
                    AccID = x.AccID,
                    ApplicationId = x.ApplicationId,
                    UserID = x.UserID,
                    Application = new()
                    {
                        Active = x.Application.active,
                        AppID = x.Application.AppID,
                        DeactivateReason = x.Application.DeactivateReason,
                        LastModifiedAdminId = x.Application.LastModifiedAdminId,
                        Name = x.Application.Name,
                    },
                    Active = x.active,
                    DeactivateReason = x.DeactivateReason,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    TypeId = x.TypeId,
                    Type = new()
                    {
                        TypeId = x.Type.TypeId,
                        Active = x.Type.active,
                        Description = x.Type.Description,
                        Type = x.Type.Type,
                        DeactivateReason = x.Type.DeactivateReason,
                        LastModifiedAdminId= x.Type.LastModifiedAdminId
                    }
                })
                .ToListAsync();
            return accounts;
        }
        public async Task<List<AccountDTO>> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<AccountDTO> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => EF.Functions.Like(x.Application.Name, searhterm)
                    || EF.Functions.Like(x.Type.Type, searhterm)
                    || EF.Functions.Like(x.Type.Description, searhterm)
                    || EF.Functions.Like(x.UserID, searhterm))
                .Select(x => new AccountDTO()
                {
                    AccID = x.AccID,
                    ApplicationId = x.ApplicationId,
                    UserID = x.UserID,
                    Application = new()
                    {
                        Active = x.Application.active,
                        AppID = x.Application.AppID,
                        DeactivateReason = x.Application.DeactivateReason,
                        LastModifiedAdminId = x.Application.LastModifiedAdminId,
                        Name = x.Application.Name,
                    },
                    Active = x.active,
                    DeactivateReason = x.DeactivateReason,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    TypeId = x.TypeId,
                    Type = new()
                    {
                        TypeId = x.Type.TypeId,
                        Active = x.Type.active,
                        Description = x.Type.Description,
                        Type = x.Type.Type,
                        DeactivateReason = x.Type.DeactivateReason,
                        LastModifiedAdminId = x.Type.LastModifiedAdminId
                    }
                })
                .ToListAsync();
            return accounts;
        }
        public async Task<AccountDTO?> GetById(int id)
        {
            var account = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == id).Select(x => new AccountDTO()
                {
                    AccID = x.AccID,
                    ApplicationId = x.ApplicationId,
                    UserID = x.UserID,
                    Application = new()
                    {
                        Active = x.Application.active,
                        AppID = x.Application.AppID,
                        DeactivateReason = x.Application.DeactivateReason,
                        LastModifiedAdminId = x.Application.LastModifiedAdminId,
                        Name = x.Application.Name,
                    },
                    Active = x.active,
                    DeactivateReason = x.DeactivateReason,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    TypeId = x.TypeId,
                    Type = new()
                    {
                        TypeId = x.Type.TypeId,
                        Active = x.Type.active,
                        Description = x.Type.Description,
                        Type = x.Type.Type,
                        DeactivateReason = x.Type.DeactivateReason,
                        LastModifiedAdminId = x.Type.LastModifiedAdminId
                    }
                })
                .FirstOrDefaultAsync();
            return account;
        }        
        public async Task<AccountDTO?> CreateNew(AccountDTO accountDTO)
        {
            Account newAcc = ConvertDto(accountDTO);
            newAcc.LastModfiedAdmin = Admin;
            _context.Accounts.Add(newAcc);
            await _context.SaveChangesAsync();
            string Value = $"Account with UserID: {newAcc.UserID} and with type {newAcc.Type.Type} for application {newAcc.Application.Name}";
            await LogCreate(Table, newAcc.AccID, Value);
            return ConvertAccount(newAcc);
        }
        public async Task<AccountDTO?> ActivateById(int id)
        {
            var _account = await GetById(id);

            return _account;
        }
        public async Task<AccountDTO?> DeactivateById(int id)
        {
            var _account = await GetById(id);

            return _account;
        }
        public async Task<AccountDTO?> Update(AccountDTO account)
        {
            var _account = await GetById(account.AccID);

            return _account;
        }
        public bool IsAccountExisting(AccountDTO account, string UserID = "", int application = 0)
        {
            bool result = false;
            if (String.IsNullOrEmpty(UserID) && application == 0)
            {
                var accounts = _context.Accounts
                    .Include(x => x.Application)
                    .Where(x => x.UserID == account.UserID && x.Application.AppID == account.Application.AppID).ToList();
                if (accounts.Count > 0)
                    result = true;
            }
            else
            {
                if (String.Compare(account.UserID, UserID) != 0 && account.Application.AppID == application)
                {
                    var accounts = _context.Accounts
                        .Include(x => x.Application)
                        .Where(x => x.UserID == UserID && x.Application.AppID == account.Application.AppID).ToList();
                    if (accounts.Count > 0)
                        result = true;
                }
                else if (String.Compare(account.UserID, UserID) == 0 && account.Application.AppID == application)
                {
                    result = false;
                }
                else if (String.Compare(account.UserID, UserID) != 0 && account.Application.AppID != application)
                {
                    var accounts = _context.Accounts
                        .Include(x => x.Application)
                        .Where(x => x.UserID == UserID && x.Application.AppID == application).ToList();
                    if (accounts.Count > 0)
                        result = true;
                }
            }
            return result;
        }      
        private AccountType GetAccountTypeById(int typeId)
        {
            AccountType accountType = _context.Types.OfType<AccountType>().Where(x => x.TypeId == typeId).First();
            return accountType;
        }
        private Application GetApplicationById(int appId)
        {
            Application application = _context.Applications.Where(x =>x.AppID == appId).First();
            return application;
        }
        private Account ConvertDto(AccountDTO accountDTO)
        {
            return new()
            {
                active = accountDTO.Active,
                AccID = accountDTO.AccID,
                Application = new()
                {
                    AppID = accountDTO.Application.AppID,
                    active = accountDTO.Application.Active,
                    DeactivateReason = accountDTO.Application.DeactivateReason,
                    LastModifiedAdminId = accountDTO.Application.LastModifiedAdminId,
                    Name = accountDTO.Application.Name
                },
                DeactivateReason = accountDTO.DeactivateReason,
                LastModifiedAdminId = accountDTO.LastModifiedAdminId,
                Type = new()
                {
                    active = accountDTO.Type.Active,
                    DeactivateReason = accountDTO.Type.DeactivateReason,
                    Description = accountDTO.Type.Description,
                    TypeId = accountDTO.Type.TypeId
                },
                UserID = accountDTO.UserID,
                ApplicationId = accountDTO.ApplicationId,
                TypeId = accountDTO.TypeId
            };
        }
        private AccountDTO ConvertAccount(Account account)
        {
            return new()
            {
                AccID = account.AccID,
                ApplicationId = account.ApplicationId,
                UserID = account.UserID,
                Application = new()
                {
                    Active = account.Application.active,
                    AppID = account.Application.AppID,
                    DeactivateReason = account.Application.DeactivateReason,
                    LastModifiedAdminId = account.Application.LastModifiedAdminId,
                    Name = account.Application.Name,
                },
                Active = account.active,
                DeactivateReason = account.DeactivateReason,
                LastModifiedAdminId = account.LastModifiedAdminId,
                TypeId = account.TypeId,
                Type = new()
                {
                    TypeId = account.Type.TypeId,
                    Active = account.Type.active,
                    Description = account.Type.Description,
                    Type = account.Type.Type,
                    DeactivateReason = account.Type.DeactivateReason,
                    LastModifiedAdminId = account.Type.LastModifiedAdminId
                }
            };
        }
    }
}
