using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IAccountRepository
    {
        AccountDTO Create(AccountDTO account);
        Task<AccountDTO?> GetById(int id);
        Task<List<AccountDTO>> GetAll();
        Task<List<AccountDTO>> GetAll(string searchstr);
        Task<AccountDTO> DeActivate(AccountDTO account, string reason);
        Task<AccountDTO> Activate(AccountDTO account);
        Task<bool> IsExisitng(AccountDTO account);
        Task<AccountDTO> Update(AccountDTO account);
        Task AssignAccount2Identity(IdenAccountDTO request);
    }
    public class AccountRepository : GenericRepository, IAccountRepository
    {
        private readonly string table = "account";
        public AccountRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public AccountDTO Create(AccountDTO account)
        {
            Account acc = new()
            {
                LastModifiedAdminId = TokenStore.AdminId,
                active = 1,
                ApplicationId = account.ApplicationId,
                TypeId = account.TypeId,
                UserID = account.UserID
            };
            string value = $"Account with UserID: {acc.UserID} and type {account.Type.Description}";
            string logline = GenericLogLineCreator.CreateLogLine(value, TokenStore.Admin.Account.UserID, table);
            try
            {
                acc.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                });
                _context.Accounts.Add(acc);
                /*string sql = $"insert into {table} (TypeId,ApplicationId,UserID, LastModifiedAdminId) " +
                        $"values ({account.TypeId},{account.ApplicationId},'{account.UserID}',{TokenStore.Admin.AdminId})";
                await _context.Database.ExecuteSqlRawAsync(sql);
                var newacc = _context.Accounts.FirstOrDefault(x => x.UserID == account.UserID);
                sql = $"insert into log(LogDate,LogText,AccountId) values (GETDATE(),'{logline}',{newacc.AccID})";
                await _context.Database.ExecuteSqlRawAsync(sql);*/
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return account;
        }
        public async Task<AccountDTO?> GetById(int id)
        {
            var account = await _context.Accounts.AsNoTracking()
                .Include(x => x.Application).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Where(x => x.AccID == id).AsNoTracking()
                .Select(x => ConvertAccount(x))
                .FirstOrDefaultAsync();
            if (account is not null) { 
                GetLogs(table, id, account);
                GetAssignedIdentitiesForAccount(account);
            }
            return account;
        }
        public async Task<List<AccountDTO>> GetAll()
        {
            var accounts = await _context.Accounts.AsNoTracking()
                .Include(x => x.Application).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Select(x => ConvertAccount(x))
                .ToListAsync();
            return accounts;
        }
        public async Task<List<AccountDTO>> GetAll(string searchstr)
        {
            string searhterm = "%" + searchstr + "%";
            List<AccountDTO> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => EF.Functions.Like(x.Application.Name, searhterm)
                    || EF.Functions.Like(x.Type.Type, searhterm)
                    || EF.Functions.Like(x.Type.Description, searhterm)
                    || EF.Functions.Like(x.UserID, searhterm))
                .AsNoTracking()
                .Select(x => ConvertAccount(x))
                .ToListAsync();
            return accounts;
        }
        public async Task<AccountDTO> DeActivate(AccountDTO account,string reason)
        {
            var _account = await GetAccountById(account.AccID);
            _account.DeactivateReason = reason;
            _account.active = 0;
            string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
            string logLine = GenericLogLineCreator.DeleteLogLine(value, $"{TokenStore.Admin.Account.UserID}", reason, table);
            _account.LastModfiedAdmin = TokenStore.Admin;
            try
            {
                string sql = $"update {table} set active = 0, Deactivate_reason='{reason}', LastModifiedAdminId= {TokenStore.Admin.AdminId} where AccId= {account.AccID}";
                await _context.Database.ExecuteSqlRawAsync(sql);
                sql = $"insert into log(LogDate,LogText,AccountId) values (GETDATE(),'{logLine}',{account.AccID})";
                await _context.Database.ExecuteSqlRawAsync(sql);
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return account;
        }
        public async Task<AccountDTO> Activate(AccountDTO account) 
        {
            var _account = await GetAccountById(account.AccID);
            _account.DeactivateReason = "";
            _account.Active = State.Active;
            string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
            string logline = GenericLogLineCreator.ActivateLogLine(value, $"{TokenStore.Admin.Account.UserID}", table);
            _account.LastModfiedAdmin = TokenStore.Admin;
            try
            {
                string sql = $"update {table} set active = 1, Deactivate_reason='', LastModifiedAdminId= {TokenStore.Admin.AdminId} where AccId= {account.AccID}";
                await _context.Database.ExecuteSqlRawAsync(sql);
                sql = $"insert into log(LogDate,LogText,AccountId) values (GETDATE(),'{logline}',{account.AccID})";
                await _context.Database.ExecuteSqlRawAsync(sql);
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return account;
        }
        public async Task<bool> IsExisitng(AccountDTO account)
        {
            var oldAccount = await GetById(account.AccID);
            bool result = false;
            if (oldAccount is null)
            {
                var accounts = _context.Accounts
                    .Include(x => x.Application).AsNoTracking()
                    .Where(x => x.UserID == account.UserID && x.Application.AppID == account.Application.AppID).AsNoTracking()
                    .ToList();
                if (accounts.Count > 0)
                    result = true;
            }
            else
            {
                if (string.Compare(account.UserID, oldAccount.UserID) != 0 && account.Application.AppID == oldAccount.Application.AppID)
                {
                    var accounts = _context.Accounts
                        .Include(x => x.Application).AsNoTracking()
                        .Where(x => x.UserID == account.UserID && x.Application.AppID == account.Application.AppID).AsNoTracking()
                        .ToList();
                    if (accounts.Count > 0)
                        result = true;
                }
                else if (string.Compare(account.UserID, oldAccount.UserID) == 0 && account.Application.AppID == oldAccount.Application.AppID)
                {
                    result = false;
                }
                else if (string.Compare(account.UserID, oldAccount.UserID) != 0 && account.Application.AppID != oldAccount.Application.AppID)
                {
                    var accounts = _context.Accounts
                        .Include(x => x.Application).AsNoTracking()
                        .Where(x => x.UserID == account.UserID && x.Application.AppID == account.Application.AppID).AsNoTracking()
                        .ToList();
                    if (accounts.Count > 0)
                        result = true;
                }
            }
            return result;
        }
        public async Task<AccountDTO> Update(AccountDTO account)
        {
            var _account = await GetAccountById(account.AccID);
            string logline;
            _account.LastModfiedAdmin = TokenStore.Admin;
            if (string.Compare(_account.UserID, account.UserID) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("UserId", _account.UserID, account.UserID, $"{TokenStore.Admin.Account.UserID}", table);
                _account.UserID = account.UserID;
                try
                {
                    string sql = $"update Account set UserId='{account.UserID}', LastModifiedAdminId={TokenStore.Admin.AdminId} where AccID={account.AccID}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,AccountId) values (GETDATE(),'{logline}',{account.AccID})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e)
                {
                    _logger.LogError("DB error {e}", e);
                    throw;
                }
            }
            if (_account.Type.TypeId != account.Type.TypeId)
            {
                var type = AccountTypeRepository.ConvertDTO(account.Type);
                logline = GenericLogLineCreator.UpdateLogLine("Type", _account.Type.Type, account.Type.Type, $"{TokenStore.Admin.Account.UserID}", table);
                _account.TypeId = account.Type.TypeId;
                _account.Type = type;
                try
                {
                    string sql = $"update Account set TypeId={account.Type.TypeId}, LastModifiedAdminId={TokenStore.Admin.AdminId} where AccID={account.AccID}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,AccountId) values (GETDATE(),'{logline}',{account.AccID})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e)
                {
                    _logger.LogError("DB error {e}", e);
                    throw;
                }
            }
            if (_account.Application.AppID != account.Application.AppID)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Application", _account.Application.Name, account.Application.Name, $"{TokenStore.Admin.Account.UserID}", table);
                _account.Application = ApplicationRepository.ConvertDTO(account.Application);
                _account.ApplicationId = account.Application.AppID;
                try
                {
                    string sql = $"update Account set ApplicationId={account.Application.AppID}, LastModifiedAdminId={TokenStore.Admin.AdminId} where AccID={account.AccID}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,AccountId) values (GETDATE(),'{logline}',{account.AccID})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e) 
                { 
                    _logger.LogError("DB error {e}", e); 
                    throw; 
                }
            }
            //_context.Accounts.Update(_account);
            return account;
        }
        public async Task AssignAccount2Identity(IdenAccountDTO request)
        {
            var iden = IdentityRepository.ConvertDTO(request.Identity);
            var acc = await GetAccountById(request.Account.AccID);
            IdenAccount IdenAcc = new()
            {
                Identity = iden,
                Account = acc,
                ValidFrom = request.ValidFrom,
                ValidUntil = request.ValidUntil,
            };
            acc.LastModfiedAdmin = TokenStore.Admin;
            iden.LastModfiedAdmin = TokenStore.Admin;
            _context.IdenAccounts.Add(IdenAcc);
            string accountInfo = $"Account with UserID: {acc.UserID}";
            string indenInfo = $"Identity with name: {iden.Name}";
            Log log = new()
            {
                LogText = GenericLogLineCreator.AssingAccount2IdenityLogLine(accountInfo, indenInfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow,
                Account = acc
            };
            _context.Logs.Add(log);
            log = new()
            {
                LogText = GenericLogLineCreator.AssingAccount2IdenityLogLine( indenInfo, accountInfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow,
                Identity = iden
            };
            _context.Logs.Add(log);
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
        public static AccountDTO ConvertAccount(in Account account)
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
            var Accounts = _context.Accounts.AsNoTracking()
                .Include(x => x.Identities)
                .ThenInclude(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identities)
                .ThenInclude(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Include(x => x.Identities)
                .ThenInclude(x => x.Identity).AsNoTracking()
                .SelectMany(x => x.Identities)
                .Where(x => x.Account.AccID == accountDTO.AccID)
                .ToList();
        }
        private async Task<Account?> GetAccountById(int id)
        {
            var account = await _context.Accounts.AsNoTracking()
                .Include(x => x.Application).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Where(x => x.AccID == id).AsNoTracking()
                .FirstOrDefaultAsync();
            return account;
        }
    }
}
