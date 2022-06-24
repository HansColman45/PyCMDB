using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _context.Mobiles.Add(mobile);
            await _context.SaveChangesAsync();
            string value = $"{mobile.Category.Category} with type {mobile.MobileType}";
            await LogCreate(table, mobile.Id, value);
        }
        public async Task Update(Mobile mobile, int newImei, AssetType newAssetType, string table)
        {
            if(mobile.IMEI != newImei)
            {
                mobile.IMEI = newImei;
                await LogUpdate(table, mobile.Id, "IMEI", mobile.IMEI.ToString(), newImei.ToString());
            }
            if(mobile.MobileType != newAssetType)
            {
                mobile.MobileType = newAssetType;
                await LogUpdate(table, mobile.Id, "Type", mobile.MobileType.ToString(), newAssetType.ToString());
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
            await LogDeactivate(table,mobile.Id,value,reason);
        }
        public async Task Activate(Mobile mobile, string table)
        {
            mobile.LastModfiedAdmin = Admin;
            mobile.Active = State.Active;
            mobile.DeactivateReason = null;
            _context.Mobiles.Update(mobile);
            await _context.SaveChangesAsync();
            string value = $"{mobile.Category.Category} with type {mobile.MobileType}";
            await LogActivate(table, mobile.Id, value);
        }
        public AssetType ListAssetTypeById(int id)
        {
            var devices = _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == id)
                .FirstOrDefault();
            return devices;
        }
        public bool IsMobileExisting(Mobile mobile, int imei = 0)
        {
            bool result = false;
            if (imei != 0 && mobile.IMEI != imei)
            {
                var mobiles = _context.Mobiles.Where(x => x.IMEI == imei).ToList();
                if (mobiles.Count > 0)
                    result = true;
            }
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
                .Where(x => x.Id == id)
                .ToList();
            return mobiles;
        }
        public void GetAssignedIdentity(Mobile mobile)
        {
            mobile.Identity = _context.Mobiles
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.Id == mobile.Id)
                .Select(x => x.Identity).First();
        }
        public void GetAssignedSubscription(Mobile mobile)
        {
            mobile.Subscriptions = _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Include(x => x.Mobile)
                .Where(x => x.Mobile.Id == mobile.Id)
                .ToList();
        }
    }
}
