using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.Graph;
using Subscription = CMDB.Domain.Entities.Subscription;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Identity = CMDB.Domain.Entities.Identity;

namespace CMDB.Services
{
    public class SubscriptionService : LogService
    {
        public SubscriptionService(CMDBContext context) : base(context)
        {
        }
        public async Task<ICollection<Subscription>> ListAll()
        {
            var subscriptions = await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .ToListAsync();
            return subscriptions;
        }
        public async Task<ICollection<Subscription>> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            var subscriptions = await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .Where(x => EF.Functions.Like(x.PhoneNumber, searhterm) || EF.Functions.Like(x.SubscriptionType.Description, searhterm)
                    || EF.Functions.Like(x.SubscriptionType.Type, searhterm))
                .ToListAsync();
            return subscriptions;
        }
        public async Task<ICollection<Subscription>> GetByID(int id)
        {
            var subscriptions = await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .Where(x => x.SubscriptionId == id)
                .ToListAsync();
            return subscriptions;
        }
        public async Task Create(SubscriptionType type, string phoneNumber, string table)
        {
            Subscription subscription = new()
            {
                LastModfiedAdmin = Admin,
                Category = type.Category,
                SubscriptionType = type,
                PhoneNumber = phoneNumber
            };
            if (type.Category.Category == "Internet Subscription")
                subscription.IdentityId = 1;
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            string Value = $"Subscription with Category: {type.Category.Category} and type {type} on {phoneNumber}";
            await LogCreate(table, subscription.SubscriptionId, Value);
        }
        public async Task Edit(Subscription subscription, string phoneNumber, string table)
        {
            string oldPhone = subscription.PhoneNumber;
            if(String.Compare(oldPhone,phoneNumber) != 0)
            {
                subscription.PhoneNumber = phoneNumber;
                subscription.LastModfiedAdmin = Admin;
                await _context.SaveChangesAsync();
                await LogUpdate(table, subscription.SubscriptionId, "phone number", oldPhone, phoneNumber);
            }
        }
        public async Task Activate(Subscription subscription, string table)
        {
            string Value = $"Subscription with Category: {subscription.SubscriptionType.Category.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            subscription.LastModfiedAdmin = Admin;
            subscription.Active = State.Active;
            subscription.DeactivateReason = "";
            await _context.SaveChangesAsync();
            await LogActivate(table, subscription.SubscriptionId, Value);
        }
        public async Task Deactivate(Subscription subscription, string reason, string table)
        {
            string Value = $"Subscription with Category: {subscription.SubscriptionType.Category.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            subscription.LastModfiedAdmin = Admin;
            subscription.Active = State.Inactive;
            subscription.DeactivateReason = reason;
            await _context.SaveChangesAsync();
            await LogDeactivate(table, subscription.SubscriptionId, Value, reason);
        }
        public async Task<SubscriptionType> GetSubscriptionTypeById(int id)
        {
            SubscriptionType type = await _context.SubscriptionTypes
                .Include(x=> x.Category)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return type;
        }
        public List<SelectListItem> GetSubscriptionTypes()
        {
            List<SelectListItem> types = new();
            var subscriptions = _context.SubscriptionTypes
                .Include(x => x.Category)
                .Where(x => x.active == 1)
                .ToList();
            foreach (var subscription in subscriptions)
            {
                types.Add(new SelectListItem($"{subscription}",$"{subscription.Id}"));
            }
            return types;
        }
        public async Task<bool> IsSubscritionExisting(SubscriptionType subscriptionType, string phoneNumber, int id =0)
        {
            if(id > 0)
            {
                var subs = await GetByID(id);
                Subscription sub = subs.First();
                if(String.Compare(sub.PhoneNumber,phoneNumber) != 0)
                {
                    var subscriptions = _context.Subscriptions
                                        .Include(x => x.SubscriptionType)
                                        .Where(x => x.SubscriptionType.Id == subscriptionType.Id && x.PhoneNumber == phoneNumber)
                                        .ToList();
                    if (subscriptions.Count > 0)
                        return true;
                }
                else
                    return false;
            }
            else
            {
                var subs = _context.Subscriptions
                    .Include(x => x.SubscriptionType)
                    .Where(x => x.SubscriptionType.Id == subscriptionType.Id && x.PhoneNumber == phoneNumber)
                    .ToList();
                if(subs.Count > 0)
                    return true;
            }
            return false;
        }
        public async Task ReleaseIdenity(Subscription subscription, Identity identity,string table)
        {
            identity.LastModfiedAdmin = Admin;
            subscription.LastModfiedAdmin = Admin;
            subscription.IdentityId = 1;
            identity.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            await LogReleaseIdentityFromSubscription("identity", identity,subscription);
            await LogReleaseSubscriptionFromIdentity(table, subscription, identity);
        }
        public async Task AssignIdentity(Subscription subscription, Identity identity, string table)
        {
            identity.LastModfiedAdmin = Admin;
            subscription.LastModfiedAdmin = Admin;
            identity.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            await LogAssignIdentity2Subscription("identity", identity, subscription);
            await LogAssignSubsciption2Identity(table, subscription, identity);
        }
    }
}
