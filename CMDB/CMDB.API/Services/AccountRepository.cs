using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class AccountRepository : GenericRepository, IAccountRepository
    {
        private readonly string table = "account";
        public AccountRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task LogPdfFile(string pdfFile, int id)
        {
            Account acc = await TrackedAccount(id);
            acc.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile)
            });
            _context.Accounts.Update(acc);
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
            var _account = await TrackedAccount(account.AccID);
            _account.DeactivateReason = reason;
            _account.active = 0;
            _account.LastModifiedAdminId = TokenStore.AdminId;
            string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
            string logLine = GenericLogLineCreator.DeleteLogLine(value, $"{TokenStore.Admin.Account.UserID}", reason, table);
            _account.LastModfiedAdmin = TokenStore.Admin;
            _account.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = logLine,
            });
            _context.Accounts.Update(_account);
            return account;
        }
        public async Task<AccountDTO> Activate(AccountDTO account) 
        {
            var _account = await TrackedAccount(account.AccID);
            _account.DeactivateReason = "";
            _account.Active = State.Active;
            string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
            string logline = GenericLogLineCreator.ActivateLogLine(value, $"{TokenStore.Admin.Account.UserID}", table);
            _account.LastModfiedAdmin = TokenStore.Admin;
            _account.LastModifiedAdminId= TokenStore.AdminId;
            _account.Logs.Add(new()
            {
                LogText = logline,
                LogDate = DateTime.UtcNow,
            });
            _context.Accounts.Update(_account);
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
            var _account = await TrackedAccount(account.AccID); 
            string logline;
            if (string.Compare(_account.UserID, account.UserID) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("UserId", _account.UserID, account.UserID, $"{TokenStore.Admin.Account.UserID}", table);
                _account.UserID = account.UserID;
                _account.LastModifiedAdminId = TokenStore.AdminId;
                _account.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow,
                });
            }
            if (_account.TypeId != account.Type.TypeId)
            {
                var Oldtype = _context.Types.OfType<AccountType>().AsNoTracking().First(x => x.TypeId == _account.TypeId);
                logline = GenericLogLineCreator.UpdateLogLine("Type", Oldtype.Type, account.Type.Type, $"{TokenStore.Admin.Account.UserID}", table);
                _account.TypeId = account.Type.TypeId;
                _account.LastModifiedAdminId = TokenStore.AdminId;
                _account.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow,
                });
            }
            if (_account.ApplicationId != account.Application.AppID)
            {
                var oldApp = _context.Applications.First(x => x.AppID == _account.ApplicationId);
                logline = GenericLogLineCreator.UpdateLogLine("Application", oldApp.Name, account.Application.Name, $"{TokenStore.Admin.Account.UserID}", table);
                _account.ApplicationId = account.Application.AppID;
                _account.LastModifiedAdminId = TokenStore.AdminId;
                _account.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow,
                });
            }
            _context.Accounts.Update(_account);
            return account;
        }
        public async Task AssignAccount2Identity(IdenAccountDTO request)
        {
            var iden = _context.Identities.First(x => x.IdenId == request.Identity.IdenId);
            var acc = await TrackedAccount(request.Account.AccID);
            IdenAccount IdenAcc = new()
            {
                IdentityId = iden.IdenId,
                AccountId = acc.AccID,
                ValidFrom = request.ValidFrom,
                ValidUntil = request.ValidUntil,
            };
            acc.LastModifiedAdminId = TokenStore.AdminId;
            iden.LastModifiedAdminId = TokenStore.AdminId;
            _context.IdenAccounts.Add(IdenAcc);
            _context.Accounts.Update(acc);
            _context.Identities.Update(iden);
            string accountInfo = $"Account with UserID: {acc.UserID}";
            string indenInfo = $"Identity with name: {iden.Name}";
            Log log = new()
            {
                LogText = GenericLogLineCreator.AssingAccount2IdenityLogLine(accountInfo, indenInfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow,
                AccountId = acc.AccID
            };
            _context.Logs.Add(log);
            log = new()
            {
                LogText = GenericLogLineCreator.AssingAccount2IdenityLogLine( indenInfo, accountInfo, TokenStore.Admin.Account.UserID, "identity"),
                LogDate = DateTime.UtcNow,
                IdentityId = iden.IdenId
            };
            _context.Logs.Add(log);
        }
        public async Task ReleaseAccountFromIdentity(IdenAccountDTO request)
        {
            var idenacc = await _context.IdenAccounts.Where(x => x.ID == request.Id).FirstAsync();
            idenacc.ValidUntil = DateTime.UtcNow.AddDays(-1);
            idenacc.LastModifiedAdminId = TokenStore.AdminId;
            _context.IdenAccounts.Update(idenacc);
            var acc = await TrackedAccount(request.Account.AccID);
            var iden = _context.Identities.First(x => x.IdenId == request.Identity.IdenId);
            string accountInfo = $"Account with UserID: {acc.UserID}";
            string indenInfo = $"Identity with name: {iden.Name}";
            acc.LastModifiedAdminId = TokenStore.AdminId;
            acc.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ReleaseAccountFromIdentityLogLine(accountInfo, indenInfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow
            });
            _context.Accounts.Update(acc);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ReleaseAccountFromIdentityLogLine(indenInfo, accountInfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow
            });
            _context.Identities.Update(iden);
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
        public async Task<IEnumerable<IdentityAccountInfo>> ListAllFreeAccounts()
        {
            var ideninfo = await _context.IdentityAccountInfos.FromSqlRaw(
                $"select distinct a.AccID, a.UserID, ap.Name from Account a " +
                "left join Application ap on a.ApplicationId = ap.AppID " +
                "left join IdenAccount ia on ia.AccountId = a.AccID " +
                "where ia.IdentityId is null or ia.ValidUntil <= GETDATE()")
                .ToListAsync();
            return ideninfo;
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
            foreach (var acc in Accounts)
            {
                accountDTO.Identities.Add(new()
                {
                    Id = acc.ID,
                    Identity = IdentityRepository.ConvertIdentity(acc.Identity),
                    ValidFrom = acc.ValidFrom,
                    ValidUntil = acc.ValidUntil,
                });
            }
        }
        private async Task<Account> TrackedAccount(int accId)
        {
            return await _context.Accounts.FirstAsync(x => x.AccID == accId);
        }
    }
}
