using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Solutions.BackupRestore.SharePointProtectionPolicies.Item.SiteProtectionUnits.Item;

namespace CMDB.API.Services
{
    /// <summary>
    /// Repository for Subscription
    /// </summary>
    public class SubscriptionRepository : GenericRepository, ISubscriptionRepository
    {
        private SubscriptionRepository()
        {
        }
        private readonly string table = "subscription";
        /// <summary>
        /// Constructor for the SubscriptionRepository
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public SubscriptionRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<SubscriptionDTO>> GetAll()
        {
            return await _context.Subscriptions
                 .Include(x => x.Category)
                 .Include(x => x.SubscriptionType)
                 .Select(x => ConvertSubscription(x))
                 .ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<SubscriptionDTO>> GetAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            return await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .Where(x => EF.Functions.Like(x.PhoneNumber, searhterm) || EF.Functions.Like(x.SubscriptionType.Description, searhterm)
                    || EF.Functions.Like(x.SubscriptionType.Type, searhterm)).AsNoTracking()
                .Select(x => ConvertSubscription(x))
                .ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<SubscriptionDTO> GetById(int id)
        {
            var sub = await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .Where(x => x.SubscriptionId == id).AsNoTracking()
                .Select(x => ConvertSubscription(x))
                .FirstOrDefaultAsync();
            if (sub is not null)
            {
                GetLogs(table, id, sub);
                await GetIdentityInfo(sub);
                await GetMobileInfo(sub);
            }
            return sub;
        }
        /// <inheritdoc/>
        public async Task<SubscriptionDTO> Activate(SubscriptionDTO subscription)
        {
            string Value = $"Subscription with Category: {subscription.SubscriptionType.AssetCategory.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            var sub = await TrackedSubscription(subscription.SubscriptionId);
            sub.active = 1;
            sub.LastModifiedAdminId = TokenStore.AdminId;
            sub.DeactivateReason = "";
            sub.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.ActivateLogLine(Value, TokenStore.Admin.Account.UserID, table)
            });
            _context.Subscriptions.Update(sub);
            return subscription;
        }
        /// <inheritdoc/>
        public SubscriptionDTO Create(SubscriptionDTO subscription)
        {
            string Value = $"Subscription with: {subscription.SubscriptionType.AssetCategory.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            Subscription sub = new()
            {
                active = 1,
                AssetCategoryId = subscription.SubscriptionType.AssetCategory.Id,
                PhoneNumber = subscription.PhoneNumber,
                SubsctiptionTypeId = subscription.SubscriptionType.Id,
                LastModifiedAdminId = TokenStore.AdminId,
            };
            if (subscription.SubscriptionType.AssetCategory.Category == "Internet Subscription")
            {
                sub.IdentityId = 1;
            }
            sub.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.CreateLogLine(Value, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow,
            });
            _context.Subscriptions.Add(sub);
            return subscription;
        }
        /// <inheritdoc/>
        public async Task<SubscriptionDTO> Delete(SubscriptionDTO subscription, string reason)
        {
            string Value = $"Subscription with Category: {subscription.SubscriptionType.AssetCategory.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            var sub = await TrackedSubscription(subscription.SubscriptionId);
            sub.active = 0;
            sub.DeactivateReason = reason;
            sub.LastModifiedAdminId = TokenStore.AdminId;
            sub.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.DeleteLogLine(Value, TokenStore.Admin.Account.UserID, reason, table)
            });
            _context.Subscriptions.Update(sub);
            return subscription;
        }
        /// <inheritdoc/>
        public async Task<SubscriptionDTO> Update(SubscriptionDTO subscription)
        {
            var sub = await TrackedSubscription(subscription.SubscriptionId);
            if (string.Compare(sub.PhoneNumber, subscription.PhoneNumber) != 0)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("phone number", sub.PhoneNumber, subscription.PhoneNumber, TokenStore.Admin.Account.UserID, table);
                sub.PhoneNumber = subscription.PhoneNumber;
                sub.LastModifiedAdminId = TokenStore.AdminId;
                sub.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline
                });
            }
            _context.Subscriptions.Update(sub);
            return subscription;
        }
        /// <inheritdoc/>
        public async Task LogPdfFile(string pdfFile, int id)
        {
            var sub = await TrackedSubscription(id);
            sub.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile)
            });
            _context.Subscriptions.Update(sub);
        }
        /// <inheritdoc/>
        public async Task<bool> IsExisting(SubscriptionDTO subscription)
        {
            var sub = await GetById(subscription.SubscriptionId);
            if (sub is null)
            {
                var subs = _context.Subscriptions
                    .Include(x => x.SubscriptionType)
                    .Where(x => x.SubscriptionType.Id == subscription.SubscriptionType.Id && x.PhoneNumber == subscription.PhoneNumber)
                    .ToList();
                if (subs.Count > 0)
                    return true;
            }
            else
            {
                if (string.Compare(sub.PhoneNumber, subscription.PhoneNumber) != 0)
                {
                    var subs = _context.Subscriptions
                    .Include(x => x.SubscriptionType)
                    .Where(x => x.SubscriptionType.Id == subscription.SubscriptionType.Id && x.PhoneNumber == subscription.PhoneNumber)
                    .ToList();
                    if (subs.Count > 0)
                        return true;
                }
                else
                    return false;
            }
            return false;
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<SubscriptionDTO>> ListAllFreeSubscriptions(string category)
        {
            return category switch
            {
                "Mobile" => await _context.Subscriptions
                    .Include(x => x.Category)
                    .Include(x => x.SubscriptionType)
                    .Where(x => x.AssetCategoryId == 3 && x.MobileId == null).AsNoTracking()
                    .Select(x => ConvertSubscription(x))
                    .ToListAsync(),
                "Internet" => await _context.Subscriptions
                    .Include(x => x.Category)
                    .Include(x => x.SubscriptionType)
                    .Where(x => x.AssetCategoryId == 4 && (x.IdentityId == 0 || x.IdentityId == 1)).AsNoTracking()
                    .Select(x => ConvertSubscription(x))
                    .ToListAsync(),
                _ => throw new NotImplementedException($"The {category} is not implemented yet"),
            };
        }
        /// <inheritdoc/>
        public async Task AssignIdentity(AssignInternetSubscriptionRequest subscriptionRequest)
        {
            var subscription = await TrackedSubscription(subscriptionRequest.SubscriptionIds.First());
            var type = _context.SubscriptionTypes.Where(x => x.Id == subscription.SubsctiptionTypeId).AsNoTracking().First();
            subscription.IdentityId = subscriptionRequest.IdentityId;
            subscription.LastModifiedAdminId = TokenStore.AdminId;
            var identity = await _context.Identities.Where(x => x.IdenId == subscriptionRequest.IdentityId).FirstAsync();
            string ideninfo = $"Identity with name: {identity.Name}";
            string subscriptionInfo = $"Subscription: {type} on {subscription.PhoneNumber}";
            identity.LastModifiedAdminId = TokenStore.AdminId;
            subscription.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(subscriptionInfo, ideninfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Subscriptions.Update(subscription);
            identity.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(ideninfo, subscriptionInfo, TokenStore.Admin.Account.UserID, "identity")
            });
            _context.Identities.Update(identity);
        }
        /// <inheritdoc/>
        public async Task ReleaseIdentity(AssignInternetSubscriptionRequest subscriptionRequest)
        {
            var subscription = await TrackedSubscription(subscriptionRequest.SubscriptionIds.First());
            var type = _context.SubscriptionTypes.Where(x => x.Id == subscription.SubsctiptionTypeId).AsNoTracking().First();
            subscription.IdentityId = 1;
            subscription.LastModifiedAdminId = TokenStore.AdminId;
            var identity = await _context.Identities.Where(x => x.IdenId == subscriptionRequest.IdentityId).FirstAsync();
            string ideninfo = $"Identity with name: {identity.Name}";
            string subscriptionInfo = $"Subscription: {type} on {subscription.PhoneNumber}";
            identity.LastModifiedAdminId = TokenStore.AdminId;
            subscription.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(subscriptionInfo, ideninfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Subscriptions.Update(subscription);
            identity.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, subscriptionInfo, TokenStore.Admin.Account.UserID, "identity")
            });
            _context.Identities.Update(identity);
        }
        /// <inheritdoc/>
        public async Task AssignMobile(AssignMobileSubscriptionRequest assignMobileRequest)
        {
            var subscription = await TrackedSubscription(assignMobileRequest.SubscriptionId);
            var type = _context.SubscriptionTypes.Where(x => x.Id == subscription.SubsctiptionTypeId).AsNoTracking().First();
            subscription.LastModifiedAdminId = TokenStore.AdminId;
            subscription.MobileId = assignMobileRequest.MobileId;
            string subscriptionInfo = $"Subscription: {type} on {subscription.PhoneNumber}";
            var mobile = await _context.Mobiles.Where(x => x.MobileId == subscription.MobileId).FirstAsync();
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            var assetType = await _context.AssetTypes.AsNoTracking().Where(x => x.TypeID == mobile.TypeId).FirstAsync();
            string mobileInfo = $"mobile with type {assetType}";
            subscription.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine( subscriptionInfo, mobileInfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Subscriptions.Update(subscription);
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine( mobileInfo, subscriptionInfo, TokenStore.Admin.Account.UserID, "mobile")
            });
            _context.Mobiles.Update(mobile);
        }
        /// <inheritdoc/>
        public async Task ReleaseMobile(AssignMobileSubscriptionRequest assignMobileRequest)
        {
            var subscription = await TrackedSubscription(assignMobileRequest.SubscriptionId);
            var type = _context.SubscriptionTypes.Where(x => x.Id == subscription.SubsctiptionTypeId).AsNoTracking().First();
            string subscriptionInfo = $"Subscription: {type} on {subscription.PhoneNumber}";
            subscription.LastModifiedAdminId = TokenStore.AdminId;
            subscription.MobileId = null;
            var mobile = await _context.Mobiles.Where(x => x.MobileId == assignMobileRequest.MobileId).FirstAsync();
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            var assetType = await _context.AssetTypes.AsNoTracking().Where(x => x.TypeID == mobile.TypeId).FirstAsync();
            string mobileInfo = $"mobile with type {assetType}";
            subscription.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.ReleaseIdentityFromDeviceLogLine(subscriptionInfo, mobileInfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Subscriptions.Update(subscription);
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.ReleaseIdentityFromDeviceLogLine(mobileInfo, subscriptionInfo, TokenStore.Admin.Account.UserID, "mobile")
            });
            _context.Mobiles.Update(mobile);
        }
        /// <summary>
        /// This function will convert the Subscription to a DTO
        /// </summary>
        /// <param name="subscription"><see cref="Subscription"/></param>
        /// <returns><see cref="SubscriptionDTO"/></returns>
        public static SubscriptionDTO ConvertSubscription(Subscription subscription)
        {
            return new()
            {
                LastModifiedAdminId = subscription.LastModifiedAdminId,
                DeactivateReason = subscription.DeactivateReason,
                PhoneNumber = subscription.PhoneNumber,
                SubscriptionId = subscription.SubscriptionId,
                Active = subscription.active,
                IdentityId = subscription.IdentityId,
                MobileId = subscription.MobileId,
                SubscriptionType = new()
                {
                    Id = subscription.SubscriptionType.Id,
                    LastModifiedAdminId = subscription.SubscriptionType.LastModifiedAdminId,
                    DeactivateReason = subscription.SubscriptionType.DeactivateReason,
                    Active = subscription.SubscriptionType.active,
                    Type = subscription.SubscriptionType.Type,
                    Description = subscription.SubscriptionType.Description,
                    Provider = subscription.SubscriptionType.Provider,
                    AssetCategory = new()
                    {
                        Active = subscription.Category.Id,
                        Category = subscription.Category.Category,
                        DeactivateReason = subscription.Category.DeactivateReason,
                        Id = subscription.Category.Id,
                        LastModifiedAdminId = subscription.Category.LastModifiedAdminId,
                        Prefix = subscription.Category.Prefix
                    }
                }
            };
        }

        private async Task<Subscription> TrackedSubscription(int Id)
        {
            return await _context.Subscriptions.Where(x => x.SubscriptionId == Id).FirstAsync();
        }
        private async Task GetIdentityInfo(SubscriptionDTO sub)
        {
            sub.Identity = await _context.Identities
                .Include(x => x.Language)
                .Include(x => x.Type)
                .Where(x => x.IdenId == sub.IdentityId)
                .Select(x => IdentityRepository.ConvertIdentity(x))
                .FirstOrDefaultAsync();
        }
        private async Task GetMobileInfo(SubscriptionDTO sub)
        {
            sub.Mobile = await _context.Mobiles
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category)
                .Include( x => x.Identity)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.MobileId == sub.MobileId)
                .Select(x => MobileRepository.ConvertMobile(x))
                .FirstOrDefaultAsync();
        }
    }
}
