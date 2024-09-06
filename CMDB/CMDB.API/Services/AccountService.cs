using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class AccountService : CMDBService,IAccountService
    {
        private readonly string Table = "account";
        private readonly ILogger<AccountService> _logger;
        private ILogService _logService;
        public AccountService(CMDBContext context, ILogger<AccountService> logger, ILogService logService) : base(context)
        {
            _logger = logger;
            _logService = logService;
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
            if(account is not null) {
                _logService.GetLogs(Table, id, account);
                GetAssignedIdentitiesForAccount(account);
            }
            return account;
        }        
        public async Task<AccountDTO?> CreateNew(AccountDTO accountDTO)
        {
            try
            {
                Account newAcc = ConvertDto(accountDTO);
                newAcc.LastModfiedAdmin = TokenStore.Admin;
                _context.Accounts.Add(newAcc);
                await _context.SaveChangesAsync();
                string Value = $"Account with UserID: {newAcc.UserID} and with type {newAcc.Type.Type} for application {newAcc.Application.Name}";
                await _logService.LogCreate(Table, newAcc.AccID, Value);
                return ConvertAccount(newAcc);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        public async Task<AccountDTO?> Activate(AccountDTO account)
        {
            try
            {
                account.DeactivateReason = "";
                Account _account = ConvertDto(account);
                _account.DeactivateReason = "";
                _account.Active = State.Active;
                string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
                await _logService.LogActivate(Table, account.AccID, value); 
                _account.LastModfiedAdmin = TokenStore.Admin;
                _context.Accounts.Update(_account);
                await _context.SaveChangesAsync();
                return account;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        public async Task<AccountDTO?> Deactivate(AccountDTO account, string reason)
        {
            try
            {
                account.DeactivateReason = reason;
                Account _account = ConvertDto(account);
                _account.DeactivateReason = reason;
                _account.Active = State.Inactive;
                string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
                await _logService.LogDeactivate(Table, account.AccID, value, reason);
                _account.LastModfiedAdmin = TokenStore.Admin;
                _context.Accounts.Update(_account);
                await _context.SaveChangesAsync();
                return account;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        public async Task<AccountDTO?> Update(AccountDTO account)
        {
            var _account = await GetById(account.AccID);
            if (string.Compare(_account.UserID, account.UserID) != 0)
            {
                await _logService.LogUpdate(Table, account.AccID, "UserId", _account.UserID, account.UserID);
            }
            if (_account.Type.TypeId != account.Type.TypeId)
            {
                await _logService.LogUpdate(Table, account.AccID, "Type", _account.Type.Type, account.Type.Type);
            }
            if (_account.Application.AppID != account.Application.AppID)
            {
                await _logService.LogUpdate(Table, account.AccID, "Application", _account.Application.Name, account.Application.Name);
            }
            try
            {
                var Account = ConvertDto(account);
                Account.LastModfiedAdmin = TokenStore.Admin;
                _context.Accounts.Update(Account);
                await _context.SaveChangesAsync();
                return _account;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        public async Task<bool> IsAccountExisting(AccountDTO account)
        {
            var oldAccount = await GetById(account.AccID);
            bool result = false;
            if (oldAccount is null)
            {
                var accounts = _context.Accounts
                    .Include(x => x.Application)
                    .Where(x => x.UserID == account.UserID && x.Application.AppID == account.Application.AppID).ToList();
                if (accounts.Count > 0)
                    result = true;
            }
            else
            {
                if (String.Compare(account.UserID, oldAccount.UserID) != 0 && account.Application.AppID == oldAccount.Application.AppID)
                {
                    var accounts = _context.Accounts
                        .Include(x => x.Application)
                        .Where(x => x.UserID == account.UserID && x.Application.AppID == account.Application.AppID).ToList();
                    if (accounts.Count > 0)
                        result = true;
                }
                else if (String.Compare(account.UserID, oldAccount.UserID) == 0 && account.Application.AppID == oldAccount.Application.AppID)
                {
                    result = false;
                }
                else if (String.Compare(account.UserID, oldAccount.UserID) != 0 && account.Application.AppID != oldAccount.Application.AppID)
                {
                    var accounts = _context.Accounts
                        .Include(x => x.Application)
                        .Where(x => x.UserID == account.UserID && x.Application.AppID == account.Application.AppID).ToList();
                    if (accounts.Count > 0)
                        result = true;
                }
            }
            return result;
        }
        public async Task<AccountDTO> AssignIdentity(IdenAccountDTO request)
        {
            var account = ConvertDto(request.Account);
            var Identity = IdentityService.ConvertDTO(request.Identity);
            IdenAccount IdenAcc = new()
            {
                Identity = Identity,
                Account = account,
                ValidFrom = request.ValidFrom,
                ValidUntil = request.ValidUntil,
            };
            account.LastModfiedAdmin = TokenStore.Admin;
            Identity.LastModfiedAdmin = TokenStore.Admin;
            _context.IdenAccounts.Add(IdenAcc);
            await _context.SaveChangesAsync();
            await _logService.LogAssignAccount2Identity(Table, account.AccID, account, Identity);
            await _logService.LogAssignIden2Account("identity", Identity.IdenId, Identity, account);
            return request.Account;
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
        public static Account ConvertDto(AccountDTO accountDTO)
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
        public static AccountDTO ConvertAccount(Account account)
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
        private void GetAssignedIdentitiesForAccount(AccountDTO accountDTO)
        {
            var Accounts = _context.Accounts
                .Include(x => x.Identities)
                .ThenInclude(x => x.Identity)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identities)
                .ThenInclude(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Include(x => x.Identities)
                .ThenInclude(x => x.Identity)
                .SelectMany(x => x.Identities)
                .Where(x => x.Account.AccID == accountDTO.AccID)
                .ToList();
        }
    }
}
