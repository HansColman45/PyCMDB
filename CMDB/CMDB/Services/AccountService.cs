using CMDB.Infrastructure;
using System.Collections.Generic;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using CMDB.Util;

namespace CMDB.Services
{
    public class AccountService : LogService
    {
        public AccountService(CMDBContext context) : base(context)
        {
        }
        public async Task<List<Account>> ListAll()
        {
            BaseUrl = _url + $"api/Account/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<Account>>();
            else
                return new List<Account>();
            /*List<Account> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .ToListAsync();
            return accounts;*/
        }
        public async Task<List<Account>> GetByID(int ID)
        {
            BaseUrl = _url + $"api/Account/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<Account>>();
            else
                return new List<Account>();
            /*List<Account> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == ID)
                .ToListAsync();
            return accounts;*/
        }
        public async Task<List<Account>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/Account/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<Account>>();
            else
                return new List<Account>();
            /*string searhterm = "%" + searchString + "%";
            List<Account> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => EF.Functions.Like(x.Application.Name, searhterm)
                    || EF.Functions.Like(x.Type.Type, searhterm)
                    || EF.Functions.Like(x.Type.Description, searhterm)
                    || EF.Functions.Like(x.UserID, searhterm))
                .ToListAsync();
            return accounts;*/
        }
        public async Task CreateNew(string UserID, int type, int application, string Table)
        {
            var accountType = GetAccountTypeByID(type).First();
            var applications = GetApplicationByID(application).First();
            Account account = new()
            {
                UserID = UserID,
                Application = applications,
                Type = accountType,
                LastModfiedAdmin = Admin
            };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            string Value = $"Account with UserID: {UserID} and with type {accountType.Type} for application {applications.Name}";
            await LogCreate(Table, account.AccID, Value);
        }
        public async Task Edit(Account account, string UserID, int type, int application, string Table)
        {
            var accountType = GetAccountTypeByID(type).First();
            var applications = GetApplicationByID(application).First();
            if (String.Compare(account.UserID, UserID) != 0)
            {
                await LogUpdate(Table, account.AccID, "UserId", account.UserID, UserID);
                account.UserID = UserID;
            }
            if (account.Type.TypeId != type)
            {
                await LogUpdate(Table, account.AccID, "Type", account.Type.Type, accountType.Type);
                account.Type = accountType;
            }
            if (account.Application.AppID != application)
            {
                await LogUpdate(Table, account.AccID, "Application", account.Application.Name, applications.Name);
                account.Application = applications;
            }
            account.LastModfiedAdmin = Admin;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
        public async Task Deactivate(Account account, string Reason, string Table)
        {
            account.DeactivateReason = Reason;
            account.Active = State.Inactive;
            string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
            await LogDeactivate(Table, account.AccID, value, Reason);
            account.LastModfiedAdmin = Admin;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
        public async Task Activate(Account account, string Table)
        {
            account.DeactivateReason = null;
            account.Active = State.Active;
            string value = $"Account with UserID: {account.UserID} and type {account.Type.Description}";
            await LogActivate(Table, account.AccID, value);
            account.LastModfiedAdmin = Admin;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
        public void GetAssignedIdentitiesForAccount(Account account)
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
                .Where(x => x.Account.AccID == account.AccID)
                .ToList();
        }
        public List<SelectListItem> ListActiveAccountTypes()
        {
            List<SelectListItem> accounts = new();
            List<AccountType> accountTypes = _context.Types.OfType<AccountType>().Where(x => x.active == 1).ToList();
            foreach (var accountType in accountTypes)
            {
                accounts.Add(new SelectListItem(accountType.Type, accountType.TypeId.ToString()));
            }
            return accounts;
        }
        public bool IsAccountExisting(Account account, string UserID = "", int application = 0)
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
        public async Task AssignIdentity2Account(Account account, int IdenID, DateTime ValidFrom, DateTime ValidUntil, string Table)
        {
            var identities = await GetIdentityByID(IdenID);
            var Identity = identities.First();
            IdenAccount IdenAcc = new()
            {
                Identity = Identity,
                Account = account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil,
                LastModifiedAdmin = Admin
            };
            account.LastModfiedAdmin = Admin;
            Identity.LastModfiedAdmin = Admin;
            _context.IdenAccounts.Add(IdenAcc);
            await _context.SaveChangesAsync();
            await LogAssignAccount2Identity(Table, account.AccID, account, Identity);
            await LogAssignIden2Account("identity", IdenID, Identity, account);
        }
        public async Task ReleaseIdentity4Acount(Account account, Identity identity, int idenAccountID, string Table, string pdfFile)
        {
            var IdenAccount = _context.IdenAccounts.Where(x => x.ID == idenAccountID).First();
            IdenAccount.LastModifiedAdmin = Admin;
            IdenAccount.ValidUntil = DateTime.Now.AddDays(-1);
            _context.IdenAccounts.Update(IdenAccount);
            await LogReleaseAccountFromIdentity("identity", identity.IdenId, identity, account);
            await LogReleaseIdentity4Account(Table, account.AccID, identity, account);
            await LogPdfFile("identity",identity.IdenId, pdfFile);
            await LogPdfFile(Table,account.AccID, pdfFile);
            account.LastModfiedAdmin = Admin;
            identity.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
        }
        public List<Application> GetApplicationByID(int ID)
        {
            List<Application> application = _context.Applications.Where(x => x.AppID == ID).ToList();
            return application;
        }
        public List<SelectListItem> ListActiveApplications()
        {
            List<SelectListItem> accounts = new();
            List<Application> applications = _context.Applications.Where(x => x.active == 1).ToList();
            foreach (var appliction in applications)
            {
                accounts.Add(new SelectListItem(appliction.Name, appliction.AppID.ToString()));
            }
            return accounts;
        }
        public List<AccountType> GetAccountTypeByID(int ID)
        {
            List<AccountType> accountTypes = _context.Types.OfType<AccountType>().Where(x => x.TypeId == ID).ToList();
            return accountTypes;
        }
        public async Task<List<SelectListItem>> ListAllFreeIdentities()
        {
            List<SelectListItem> accounts = new();
            var identities = await _context.Identities
                .Include(x => x.Accounts)
                .Where(x => x.active == 1 && x.IdenId != 1)
                .Where(x => !x.Accounts.Any(y => y.ValidFrom <= DateTime.Now && y.ValidUntil >= DateTime.Now))
                .ToListAsync();
            foreach (var idenity in identities)
            {
                accounts.Add(new SelectListItem(idenity.UserID + " " + idenity.Name, idenity.IdenId.ToString()));
            }
            return accounts;
        }
        public bool IsPeriodOverlapping(int AccID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            var accounts = _context.IdenAccounts
                    .Include(x => x.Account)
                    .Where(x => x.Account.AccID == AccID && ValidFrom <= x.ValidFrom && x.ValidUntil >= ValidUntil)
                    .ToList();
            if (accounts.Count > 0)
                result = true;
            else
                result = false;
            return result;
        }
        public async Task<List<Identity>> GetIdentityByID(int id)
        {
            List<Identity> identities = await _context.Identities
                .Include(x => x.Type)
                .Where(x => x.IdenId == id)
                .ToListAsync();
            return identities;
        }
        public async Task<List<IdenAccount>> GetIdenAccountByID(int id)
        {
            var idenAccounts = await _context.IdenAccounts
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.ID == id)
                .ToListAsync();
            return idenAccounts;
        }
        public async Task LogPdfFile(string table, Account account, string pdfFile)
        {
            await LogPdfFile("identity", account.Identities.Last().Identity.IdenId, pdfFile);
            await LogPdfFile(table, account.AccID, pdfFile);
        }
    }
}
