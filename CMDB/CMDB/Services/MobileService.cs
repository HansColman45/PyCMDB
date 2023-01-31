using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMDB.Services
{
    public class MobileService : LogService
    {
        public MobileService(CMDBContext context) : base(context)
        {
        }
        public async Task<List<Mobile>> ListAll()
        {
            List<Mobile> mobiles = await _context.Mobiles
                .Include(x => x.Identity)
                .Include(x => x.Category)
                .Include(x => x.MobileType)
                .ToListAsync();
            return mobiles;
        }
        public async Task<List<Mobile>> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<Mobile> mobiles = await _context.Mobiles
                .Include(x => x.Identity)
                .Include(x => x.Category)
                .Include(x => x.MobileType)
                .Where(x => EF.Functions.Like(x.IMEI.ToString(), searhterm) || EF.Functions.Like(x.MobileType.Type, searhterm) || EF.Functions.Like(x.MobileType.Vendor, searhterm))
                .ToListAsync();
            return mobiles;
        }
        public async Task CreateNew(Mobile mobile, string table)
        {
            mobile.LastModfiedAdmin = Admin;
            mobile.IdentityId = 1;
            _context.Mobiles.Add(mobile);
            await _context.SaveChangesAsync();
            string value = $"{mobile.Category.Category} with type {mobile.MobileType}";
            await LogCreate(table, mobile.MobileId, value);
        }
        public async Task Update(Mobile mobile, long newImei, AssetType newAssetType, string table)
        {
            if(mobile.IMEI != newImei)
            {
                mobile.IMEI = newImei;
                await LogUpdate(table, mobile.MobileId, "IMEI", mobile.IMEI.ToString(), newImei.ToString());
            }
            if(mobile.MobileType != newAssetType)
            {
                mobile.MobileType = newAssetType;
                await LogUpdate(table, mobile.MobileId, "Type", mobile.MobileType.ToString(), newAssetType.ToString());
            }
            mobile.LastModfiedAdmin = Admin;
            _context.Mobiles.Update(mobile);
            await _context.SaveChangesAsync();
        }
        public async Task Deactivate(Mobile mobile, string reason, string table)
        {
            mobile.LastModfiedAdmin = Admin;
            mobile.Active = State.Inactive;
            mobile.DeactivateReason = reason;
            _context.Mobiles.Update(mobile);
            await _context.SaveChangesAsync();
            string value = $"{mobile.Category.Category} with type {mobile.MobileType}";
            await LogDeactivate(table,mobile.MobileId,value,reason);
        }
        public async Task Activate(Mobile mobile, string table)
        {
            mobile.LastModfiedAdmin = Admin;
            mobile.Active = State.Active;
            mobile.DeactivateReason = null;
            _context.Mobiles.Update(mobile);
            await _context.SaveChangesAsync();
            string value = $"{mobile.Category.Category} with type {mobile.MobileType}";
            await LogActivate(table, mobile.MobileId, value);
        }
        public AssetType ListAssetTypeById(int id)
        {
            var devices = _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == id)
                .FirstOrDefault();
            return devices;
        }
        public bool IsMobileExisting(Mobile mobile, long? imei = null)
        {
            bool result = false;
            if (imei != null && mobile.IMEI != imei)
            {
                var mobiles = _context.Mobiles.Where(x => x.IMEI == imei).ToList();
                if (mobiles.Count > 0)
                    result = true;
            }
            else if (imei != null && mobile.IMEI == imei)
                result = false;
            else
            {
                var mobiles = _context.Mobiles.Where(x => x.IMEI == mobile.IMEI).ToList();
                if (mobiles.Count > 0)
                    result = true;
            }
            return result;
        }
        public List<Mobile> GetMobileById(int id)
        {
            List<Mobile> mobiles = _context.Mobiles
                .Include(x => x.Identity)
                .Include(x => x.Category)
                .Include(x => x.MobileType)
                .Where(x => x.MobileId == id)
                .ToList();
            return mobiles;
        }
        public void GetAssignedIdentity(Mobile mobile)
        {
            mobile.Identity = _context.Mobiles
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.MobileId == mobile.MobileId)
                .Select(x => x.Identity).First();
        }
        public void GetAssignedSubscription(Mobile mobile)
        {
            mobile.Subscriptions = _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Include(x => x.Mobile)
                .Where(x => x.Mobile.MobileId == mobile.MobileId)
                .ToList();
        }
        public List<SelectListItem> ListFreeIdentities()
        {
            List<SelectListItem> identites = new();
            var idens = _context.Identities
                .Include(x => x.Mobiles)
                .Where(x => !x.Mobiles.Any())
                .Where(x => x.IdenId != 1)
                .ToList();
            foreach (var identity in idens)
            {
                identites.Add(new(identity.Name + " " + identity.UserID, identity.IdenId.ToString()));
            }
            return identites;
        }
        public async Task<Identity> GetIdentity(int IdenId) 
        {
            IdentityService identityService = new(_context);
            var idenitities = await identityService.GetByID(IdenId);
            return idenitities.FirstOrDefault();
        }
        public bool IsDeviceFree(Mobile mobile, bool checkSub = false)
        {
            bool result = false;
            if (!checkSub) { 
                var mobiles = _context.Mobiles.Where(x => x.MobileId == mobile.MobileId).First();
                if (mobiles.Identity is null || mobiles.MobileId == 1)
                    result = true;
            }
            if (checkSub)
            {
                var mobiles = _context.Mobiles
                    .Include(x => x.Subscriptions)
                    .Where(x => x.MobileId == mobile.MobileId).First();
                if(mobiles.Subscriptions.Count == 0)
                    result= true;
            }
            return result;
        }
        public async Task AssignIdentity2Mobile(Identity identity, Mobile mobile, string table)
        {
            identity.LastModfiedAdmin = Admin;
            mobile.LastModfiedAdmin = Admin;
            identity.Mobiles.Add(mobile);
            await _context.SaveChangesAsync();
            await LogAssignMobile2Identity("identity",mobile,identity);
            await LogAssignIdentity2Mobile(table, identity, mobile);
        }
        public async Task ReleaseIdenity(Mobile mobile, Identity identity, string table)
        {
            identity.LastModfiedAdmin = Admin;
            mobile.LastModfiedAdmin = Admin;
            identity.Mobiles.Remove(mobile);
            mobile.Identity = null;
            await _context.SaveChangesAsync();
            await LogReleaseMobileFromIdenity(table,mobile,identity);
            await LogReleaseIdentityFromMobile("identity", identity, mobile);
        }
        public List<SelectListItem> ListFreeMobileSubscriptions()
        {
            List<SelectListItem> identites = new();
            var subscriptions = _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Where(x => x.Category.Category == "Mobile Subscription" && x.Mobile == null).ToList();
            foreach(var subscription in subscriptions)
            {
                identites.Add(new($"Type: {subscription.SubscriptionType} on phonenumber: {subscription.PhoneNumber}",$"{subscription.SubscriptionId}"));
            }
            return identites;
        }
        public async Task AssignSubscription(Mobile mobile, Subscription subscription, string table)
        {
            mobile.LastModfiedAdmin = Admin;
            subscription.LastModfiedAdmin = Admin;
            mobile.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            await LogAssignSubscription2Mobile(table, mobile, subscription);
            await LogAssignMobile2Subscription("mobile",subscription,mobile);
        }

        public async Task<Subscription> GetSubribtion(int id)
        {
            SubscriptionService subscriptionService = new(_context);
            var subscriptions = await subscriptionService.GetByID(id);
            return subscriptions.FirstOrDefault();
        }
    }
}
