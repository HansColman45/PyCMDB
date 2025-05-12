using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMDB.API.Services
{
    public class KensingtonRepository : GenericRepository, IKensingtonRepository
    {
        private readonly string table = "kensington";
        public KensingtonRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task LogPdfFile(string pdfFile, int id)
        {
            var identity = await GetTrackedKensington(id);
            identity.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile)
            });
        }
        public async Task<List<KensingtonDTO>> ListAll()
        {
            var keys = await _context.Kensingtons
                .Include(x => x.Type)
                .ThenInclude(x => x.Category)
                .Select(x => Convert2DTO(x)).ToListAsync();
            return keys;
        }
        public async Task<List<KensingtonDTO>> ListAll(string search)
        {
            string searhterm = "%" + search + "%";
            var keys = await _context.Kensingtons
                .Include(x => x.Type)
                .ThenInclude(x => x.Category)
                .Where(x => EF.Functions.Like(x.SerialNumber,searhterm) || EF.Functions.Like(x.Type.Vendor, searhterm) || EF.Functions.Like(x.Type.Type,searhterm))
                .Select(x => Convert2DTO(x)).ToListAsync();
            return keys;
        }
        public async Task<KensingtonDTO> GetById(int id)
        {
            var key = await _context.Kensingtons
                .Include(x => x.Type)
                .ThenInclude(x => x.Category)
                .Where(x => x.KeyID == id).AsNoTracking()
                .Select(x => Convert2DTO(x))
                .FirstOrDefaultAsync();
            if (key is not null)
            {
                GetLogs("kensington", id, key);
                await GetDeviceInfo(key);
            }
            return key;
        }
        public KensingtonDTO Create(KensingtonDTO key)
        {
            Kensington kensington = new()
            {
                SerialNumber = key.SerialNumber,
                AmountOfKeys = key.AmountOfKeys,
                HasLock = key.HasLock,
                TypeId = key.Type.TypeID,
                active = 1,
                CategoryId = key.Type.CategoryId,
                LastModifiedAdminId = TokenStore.Admin.AdminId
            };
            kensington.Logs.Add(
                new()
                {
                    LogText = GenericLogLineCreator.CreateLogLine($"Kensington with serial number: {key.SerialNumber}", TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.Now
                }
            );
            _context.Kensingtons.Add(kensington);
            return key;
        }
        public async Task<KensingtonDTO> Update(KensingtonDTO key)
        {
            var identity = await GetTrackedKensington(key.KeyID);
            identity.LastModifiedAdminId = TokenStore.Admin.AdminId;
            if (string.Compare(key.SerialNumber, identity.SerialNumber) != 0)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", identity.SerialNumber,key.SerialNumber,TokenStore.Admin.Account.UserID,table);
                identity.SerialNumber = key.SerialNumber;
                identity.Logs.Add(
                    new()
                    {
                        LogText = logline,
                        LogDate = DateTime.Now
                    }
                );
            }
            if(key.AmountOfKeys != identity.AmountOfKeys)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("AmountOfKeys", identity.AmountOfKeys.ToString(), key.AmountOfKeys.ToString(), TokenStore.Admin.Account.UserID, table);
                identity.AmountOfKeys = key.AmountOfKeys;
                identity.Logs.Add(
                    new()
                    {
                        LogText = logline,
                        LogDate = DateTime.Now
                    }
                );
            }
            if(key.HasLock != identity.HasLock)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("HasLock", identity.HasLock.ToString(), key.HasLock.ToString(), TokenStore.Admin.Account.UserID, table);
                identity.HasLock = key.HasLock;
                identity.Logs.Add(
                    new()
                    {
                        LogText = logline,
                        LogDate = DateTime.Now
                    }
                );
            }

            _context.Kensingtons.Update(identity);
            return key;
        }
        public async Task<KensingtonDTO> DeActivate(KensingtonDTO key, string reason)
        {
            var identity = await GetTrackedKensington(key.KeyID);
            identity.active = 0;
            identity.DeactivateReason = reason;
            identity.LastModifiedAdminId = TokenStore.Admin.AdminId;
            identity.Logs.Add(
                new()
                {
                    LogText = GenericLogLineCreator.DeleteLogLine($"Kensington with serial number: {key.SerialNumber}", TokenStore.Admin.Account.UserID, reason, table),
                    LogDate = DateTime.Now
                }
            );
            _context.Kensingtons.Update(identity);
            return key;
        }
        public async Task<KensingtonDTO> Activate(KensingtonDTO key)
        {
            var identity = await GetTrackedKensington(key.KeyID);
            identity.active = 1;
            identity.DeactivateReason = "";
            identity.LastModifiedAdminId = TokenStore.Admin.AdminId;
            identity.Logs.Add(
                new()
                {
                    LogText = GenericLogLineCreator.ActivateLogLine($"Kensington with serial number: {key.SerialNumber}", TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.Now
                }
            );
            _context.Kensingtons.Update(identity);
            return key;
        }
        public async Task AssignDevice(KensingtonDTO key)
        {
            var kensington = await GetTrackedKensington(key.KeyID);
            var device = await _context.Devices.Where(x => x.AssetTag == key.Device.AssetTag).FirstAsync();
            var cat = await _context.AssetCategories.Where(x => x.Id == device.CategoryId).AsNoTracking().FirstAsync();
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{cat.Category} with {device.AssetTag}";
            string devicetable = cat.Category.ToLower();
            kensington.LastModifiedAdminId = TokenStore.AdminId;
            kensington.AssetTag = key.Device.AssetTag;
            kensington.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(keyinfo, deviceinfo, TokenStore.Admin.Account.UserID,table)
            });
            _context.Kensingtons.Update(kensington);
            device.LastModifiedAdminId = TokenStore.AdminId;
            device.Logs.Add(new() {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID,devicetable)
            });
            _context.Devices.Update(device);
        }
        public async Task ReleaseDevice(KensingtonDTO key)
        {
            var kensington = await GetTrackedKensington(key.KeyID);
            var device = await _context.Devices.Where(x => x.AssetTag == key.Device.AssetTag).FirstAsync();
            var cat = await _context.AssetCategories.Where(x => x.Id == device.CategoryId).AsNoTracking().FirstAsync();
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{cat.Category} with {device.AssetTag}";
            string devicetable = cat.Category.ToLower();
            kensington.LastModifiedAdminId = TokenStore.AdminId;
            kensington.AssetTag = null;
            kensington.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(keyinfo, deviceinfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Kensingtons.Update(kensington);
            device.LastModifiedAdminId = TokenStore.AdminId;
            device.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, devicetable)
            });
            _context.Devices.Update(device);
        }
        public async Task<List<KensingtonDTO>> ListAllFreeKeys()
        {
            return await _context.Kensingtons
                .Include(x => x.Type)
                .ThenInclude(x => x.Category)
                .Where(x => x.AssetTag == null)
                .Select(x => Convert2DTO(x)).ToListAsync();
        }
        public static KensingtonDTO Convert2DTO(Kensington entity)
        {
            return new KensingtonDTO
            {
                Active = entity.active,
                AmountOfKeys = entity.AmountOfKeys,
                DeactivateReason = entity.DeactivateReason,
                HasLock = entity.HasLock,
                KeyID = entity.KeyID,
                SerialNumber = entity.SerialNumber,
                LastModifiedAdminId = entity.LastModifiedAdminId,
                AssetTag = entity.AssetTag,
                Type = new()
                {
                    TypeID = entity.Type.TypeID,
                    Active = entity.Type.active,
                    CategoryId = entity.Type.CategoryId,
                    LastModifiedAdminId = entity.Type.LastModifiedAdminId,
                    Type = entity.Type.Type,
                    DeactivateReason = entity.Type.DeactivateReason,
                    Vendor = entity.Type.Vendor,
                    AssetCategory = new()
                    {
                        Active = entity.Type.Category.active,
                        Category = entity.Type.Category.Category,
                        DeactivateReason = entity.Type.Category.DeactivateReason,
                        Id = entity.Type.Category.Id,
                        LastModifiedAdminId = entity.Type.Category.LastModifiedAdminId
                    }
                }
            };
        }
        private async Task<Kensington> GetTrackedKensington(int id)
        {
            return await _context.Kensingtons.FirstAsync(x => x.KeyID == id);
        }
        private async Task GetDeviceInfo(KensingtonDTO key)
        {
            key.Device = await _context.Devices
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Include(x => x.Type)
                .Where(x => x.AssetTag == key.AssetTag)
                .Select(x => DeviceRepository.ConvertDevice(x)).FirstOrDefaultAsync();
        }
    }
}