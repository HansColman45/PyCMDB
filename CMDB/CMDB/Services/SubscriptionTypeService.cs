using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMDB.Services
{
    public class SubscriptionTypeService : LogService
    {
        public SubscriptionTypeService() : base()
        {
        }
        public async Task<ICollection<SubscriptionType>> ListAll()
        {
            /*var types = await _context.SubscriptionTypes
                .Include(x => x.Category).ToListAsync();
            return types;*/
            return [];
        }
        public async Task<ICollection<SubscriptionType>> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            /*var types = await _context.SubscriptionTypes
                .Include(x => x.Category)
                .Where(x => EF.Functions.Like(x.Category.Category,searhterm) || EF.Functions.Like(x.Description,searhterm) || EF.Functions.Like(x.Type,searhterm) || EF.Functions.Like(x.Provider,searhterm))
                .ToListAsync();
            return types;*/
            return [];
        }
        public async Task<ICollection<SubscriptionType>> GetById(int TypeId)
        {
            /*var types = await _context.SubscriptionTypes
               .Include(x => x.Category)
               .Where(x => x.Id == TypeId)
               .ToListAsync();
            return types;*/
            return [];
        }
        public List<SelectListItem> GetCategories()
        {
            List<SelectListItem> types = new();
            /*var categories = _context.AssetCategories
                .Where(x => EF.Functions.Like(x.Category, "%Subscription"))
                .ToList();
            foreach (var category in categories)
            {
                types.Add(new SelectListItem($"{category.Category}",$"{category.Id}"));
            }*/
            return types;
        }
        public AssetCategory GetAssetCategory(int Id)
        {
            /*var cat = _context.AssetCategories.Where(x => x.Id == Id).First();
            return cat;*/
            return new();
        }
        public async Task Create(SubscriptionType subscriptionType, string table)
        {
            /*_context.SubscriptionTypes.Add(subscriptionType);
            await _context.SaveChangesAsync();
            await LogCreate(table, subscriptionType.Id, $"{subscriptionType.Category.Category} with {subscriptionType.Provider} and {subscriptionType.Type}");*/
        }
        public async Task Edit(SubscriptionType subscriptionType, string provider, string Type, string description, string table)
        {
            bool changed = false;
            string oldProvider = subscriptionType.Provider;
            string oldType = subscriptionType.Type;
            string oldDescription = subscriptionType.Description;
            if(String.Compare(oldProvider,provider) != 0)
            {
                changed = true;
                subscriptionType.Provider = provider;
            }
            if(String.Compare(oldDescription,description) != 0)
            {
                changed = true;
                subscriptionType.Description = description;
            }
            if(String.Compare(oldType,Type) != 0)
            {
                changed = true;
                subscriptionType.Type = Type;
            }
            if (changed)
            {
                /*subscriptionType.LastModfiedAdmin = Admin;
                //await _context.SaveChangesAsync();*/
            }
        }
        public async Task Delete(SubscriptionType subscriptionType, string reason, string table)
        {
            /*subscriptionType.LastModfiedAdmin = Admin;
            subscriptionType.Active = State.Inactive;
            subscriptionType.DeactivateReason = reason;
            /*await _context.SaveChangesAsync();
            await LogDeactivate(table, subscriptionType.Id, $"{subscriptionType.Category.Category} with {subscriptionType.Provider} and {subscriptionType.Type}", reason);*/
        }
        public async Task Activate(SubscriptionType subscriptionType, string table)
        {
            /*subscriptionType.LastModfiedAdmin = Admin;
            subscriptionType.Active = State.Active;
            subscriptionType.DeactivateReason = "";
            /*await _context.SaveChangesAsync();
            await LogActivate(table, subscriptionType.Id, $"{subscriptionType.Category.Category} with {subscriptionType.Provider} and {subscriptionType.Type}");*/
        }
    }
}
