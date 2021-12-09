using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMDB.Services
{
    public class IdentityServices : LogService
    {
        public IdentityServices(CMDBContext context) : base(context)
        {
        }
        public ICollection<Identity> ListAll()
        {
            List<Identity> identities = _context.Identities
                .Include(x => x.Type)
                .ToList();
            return identities;
        }
        public ICollection<Identity> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<Identity> list = _context.Identities
                .Include(x => x.Type)
                .Where(x => EF.Functions.Like(x.Name, searhterm) || EF.Functions.Like(x.UserID, searhterm)
                    || EF.Functions.Like(x.EMail, searhterm) || EF.Functions.Like(x.Type.Type, searhterm))
                .ToList();
            return list;
        }
        public ICollection<Identity> GetByID(int id)
        {
            List<Identity> identities = _context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.IdenId == id)
                .ToList();
            return identities;
        }
        public List<SelectListItem> ListActiveIdentityTypes()
        {
            List<SelectListItem> identityTypes = new();
            List<IdentityType> types = _context.IdentityTypes.Where(x => x.active == 1).ToList();
            foreach (var type in types)
            {
                identityTypes.Add(new SelectListItem(type.Type + " " + type.Description, type.TypeID.ToString()));
            }
            return identityTypes;
        }
        public List<SelectListItem> ListAllActiveLanguages()
        {
            List<SelectListItem> langs = new();
            List<Language> languages = _context.Languages.Where(x => x.active == 1).ToList();
            foreach (var language in languages)
            {
                langs.Add(new SelectListItem(language.Description, language.Code));
            }
            return langs;
        }
        public void GetAssingedDevices(Identity identity)
        {
            var laptops = _context.Identities
                .Include(x => x.Laptops)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Laptops)
                .ToList();
            foreach (var laptop in laptops)
            {
                identity.Laptops.Add(laptop);
            }
            var desktops = _context.Identities
                .Include(x => x.Desktops)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Desktops)
                .ToList();
            foreach (var desktop in desktops)
            {
                identity.Desktops.Add(desktop);
            }
            var Screens = _context.Identities
                .Include(x => x.Screens)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Screens)
                .ToList();
            foreach (var screen in Screens)
            {
                identity.Screens.Add(screen);
            }
            var Dockings = _context.Identities
                .Include(x => x.Dockings)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Dockings)
                .ToList();
            foreach (var docking in Dockings)
            {
                identity.Dockings.Add(docking);
            }
            var mobiles = _context.Identities
                .Include(x => x.Mobiles)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Mobiles)
                .ToList();
            foreach (var mobile in mobiles)
            {
                identity.Mobiles.Add(mobile);
            }
            var tokens = _context.Identities
                .Include(x => x.Tokens)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Tokens)
                .ToList();
            foreach (var token in tokens)
            {
                identity.Tokens.Add(token);
            }

        }
        public void GetAssignedAccounts(Identity identity)
        {
            var accounts = _context.Identities
                .Include(x => x.Language)
                .Include(x => x.Accounts)
                .ThenInclude(d => d.Account)
                .SelectMany(x => x.Accounts)
                .Where(x => x.Identity.IdenId == identity.IdenId)
                .ToList();
        }
        public void Create(string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language, string Table)
        {
            var Type = GetIdenityTypeByID(type).First();
            var Lang = _context.Languages.
                Where(x => x.Code == Language)
                .SingleOrDefault();
            Identity identity = new()
            {
                FirstName = firstName,
                LastName = LastName,
                UserID = UserID,
                Company = Company,
                EMail = EMail,
                Type = Type,
                Language = Lang
            };
            _context.Identities.Add(identity);
            _context.SaveChanges();
            string Value = "Identity width name: " + firstName + ", " + LastName;
            LogCreate(Table, identity.IdenId, Value);
        }
        public void Edit(Identity identity, string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language, string Table)
        {
            if (String.Compare(identity.FirstName, firstName) != 0)
            {
                LogUpdate(Table, identity.IdenId, "FirstName", identity.FirstName, firstName);
                identity.FirstName = firstName;
            }
            if (String.Compare(identity.LastName, LastName) != 0)
            {
                LogUpdate(Table, identity.IdenId, "LastName", identity.LastName, LastName);
                identity.LastName = LastName;
            }
            if (String.Compare(identity.Company, Company) != 0)
            {
                LogUpdate(Table, identity.IdenId, "Company", identity.Company, Company);
                identity.Company = Company;
            }
            if (String.Compare(identity.Language.Code, Language) != 0)
            {
                var language = _context.Languages.Where(x => x.Code == Language).First();
                LogUpdate(Table, identity.IdenId, "Language", identity.Language.Code, Language);
                identity.Language = language;
            }
            if (string.Compare(identity.EMail, EMail) != 0)
            {
                LogUpdate(Table, identity.IdenId, "Email", identity.EMail, EMail);
                identity.EMail = EMail;
            }
            if (String.Compare(identity.UserID, UserID) != 0)
            {
                LogUpdate(Table, identity.IdenId, "UserID", identity.UserID, UserID);
                identity.UserID = UserID;
            }
            if (identity.Type.TypeID != type)
            {
                var Type = GetIdenityTypeByID(type);
                IdentityType newType = Type.ElementAt<IdentityType>(0);
                LogUpdate(Table, identity.IdenId, "Type", identity.Type.Type, newType.Type);
                identity.Type = newType;
            }
            _context.Identities.Update(identity);
            _context.SaveChanges();
        }
        public void Deactivate(Identity identity, string Reason, string Table)
        {
            identity.DeactivateReason = Reason;
            identity.Active = "Inactive";
            _context.Identities.Update(identity);
            _context.SaveChanges();
            string value = $"Identity width name: {identity.FirstName} , {identity.LastName}";
            LogDeactivate(Table, identity.IdenId, value, Reason);
        }
        public void Activate(Identity identity, string Table)
        {
            identity.DeactivateReason = null;
            identity.Active = "Active";
            _context.Identities.Update(identity);
            _context.SaveChanges();
            string value = $"Identity width name: {identity.FirstName} , {identity.LastName}";
            LogActivate(Table, identity.IdenId, value);
        }
        public bool IsExisting(Identity identity, string UserID = "")
        {
            bool result = false;
            if (String.IsNullOrEmpty(UserID) && String.Compare(identity.UserID, UserID) != 0)
            {
                var idenities = _context.Identities
                .Where(x => x.UserID == UserID)
                .ToList();
                if (idenities.Count > 0)
                    result = true;
                else
                    result = false;
            }
            return result;
        }
        public ICollection<IdentityType> GetIdenityTypeByID(int id)
        {
            var types = _context.IdentityTypes
                .Where(x => x.TypeID == id)
                .ToList();
            return types;
        }
        public List<SelectListItem> ListAllFreeAccounts()
        {
            List<SelectListItem> accounts = new();
            var freeAccounts = _context.Accounts
                .Include(x => x.Application)
                .Where(x => x.active == 1)
                .ToList();
            var idenaccounts = _context.IdenAccounts
                .Include(x => x.Account)
                .Where(x => x.ValidFrom <= DateTime.Now && x.ValidUntil >= DateTime.Now)
                .ToList();
            foreach (var account in freeAccounts)
            {
                foreach (var iden in idenaccounts)
                {
                    if (!(iden.Account.AccID == account.AccID))
                        accounts.Add(new(account.AccID + " " + account.Application.Name, account.AccID.ToString()));
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
        public void AssignAccount2Idenity(Identity identity, int AccID, DateTime ValidFrom, DateTime ValidUntil, string Table)
        {
            var Account = GetAccountByID(AccID).First<Account>();
            identity.Accounts.Add(new()
            {
                Account = Account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil
            });
            LogAssignIden2Account(Table, identity.IdenId, identity, Account);
            LogAssignAccount2Identity("account", AccID, Account, identity);
        }
        public void ReleaseAccount4Identity(Identity identity, Account account, int idenAccountID, string Table)
        {
            var idenAccount = _context.IdenAccounts.
                Include(x => x.Identity)
                .Where(x => x.ID == idenAccountID)
                .Single<IdenAccount>();
            idenAccount.ValidUntil = new DateTime();
            _context.IdenAccounts.Update(idenAccount);
            _context.SaveChanges();
            LogReleaseAccountFromIdentity(Table, identity.IdenId, identity, account);
            LogReleaseIdentity4Account("account", account.AccID, identity, account);
        }
        public List<Account> GetAccountByID(int ID)
        {
            List<Account> accounts = _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == ID)
                .ToList();
            return accounts;
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
