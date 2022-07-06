using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class IdentityService : LogService
    {
        public IdentityService(CMDBContext context) : base(context)
        {
        }
        public async Task<ICollection<Identity>> ListAll()
        {
            List<Identity> identities = await _context.Identities
                .Include(x => x.Type)
                .ToListAsync();
            return identities;
        }
        public async Task<ICollection<Identity>> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<Identity> list = await _context.Identities
                .Include(x => x.Type)
                .Where(x => EF.Functions.Like(x.Name, searhterm) || EF.Functions.Like(x.UserID, searhterm)
                    || EF.Functions.Like(x.EMail, searhterm) || EF.Functions.Like(x.Type.Type, searhterm))
                .ToListAsync();
            return list;
        }
        public async Task<ICollection<Identity>> GetByID(int id)
        {
            List<Identity> identities = await _context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.IdenId == id)
                .ToListAsync();
            return identities;
        }
        public List<SelectListItem> ListActiveIdentityTypes()
        {
            List<SelectListItem> identityTypes = new();
            List<IdentityType> types = _context.Types.OfType<IdentityType>().Where(x => x.active == 1).ToList();
            foreach (var type in types)
            {
                identityTypes.Add(new SelectListItem(type.Type + " " + type.Description, type.TypeId.ToString()));
            }
            return identityTypes;
        }
        public List<SelectListItem> ListAllActiveLanguages()
        {
            List<SelectListItem> langs = new();
            List<Language> languages = _context.Languages.ToList();
            foreach (var language in languages)
            {
                langs.Add(new SelectListItem(language.Description, language.Code));
            }
            return langs;
        }
        public async Task<List<Device>> ListAllFreeDevices()
        {
            List<Device> devices = new();

            var Laptops = await _context.Devices.OfType<Laptop>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.Identity == null)
                .ToListAsync();
            foreach (var laptop in Laptops)
            {
                devices.Add(laptop);
            }

            var Desktops = await _context.Devices.OfType<Desktop>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.Identity == null)
                .ToListAsync();
            foreach (var desktop in Desktops)
            {
                devices.Add(desktop);
            }
            var screens = await _context.Devices.OfType<Screen>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.Identity == null)
                .ToListAsync();
            foreach (var screen in screens)
            {
                devices.Add(screen);
            }
            var dockings = await _context.Devices.OfType<Docking>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.Identity == null)
                .ToListAsync();
            foreach(var docking in dockings)
            {
                devices.Add(docking);
            }
            var tokens = await _context.Devices.OfType<Token>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.Identity == null)
                .ToListAsync();
            foreach (var token in tokens)
            {
                devices.Add(token);
            }
            return devices;
        }
        public void GetAssingedDevices(Identity identity)
        {
            identity.Devices = _context.Identities
                .Include(x => x.Devices)
                .ThenInclude(x => x.Category)
                .Include(x => x.Devices)
                .ThenInclude(x => x.Type)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Devices)
                .ToList();
            identity.Mobiles = _context.Identities
                .Include(x => x.Mobiles)
                .ThenInclude(x => x.Subscriptions)
                .Include(x => x.Mobiles)
                .ThenInclude(x => x.MobileType)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Mobiles)
                .ToList();
            identity.Subscriptions = _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Include(x => x.Category)
                .Where(x => x.IdentityId == identity.IdenId)
                .ToList();
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
        public async Task Create(string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language, string Table)
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
                Language = Lang,
                LastModfiedAdmin = Admin
            };
            _context.Identities.Add(identity);
            await _context.SaveChangesAsync();
            string Value = "Identity width name: " + firstName + ", " + LastName;
            await LogCreate(Table, identity.IdenId, Value);
        }
        public async Task EditAsync(Identity identity, string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language, string Table)
        {
            identity.LastModfiedAdmin = Admin;
            if (String.Compare(identity.FirstName, firstName) != 0)
            {
                await LogUpdate(Table, identity.IdenId, "FirstName", identity.FirstName, firstName);
                identity.FirstName = firstName;
            }
            if (String.Compare(identity.LastName, LastName) != 0)
            {
                await LogUpdate(Table, identity.IdenId, "LastName", identity.LastName, LastName);
                identity.LastName = LastName;
            }
            if (String.Compare(identity.Company, Company) != 0)
            {
                await LogUpdate(Table, identity.IdenId, "Company", identity.Company, Company);
                identity.Company = Company;
            }
            if (String.Compare(identity.Language.Code, Language) != 0)
            {
                var language = _context.Languages.Where(x => x.Code == Language).First();
                await LogUpdate(Table, identity.IdenId, "Language", identity.Language.Code, Language);
                identity.Language = language;
            }
            if (string.Compare(identity.EMail, EMail) != 0)
            {
                await LogUpdate(Table, identity.IdenId, "Email", identity.EMail, EMail);
                identity.EMail = EMail;
            }
            if (String.Compare(identity.UserID, UserID) != 0)
            {
                await LogUpdate(Table, identity.IdenId, "UserID", identity.UserID, UserID);
                identity.UserID = UserID;
            }
            if (identity.Type.TypeId != type)
            {
                var Type = GetIdenityTypeByID(type);
                IdentityType newType = Type.First();
                await LogUpdate(Table, identity.IdenId, "Type", identity.Type.Type, newType.Type);
                identity.Type = newType;
            }
            _context.Identities.Update(identity);
            await _context.SaveChangesAsync();
        }
        public async Task Deactivate(Identity identity, string Reason, string Table)
        {
            identity.LastModfiedAdmin = Admin;
            identity.DeactivateReason = Reason;
            identity.Active = State.Inactive;
            _context.Identities.Update(identity);
            await _context.SaveChangesAsync();
            string value = $"Identity width name: {identity.Name}";
            await LogDeactivate(Table, identity.IdenId, value, Reason);
        }
        public async Task Activate(Identity identity, string Table)
        {
            identity.LastModfiedAdmin = Admin;
            identity.DeactivateReason = null;
            identity.Active = State.Active;
            _context.Identities.Update(identity);
            await _context.SaveChangesAsync();
            string value = $"Identity width name: {identity.Name}";
            await LogActivate(Table, identity.IdenId, value);
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
            var types = _context.Types
                .OfType<IdentityType>()
                .Where(x => x.TypeId == id)
                .ToList();
            return types;
        }
        public async Task ReleaseDevices(Identity identity, List<Device> devices, string table)
        {
            foreach (Device device in devices)
            {
                device.Identity = null;
                identity.Devices.Remove(device);
                await _context.SaveChangesAsync();
                switch (device.Category.Category)
                {
                    case "Laptop":
                        await LogReleaseIdentityFromDevice("laptop", identity, device);
                        break;
                    case "Desktop":
                        await LogReleaseIdentityFromDevice("desktop", identity, device);
                        break;
                    case "Token":
                        await LogReleaseIdentityFromDevice("token", identity, device);
                        break;
                    case "Docking station":
                        await LogReleaseIdentityFromDevice("docking", identity, device);
                        break;
                    case "Monitor":
                        await LogReleaseIdentityFromDevice("screen", identity, device);
                        break;
                    default:
                        throw new Exception("Category not know");
                }
                await LogReleaseDeviceFromIdenity(table, device,identity);
            }
        }
        public async Task<List<SelectListItem>> ListAllFreeAccounts()
        {
            List<SelectListItem> accounts = new();
            var freeAccounts = await _context.Accounts
                .Include(x => x.Application)
                .Where(x => x.active == 1)
                .ToListAsync();
            var idenaccounts = await _context.IdenAccounts
                .Include(x => x.Account)
                .Where(x => x.ValidFrom <= DateTime.Now && x.ValidUntil >= DateTime.Now)
                .ToListAsync();
            foreach (var account in freeAccounts)
            {
                foreach (var iden in idenaccounts)
                {
                    if (!(iden.Account.AccID == account.AccID))
                        accounts.Add(new(account.UserID + " " + account.Application.Name, account.AccID.ToString()));
                }
            }
            return accounts;
        }
        public bool IsPeriodOverlapping(int? IdenID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            if (IdenID == null)
                throw new Exception("Missing required id");
            else
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
            return result;
        }
        public async Task AssignAccount2Idenity(Identity identity, int AccID, DateTime ValidFrom, DateTime ValidUntil, string Table)
        {
            var Accounts = await GetAccountByID(AccID);
            var Account = Accounts.First<Account>();
            identity.Accounts.Add(new()
            {
                Account = Account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil,
                LastModifiedAdmin = Admin
            });
            identity.LastModfiedAdmin = Admin;
            Account.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            await LogAssignIden2Account(Table, identity.IdenId, identity, Account);
            await LogAssignAccount2Identity("account", AccID, Account, identity);
        }
        public async Task AssignDevice(Identity identity, List<Device> devicesToAdd, string table)
        {
            identity.LastModfiedAdmin = Admin;
            identity.Devices = devicesToAdd;
            foreach (var device in devicesToAdd)
            {
                device.LastModfiedAdmin = Admin;
                switch (device.Category.Category)
                {
                    case "Laptop":
                        await LogAssignIdentity2Device("laptop", identity, device);
                        break;
                    case "Desktop":
                        await LogAssignIdentity2Device("desktop", identity, device);
                        break;
                    case "Token":
                        await LogAssignIdentity2Device("token", identity, device);
                        break;
                    case "Docking station":
                        await LogAssignIdentity2Device("docking", identity, device);
                        break;
                    case "Monitor":
                        await LogAssignIdentity2Device("screen", identity, device);
                        break;
                    default:
                        throw new Exception("Category not know");
                }
                await LogAssignDevice2Identity(table, device, identity);
            }
            await _context.SaveChangesAsync();
        }
        public async Task ReleaseAccount4Identity(Identity identity, Account account, int idenAccountID, string Table)
        {
            var idenAccount = _context.IdenAccounts.
                Include(x => x.Identity)
                .Where(x => x.ID == idenAccountID)
                .Single<IdenAccount>();
            idenAccount.ValidUntil = DateTime.Now;
            idenAccount.LastModifiedAdmin = Admin;
            account.LastModfiedAdmin = Admin;
            identity.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            await LogReleaseAccountFromIdentity(Table, identity.IdenId, identity, account);
            await LogReleaseIdentity4Account("account", account.AccID, identity, account);
        }
        public async Task<List<Account>> GetAccountByID(int ID)
        {
            List<Account> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == ID)
                .ToListAsync();
            return accounts;
        }
        public async Task<List<IdenAccount>> GetIdenAccountByID(int id)
        {
            var idenAccounts = await _context.IdenAccounts
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Include(x => x.Account)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.ID == id)
                .ToListAsync();
            return idenAccounts;
        }
        public async Task<Device> GetDevice(string assetTag)
        {
            Device device = await _context.Devices
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.AssetTag == assetTag)
                .FirstOrDefaultAsync();
            return device;
        }
    }
}
