using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface ISubscriptionTypeRepository
    {
        Task<IEnumerable<SubscriptionTypeDTO>> GetAll();
        Task<IEnumerable<SubscriptionTypeDTO>> GetAll(string search);
        Task<SubscriptionTypeDTO?> GetById(int id);
        SubscriptionTypeDTO Create(SubscriptionTypeDTO subscriptionDTO);
        Task<SubscriptionTypeDTO> Update(SubscriptionTypeDTO subscriptionDTO);
        Task<SubscriptionTypeDTO> Deactivate(SubscriptionTypeDTO subscriptionDTO, string reason);
        Task<SubscriptionTypeDTO> Activate(SubscriptionTypeDTO subscriptionDTO);
    }
    public class SubscriptionTypeRepository : GenericRepository, ISubscriptionTypeRepository
    {
        private readonly string table = "subscriptiontype";
        public SubscriptionTypeRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<SubscriptionTypeDTO>> GetAll()
        {
            var types = await _context.SubscriptionTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Select(x => ConvertType(x))
                .ToListAsync();
            return types;
        }

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

        public async Task<SubscriptionTypeDTO?> GetById(int id)
        {
            var types = await _context.SubscriptionTypes.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => ConvertType(x))
                .FirstOrDefaultAsync();
            return types;
        }
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
            _context.SubscriptionTypes.Add(type);
            return subscriptionTypeDTO;
        }
        public async Task<SubscriptionTypeDTO> Update(SubscriptionTypeDTO subscriptionTypeDTO)
        {
            var type = await GetSubscriptionType(subscriptionTypeDTO.Id);
            return subscriptionTypeDTO;
        }
        public async Task<SubscriptionTypeDTO> Deactivate(SubscriptionTypeDTO subscriptionTypeDTO, string reason)
        {
            var type = await GetSubscriptionType(subscriptionTypeDTO.Id);
            type.active = 0;
            type.DeactivateReason = reason;
            type.LastModifiedAdminId = TokenStore.AdminId;
            type.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ActivateLogLine($"{subscriptionTypeDTO.AssetCategory.Category} with {subscriptionTypeDTO.Provider} and {subscriptionTypeDTO.Type}", TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now
            });
            _context.SubscriptionTypes.Update(type);
            return subscriptionTypeDTO;
        }
        public async Task<SubscriptionTypeDTO> Activate(SubscriptionTypeDTO subscriptionTypeDTO)
        {
            var type = await GetSubscriptionType(subscriptionTypeDTO.Id);
            return subscriptionTypeDTO;
        }

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
