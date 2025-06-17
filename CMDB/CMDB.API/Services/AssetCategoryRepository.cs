using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// Repository for AssetCategory
    /// </summary>
    public class AssetCategoryRepository : GenericRepository, IAssetCategoryRepository
    {
        private AssetCategoryRepository()
        {
        }

        private readonly string table = "assetcategory";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public AssetCategoryRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc />
        public async Task<bool> IsCategoryExisting(AssetCategoryDTO assetCategory)
        {
            List<AssetCategory> Catogories;
            var dto = await GetById(assetCategory.Id);
            if (dto is null)
            {
                Catogories = _context.AssetCategories.Where(x => x.Category == assetCategory.Category).ToList();
            }
            else if (string.Compare(dto.Category, assetCategory.Category) != 0)
            {
                Catogories = _context.AssetCategories.Where(x => x.Category == assetCategory.Category).ToList();
            }
            else
                Catogories = _context.AssetCategories.Where(x => x.Category == assetCategory.Category).ToList();
            if (Catogories.Count > 0)
                return true;
            else
                return false;
        }
        /// <inheritdoc />
        public async Task<IEnumerable<AssetCategoryDTO>> GetAll()
        {
            var category = await _context.AssetCategories.AsNoTracking()
                .Select(x => ConvertCategory(x))
                .ToListAsync();
            return category;
        }
        /// <inheritdoc />
        public async Task<IEnumerable<AssetCategoryDTO>> GetAll(string search)
        {
            var category = await _context.AssetCategories.AsNoTracking()
                .Select(x => ConvertCategory(x))
                .ToListAsync();
            return category;
        }
        /// <inheritdoc />
        public async Task<AssetCategoryDTO> GetByCategory(string category)
        {
            return await _context.AssetCategories.AsNoTracking()
                .Where(x => x.Category == category)
                .Select(x => ConvertCategory(x))
                .FirstAsync();
        }
        /// <inheritdoc />
        public async Task<AssetCategoryDTO> GetById(int id)
        {
            var category = await _context.AssetCategories.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => ConvertCategory(x))
                .FirstOrDefaultAsync();
            if (category is not null)
            {
                GetLogs(table, id, category);
            }
            return category;
        }
        /// <summary>
        /// This will convert the DTO to the entity
        /// </summary>
        /// <param name="dto"><see cref="AssetCategoryDTO"/></param>
        /// <returns><see cref="AssetCategory"/></returns>
        public static AssetCategory ConvertDTO(AssetCategoryDTO dto)
        {
            return new()
            {
                LastModifiedAdminId = dto.LastModifiedAdminId,
                Prefix = dto.Prefix,
                active = dto.Active,
                Id = dto.Id,
                DeactivateReason = dto.DeactivateReason
            };
        }
        /// <summary>
        /// This will convert the entity to the DTO
        /// </summary>
        /// <param name="assetCategory"><see cref="AssetCategory"/></param>
        /// <returns><see cref="AssetCategoryDTO"/></returns>
        public static AssetCategoryDTO ConvertCategory(AssetCategory assetCategory)
        {
            return new()
            {
                Category = assetCategory.Category,
                Active = assetCategory.active,
                DeactivateReason = assetCategory.DeactivateReason,
                Id = assetCategory.Id,
                LastModifiedAdminId = assetCategory.LastModifiedAdminId,
                Prefix = assetCategory.Prefix
            };
        }
        /// <inheritdoc />
        public AssetCategoryDTO Create(AssetCategoryDTO assetCategoryDTO)
        {
            AssetCategory category = new()
            {
                active = 1,
                Category = assetCategoryDTO.Category,
                Prefix = assetCategoryDTO.Prefix,
                LastModifiedAdminId = TokenStore.AdminId,
            };
            category.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.CreateLogLine($"Assetcategory {assetCategoryDTO.Category} with prefix {assetCategoryDTO.Prefix}", TokenStore.Admin.Account.UserID, table)
            });
            _context.AssetCategories.Add(category);
            return assetCategoryDTO;
        }
        /// <inheritdoc />
        public async Task<AssetCategoryDTO> Update(AssetCategoryDTO assetCategoryDTO)
        {
            var oldType = await GetCategory(assetCategoryDTO.Id);
            if (string.Compare(oldType.Category, assetCategoryDTO.Category) != 0)
            {
                var logText = GenericLogLineCreator.UpdateLogLine("Category", oldType.Category, assetCategoryDTO.Category, TokenStore.Admin.Account.UserID, table);
                oldType.Category = assetCategoryDTO.Category;
                oldType.LastModifiedAdminId = TokenStore.AdminId;
                oldType.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logText,
                });

            }
            if (string.Compare(oldType.Prefix, assetCategoryDTO.Prefix) != 0)
            {
                var logText = GenericLogLineCreator.UpdateLogLine("Prefix", oldType.Prefix, assetCategoryDTO.Prefix, TokenStore.Admin.Account.UserID, table);
                oldType.Prefix = assetCategoryDTO.Prefix;
                oldType.LastModifiedAdminId = TokenStore.AdminId;
                oldType.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logText,
                });
            }
            _context.AssetCategories.Update(oldType);
            return assetCategoryDTO;
        }
        /// <inheritdoc />
        public async Task<AssetCategoryDTO> Delete(AssetCategoryDTO assetCategoryDTO, string reason)
        {
            var cat = await GetCategory(assetCategoryDTO.Id);
            cat.active = 0;
            cat.DeactivateReason = reason;
            cat.LastModifiedAdminId = TokenStore.AdminId;
            cat.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.DeleteLogLine($"Assetcategory {assetCategoryDTO.Category} with prefix {assetCategoryDTO.Prefix}", TokenStore.Admin.Account.UserID, reason, table)
            });
            return assetCategoryDTO;
        }
        /// <inheritdoc />
        public async Task<AssetCategoryDTO> Activate(AssetCategoryDTO assetCategoryDTO)
        {
            var cat = await GetCategory(assetCategoryDTO.Id);
            cat.active = 1;
            cat.DeactivateReason = "";
            cat.LastModifiedAdminId = TokenStore.AdminId;
            cat.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ActivateLogLine($"Assetcategory {assetCategoryDTO.Category} with prefix {assetCategoryDTO.Prefix}", TokenStore.Admin.Account.UserID, table)
            });
            return assetCategoryDTO;
        }
        private async Task<AssetCategory> GetCategory(int Id)
        {
            return await _context.AssetCategories.Where(x => x.Id == Id).FirstAsync();
        }
    }
}
