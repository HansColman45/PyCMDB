using CMDB.Infrastructure;
using System.Collections.Generic;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace CMDB.Services
{
    public class AccountService : LogService
    {
        public AccountService(CMDBContext context) : base(context)
        {
        }
        public ICollection<Account> ListAll()
        {
            List<Account> accounts = _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .ToList();
            return accounts;
        }
        public List<Account> GetByID(int ID)
        {
            List<Account> accounts = _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == ID)
                .ToList();
            return accounts;
        }
        public List<Account> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<Account> accounts = _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => EF.Functions.Like(x.Application.Name, searhterm) || EF.Functions.Like(x.Type.Type, searhterm) || EF.Functions.Like(x.Type.Description, searhterm) || EF.Functions.Like(x.UserID, searhterm))
                .ToList();
            return accounts;
        }
        public void CreateNew(string UserID, int type, int application, string Table)
        {
            var accountType = GetAccountTypeByID(type).First();
            var applications = GetApplicationByID(application).First();
            Account account = new()
            {
                UserID = UserID,
                Application = applications,
                Type = accountType
            };
            _context.Accounts.Add(account);
            _context.SaveChanges();
            string Value = $"Account width UserID: {UserID} with type {accountType.Type} for application {applications.Name}";
            LogCreate(Table, account.AccID, Value);
        }
        public void Edit(Account account, string UserID, int type, int application, string Table)
        {
            var accountType = GetAccountTypeByID(type).First();
            var applications = GetApplicationByID(application).First();
            if (String.Compare(account.UserID, UserID) != 0)
            {
                LogUpdate(Table, account.AccID, "UserId", account.UserID, UserID);
                account.UserID = UserID;
            }
            if (account.Type.TypeID != type)
            {
                LogUpdate(Table, account.AccID, "Type", account.Type.Type, accountType.Type);
                account.Type = accountType;
            }
            if (account.Application.AppID != application)
            {
                LogUpdate(Table, account.AccID, "Application", account.Application.Name, applications.Name);
                account.Application = applications;
            }
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }
        public void Deactivate(Account account, string Reason, string Table)
        {
            account.DeactivateReason = Reason;
            account.Active = "Inactive";
            string value = $"Account width UserID: {account.UserID} and type {account.Type.Description}";
            LogDeactivate(Table, account.AccID, value, Reason);
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }
        public void Activate(Account account, string Table)
        {
            account.DeactivateReason = null;
            account.Active = "Active";
            string value = $"Account width UserID: {account.UserID} and type {account.Type.Description}";
            LogActivate(Table, account.AccID, value);
            _context.Accounts.Update(account);
            _context.SaveChanges();
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
            List<AccountType> accountTypes = _context.AccountTypes.Where(x => x.active == 1).ToList();
            foreach (var accountType in accountTypes)
            {
                accounts.Add(new SelectListItem(accountType.Type, accountType.TypeID.ToString()));
            }
            return accounts;
        }
        public bool IsAccountExisting(Account account, string UserID = "", int application = 0)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(UserID) && String.Compare(account.UserID, UserID) != 0)
            {
                var accounts = _context.Accounts.Where(x => x.UserID == UserID).ToList();
                if (accounts.Count > 0)
                    result = true;
            }
            if (application != 0 && account.Application.AppID != application)
            {
                var accounts = _context.Accounts.Include(x => x.Application).Where(x => x.Application.AppID == application).ToList();
                if (accounts.Count > 0)
                    result = true;
            }
            return result;
        }
        public void AssignIdentity2Account(Account account, int IdenID, DateTime ValidFrom, DateTime ValidUntil, string Table)
        {
            var Identity = GetIdentityByID(IdenID).First();
            IdenAccount IdenAcc = new()
            {
                Identity = Identity,
                Account = account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil
            };
            _context.IdenAccounts.Add(IdenAcc);
            _context.SaveChanges();
            LogAssignAccount2Identity(Table, account.AccID, account, Identity);
            LogAssignIden2Account("identity", IdenID, Identity, account);
        }
        public void ReleaseIdentity4Acount(Account account, Identity identity, int idenAccountID, string Table)
        {
            var IdenAccount = _context.IdenAccounts.Where(x => x.ID == idenAccountID).First();
            IdenAccount.ValidUntil = DateTime.Now.AddDays(-1);
            _context.IdenAccounts.Update(IdenAccount);
            LogReleaseAccountFromIdentity(Table, identity.IdenId, identity, account);
            LogReleaseIdentity4Account("identity", account.AccID, identity, account);
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
            List<AccountType> accountTypes = _context.AccountTypes.Where(x => x.TypeID == ID).ToList();
            return accountTypes;
        }
        public List<SelectListItem> ListAllFreeIdentities()
        {
            List<SelectListItem> accounts = new();
            var identities = _context.Identities
                .Include(x => x.Accounts)
                .Where(x => x.Active == "Active" && x.IdenId != 1)
                .ToList();
            foreach (var idenity in identities)
            {
                foreach (var acc in idenity.Accounts)
                {
                    DateTime endDate = acc.ValidUntil;
                    if (acc.ValidFrom <= DateTime.Now || DateTime.Now >= endDate)
                        accounts.Add(new SelectListItem(idenity.UserID + " " + idenity.Name, idenity.IdenId.ToString()));
                }
            }
            return accounts;
        }
        public bool IsPeriodOverlapping(int? IdenID, int? AccID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            if (IdenID == null || AccID == null)
                throw new Exception("Missing required id's");
            else
            {
                if (IdenID != null)
                {
                    var Identity = _context.IdenAccounts
                        .Include(x => x.Identity)
                        .Where(x => x.Identity.IdenId == IdenID && ValidFrom <= x.ValidFrom && x.ValidUntil >= ValidUntil)
                        .ToList();
                    if (Identity.Count > 0)
                        result = true;
                    else
                        result = false;
                }
                else if (AccID != null)
                {
                    var accounts = _context.IdenAccounts
                        .Include(x => x.Account)
                        .Where(x => x.Account.AccID == AccID && ValidFrom <= x.ValidFrom && x.ValidUntil >= ValidUntil)
                        .ToList();
                    if (accounts.Count > 0)
                        result = true;
                    else
                        result = false;
                }

            }
            return result;
        }
        public ICollection<Identity> GetIdentityByID(int id)
        {
            List<Identity> identities = _context.Identities
                .Include(x => x.Type)
                .Where(x => x.IdenId == id)
                .ToList();
            return identities;
        }
        public List<IdenAccount> GetIdenAccountByID(int id)
        {
            var idenAccounts = _context.IdenAccounts
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.ID == id)
                .ToList();
            return idenAccounts;
        }
    }
}
