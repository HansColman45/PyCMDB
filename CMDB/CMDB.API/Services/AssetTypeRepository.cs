using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class AssetTypeRepository : GenericRepository, IAssetTypeRepository
    {
        private readonly string table = "assettype";
        public AssetTypeRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<IEnumerable<AssetTypeDTO>> GetAll()
        {
            var types =  await _context.AssetTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Select(x => ConvertType(x))
                .ToListAsync();
            return types;
        }
        public async Task<IEnumerable<AssetTypeDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            return await _context.AssetTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Where(x => EF.Functions.Like(x.Vendor, searhterm)
                    || EF.Functions.Like(x.Type, searhterm)
                    || EF.Functions.Like(x.Category.Category, searhterm))
                .Select(x => ConvertType(x))
                .ToListAsync();
        }
        public AssetTypeDTO Create(AssetTypeDTO assetTypeDTO)
        {
            AssetType type = new()
            {
                CategoryId = assetTypeDTO.AssetCategory.Id,
                LastModifiedAdminId = TokenStore.AdminId,
                Vendor = assetTypeDTO.Vendor,
                Type = assetTypeDTO.Type,
                active = 1
            };
            type.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.CreateLogLine($"{assetTypeDTO.AssetCategory.Category} type Vendor: {type.Vendor} and type {type.Type}",TokenStore.Admin.Account.UserID,table)
            });
            _context.AssetTypes.Add(type);
            return assetTypeDTO;
        }
        public async Task<AssetTypeDTO> Update(AssetTypeDTO assetTypeDTO)
        {
            var oldType = await GetTypeById(assetTypeDTO);
            if(string.Compare(oldType.Type, assetTypeDTO.Type) != 0)
            {
                var logtext = GenericLogLineCreator.UpdateLogLine("Type", oldType.Type, assetTypeDTO.Type, TokenStore.Admin.Account.UserID, table);
                oldType.Type = assetTypeDTO.Type;
                oldType.LastModifiedAdminId = assetTypeDTO.LastModifiedAdminId;
                oldType.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logtext
                });
            }
            if(string.Compare(oldType.Vendor, assetTypeDTO.Vendor) != 0)
            {
                var logtext = GenericLogLineCreator.UpdateLogLine("Vendor", oldType.Vendor, assetTypeDTO.Vendor, TokenStore.Admin.Account.UserID, table);
                oldType.Vendor = assetTypeDTO.Vendor;
                oldType.LastModifiedAdminId = assetTypeDTO.LastModifiedAdminId;
                oldType.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logtext
                });
            }
            _context.AssetTypes.Update(oldType);
            return assetTypeDTO;
        }
        public async Task<AssetTypeDTO> Deactivate(AssetTypeDTO assetTypeDTO, string reason)
        {
            var oldType = await GetTypeById(assetTypeDTO);
            oldType.active = 0;
            oldType.DeactivateReason = reason;
            oldType.LastModifiedAdminId = assetTypeDTO.LastModifiedAdminId;
            oldType.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.DeleteLogLine($"{assetTypeDTO.AssetCategory.Category} type Vendor: {assetTypeDTO.Vendor} and type {assetTypeDTO.Type}", TokenStore.Admin.Account.UserID, reason, table),
                LogDate = DateTime.Now
            });
            _context.AssetTypes.Update(oldType);
            return assetTypeDTO;
        }
        public async Task<AssetTypeDTO> Activate(AssetTypeDTO assetTypeDTO)
        {
            var oldType = await GetTypeById(assetTypeDTO);
            oldType.active = 1;
            oldType.DeactivateReason = "";
            oldType.LastModifiedAdminId = assetTypeDTO.LastModifiedAdminId;
            oldType.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ActivateLogLine($"{assetTypeDTO.AssetCategory.Category} type Vendor: {assetTypeDTO.Vendor} and type {assetTypeDTO.Type}", TokenStore.Admin.Account.UserID, table)
            });
            return assetTypeDTO;
        }
        public async Task<IEnumerable<AssetTypeDTO>> GetByCategory(string category)
        {
            return await _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.Category.Category == category)
                .Select(x => ConvertType(x))
                .ToListAsync();
        }
        public async Task<AssetTypeDTO> GetById(int id)
        {
            var type = await _context.AssetTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Where(x => x.TypeID == id)
                .Select(x => ConvertType(x))
                .FirstOrDefaultAsync();
            if(type is not null)
                GetLogs(table,id,type);
            return type;
        }
        public static AssetTypeDTO ConvertType(AssetType assetType)
        {
            return new()
            {
                Type = assetType.Type,
                Active = assetType.active,
                DeactivateReason = assetType.DeactivateReason,
                Vendor = assetType.Vendor,
                LastModifiedAdminId = assetType.LastModifiedAdminId,
                TypeID = assetType.TypeID,
                CategoryId = assetType.CategoryId,
                AssetCategory = new()
                {
                    Category = assetType.Category.Category,
                    Active = assetType.Category.active,
                    DeactivateReason = assetType.Category.DeactivateReason,
                    Id = assetType.Category.Id,
                    LastModifiedAdminId= assetType.LastModifiedAdminId,
                    Prefix = assetType.Category.Prefix
                }
            };
        }

        private async Task<AssetType> GetTypeById(AssetTypeDTO dTO)
        {
            return await _context.AssetTypes.Where(x => x.TypeID == dTO.TypeID).FirstAsync();
        }
    }
}
