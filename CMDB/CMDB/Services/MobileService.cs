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
            await LogCreate(table, mobile.IMEI, value);
        }
        public AssetType ListAssetTypeById(int id)
        {
            var devices = _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == id)
                .FirstOrDefault();
            return devices;
        }
        public bool IsMobileExisting(Mobile mobile)
        {
            bool result = false;
            var mobiles = _context.Mobiles.Where(x => x.IMEI == mobile.IMEI).ToList();
            if (mobiles.Count > 0)
                result = true;
            return result;
        }
        public List<Mobile> GetMobileById(int imei)
        {
            List<Mobile> mobiles = _context.Mobiles
                .Include(x => x.Identity)
                .Include(x => x.Category)
                .Include(x => x.MobileType)
                .Where(x => x.IMEI == imei)
                .ToList();
            return mobiles;
        }
        public void GetAssignedIdentity(Mobile mobile)
        {
            mobile.Identity = _context.Mobiles
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.IMEI == mobile.IMEI)
                .Select(x => x.Identity).First();
        }
    }
}
