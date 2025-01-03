using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class SubscriptionRepository : GenericRepository, ISubscriptionRepository
    {
        private readonly string table = "subscription";
        public SubscriptionRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<IEnumerable<SubscriptionDTO>> GetAll()
        {
           return await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .Select(x => ConvertSubscription(x))
                .ToListAsync();
        }
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
        public async Task<SubscriptionDTO?> GetById(int id)
        {
            var sub = await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .Where(x => x.SubscriptionId == id).AsNoTracking()
                .Select(x => ConvertSubscription(x))
                .FirstOrDefaultAsync();
            if (sub is not null)
            {
                GetLogs(table,id,sub);
            }
            return sub;
        }
        public async Task<SubscriptionDTO> Activate(SubscriptionDTO subscription)
        {
            string Value = $"Subscription with Category: {subscription.SubscriptionType.AssetCategory.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            var sub = await TrackedSubscription(subscription.SubscriptionId);
            sub.active = 1;
            sub.LastModifiedAdminId = TokenStore.AdminId;
            sub.DeactivateReason = "";
            sub.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ActivateLogLine(Value, TokenStore.Admin.Account.UserID, table)
            });
            _context.Subscriptions.Update(sub);
            return subscription;
        }
        public SubscriptionDTO Create(SubscriptionDTO subscription)
        {
            string Value = $"Subscription with Category: {subscription.SubscriptionType.AssetCategory.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            Subscription sub = new()
            {
                active = 1,
                AssetCategoryId = subscription.SubscriptionType.AssetCategory.Id,
                PhoneNumber = subscription.PhoneNumber,
                SubsctiptionTypeId = subscription.SubscriptionType.Id,
                LastModifiedAdminId = TokenStore.AdminId,
            };
            sub.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.CreateLogLine(Value,TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now,
            });
            _context.Subscriptions.Add(sub);
            return subscription;
        }
        public async Task<SubscriptionDTO> Delete(SubscriptionDTO subscription, string reason)
        {
            string Value = $"Subscription with Category: {subscription.SubscriptionType.AssetCategory.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            var sub = await TrackedSubscription(subscription.SubscriptionId);
            sub.active = 0;
            sub.DeactivateReason = reason;
            sub.LastModifiedAdminId = TokenStore.AdminId;
            sub.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.DeleteLogLine(Value, TokenStore.Admin.Account.UserID, reason, table)
            });
            _context.Subscriptions.Update(sub);
            return subscription;
        }
        public async Task<SubscriptionDTO> Update(SubscriptionDTO subscription)
        {
            var sub = await TrackedSubscription(subscription.SubscriptionId);
            if(string.Compare(sub.PhoneNumber, subscription.PhoneNumber) != 0)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("phone number", sub.PhoneNumber,subscription.PhoneNumber,TokenStore.Admin.Account.UserID,table);
                sub.PhoneNumber = subscription.PhoneNumber;
                sub.LastModifiedAdminId = TokenStore.AdminId;
                sub.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logline
                });
            }
            _context.Subscriptions.Update(sub);
            return subscription;
        }
        public async Task LogPdfFile(string pdfFile, int id)
        {
            var sub = await TrackedSubscription(id);
            sub.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile)
            });
            _context.Subscriptions.Update(sub);
        }
        public async Task<bool> IsExisting(SubscriptionDTO subscription)
        {
            var sub = await GetById(subscription.SubscriptionId);
            if(sub is null)
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


        public static SubscriptionDTO ConvertSubscription(Subscription subscription)
        {
            return new()
            {
                LastModifiedAdminId = subscription.LastModifiedAdminId,
                DeactivateReason = subscription.DeactivateReason,
                PhoneNumber = subscription.PhoneNumber,
                SubscriptionId = subscription.SubscriptionId,
                Active = subscription.active,
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
    }
}
