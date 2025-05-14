using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// Repository for SubscriptionType
    /// </summary>
    public class SubscriptionTypeRepository : GenericRepository, ISubscriptionTypeRepository
    {
        private SubscriptionTypeRepository()
        {
        }

        private readonly string table = "subscriptiontype";
        /// <summary>
        /// Constructor for the SubscriptionTypeRepository
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public SubscriptionTypeRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<SubscriptionTypeDTO>> GetAll()
        {
            var types = await _context.SubscriptionTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Select(x => ConvertType(x))
                .ToListAsync();
            return types;
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<SubscriptionTypeDTO>> GetAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            var types = await _context.SubscriptionTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Where(x => EF.Functions.Like(x.Category.Category, searhterm) || EF.Functions.Like(x.Description, searhterm) || EF.Functions.Like(x.Type, searhterm) || EF.Functions.Like(x.Provider, searhterm))
                .Select(x => ConvertType(x))
                .ToListAsync();
            return types;
        }
        /// <inheritdoc/>
        public async Task<SubscriptionTypeDTO> GetById(int id)
        {
            var types = await _context.SubscriptionTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => ConvertType(x))
                .FirstOrDefaultAsync();
            if (types is not null)
            {
                GetLogs(table, id, types);
            }
            return types;
        }
        /// <inheritdoc/>
        public SubscriptionTypeDTO Create(SubscriptionTypeDTO subscriptionTypeDTO)
        {
            SubscriptionType type = new()
            {
                active = 1,
                LastModifiedAdminId = TokenStore.AdminId,
                AssetCategoryId = subscriptionTypeDTO.AssetCategory.Id,
                Description = subscriptionTypeDTO.Description,
                Type = subscriptionTypeDTO.Type,
                Provider = subscriptionTypeDTO.Provider,
            };
            type.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.CreateLogLine($"{subscriptionTypeDTO.AssetCategory.Category} with {subscriptionTypeDTO.Provider} and {subscriptionTypeDTO.Type}",
                    TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now
            });
            _context.SubscriptionTypes.Add(type);
            return subscriptionTypeDTO;
        }
        /// <inheritdoc/>
        public async Task<SubscriptionTypeDTO> Update(SubscriptionTypeDTO subscriptionTypeDTO)
        {
            var type = await GetSubscriptionType(subscriptionTypeDTO.Id);
            if (string.Compare(type.Type, subscriptionTypeDTO.Type) != 0)
            {
                var logText = GenericLogLineCreator.UpdateLogLine("Type", type.Type, subscriptionTypeDTO.Type, TokenStore.Admin.Account.UserID, table);
                type.Type = subscriptionTypeDTO.Type;
                type.LastModifiedAdminId = TokenStore.AdminId;
                type.Logs.Add(new()
                {
                    LogText = logText,
                    LogDate = DateTime.Now,
                });
            }
            if (string.Compare(type.Provider, subscriptionTypeDTO.Provider) != 0)
            {
                var logText = GenericLogLineCreator.UpdateLogLine("Provider", type.Provider, subscriptionTypeDTO.Provider, TokenStore.Admin.Account.UserID, table);
                type.Provider = subscriptionTypeDTO.Provider;
                type.LastModifiedAdminId = TokenStore.AdminId;
                type.Logs.Add(new()
                {
                    LogText = logText,
                    LogDate = DateTime.Now,
                });
            }
            if (string.Compare(type.Description, subscriptionTypeDTO.Description) != 0)
            {
                var logText = GenericLogLineCreator.UpdateLogLine("Description", type.Description, subscriptionTypeDTO.Description, TokenStore.Admin.Account.UserID, table);
                type.Description = subscriptionTypeDTO.Description;
                type.LastModifiedAdminId = TokenStore.AdminId;
                type.Logs.Add(new()
                {
                    LogText = logText,
                    LogDate = DateTime.Now,
                });
            }
            _context.SubscriptionTypes.Update(type);
            return subscriptionTypeDTO;
        }
        /// <inheritdoc/>
        public async Task<SubscriptionTypeDTO> Deactivate(SubscriptionTypeDTO subscriptionTypeDTO, string reason)
        {
            var type = await GetSubscriptionType(subscriptionTypeDTO.Id);
            type.active = 0;
            type.DeactivateReason = reason;
            type.LastModifiedAdminId = TokenStore.AdminId;
            type.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.DeleteLogLine($"{subscriptionTypeDTO.AssetCategory.Category} with {subscriptionTypeDTO.Provider} and {subscriptionTypeDTO.Type}", TokenStore.Admin.Account.UserID, reason, table),
                LogDate = DateTime.Now
            });
            _context.SubscriptionTypes.Update(type);
            return subscriptionTypeDTO;
        }
        /// <inheritdoc/>
        public async Task<SubscriptionTypeDTO> Activate(SubscriptionTypeDTO subscriptionTypeDTO)
        {
            var type = await GetSubscriptionType(subscriptionTypeDTO.Id);
            type.active = 1;
            type.DeactivateReason = "";
            type.LastModifiedAdminId = TokenStore.AdminId;
            type.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ActivateLogLine($"{subscriptionTypeDTO.AssetCategory.Category} with {subscriptionTypeDTO.Provider} and {subscriptionTypeDTO.Type}", TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now
            });
            _context.SubscriptionTypes.Update(type);
            return subscriptionTypeDTO;
        }
        /// <inheritdoc/>
        public async Task<bool> IsExisting(SubscriptionTypeDTO subscriptionType)
        {
            var type = await GetSubscriptionType(subscriptionType.Id);
            if (type is null)
            {
                var types = _context.SubscriptionTypes.Where(x => x.Type == subscriptionType.Type && x.Provider == subscriptionType.Provider).ToList();
                if (types.Count >0)
                    return true;
                else 
                    return false;
            }
            else
            {
                if(string.Compare(type.Type,subscriptionType.Type) !=0 && string.Compare(type.Provider,subscriptionType.Provider) != 0)
                {
                    var types = _context.SubscriptionTypes.Where(x => x.Type == subscriptionType.Type && x.Provider == subscriptionType.Provider).ToList();
                    if (types.Count > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }
        /// <summary>
        /// This will convert a SubscriptionType to a SubscriptionTypeDTO
        /// </summary>
        /// <param name="subscriptionType"></param>
        /// <returns></returns>
        public static SubscriptionTypeDTO ConvertType(SubscriptionType subscriptionType)
        {
            return new()
            {
                Active = subscriptionType.active,
                DeactivateReason = subscriptionType.DeactivateReason,
                Description = subscriptionType.Description,
                Id = subscriptionType.Id,
                LastModifiedAdminId = subscriptionType.LastModifiedAdminId,
                Provider = subscriptionType.Provider,
                Type = subscriptionType.Type,
                AssetCategory = new()
                {
                    Active = subscriptionType.Category.active,
                    Category = subscriptionType.Category.Category,
                    DeactivateReason = subscriptionType.Category.DeactivateReason,
                    Id = subscriptionType.Category.Id,
                    LastModifiedAdminId= subscriptionType.Category.LastModifiedAdminId,
                    Prefix = subscriptionType.Category.Prefix
                }
            };
        }

        private async Task<SubscriptionType> GetSubscriptionType(int id)
        {
            return await _context.SubscriptionTypes.FirstAsync(x => x.Id == id);
        }
    }
}
