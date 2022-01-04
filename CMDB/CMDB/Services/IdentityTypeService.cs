using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CMDB.Services
{
    public class IdentityTypeService : LogService
    {
        public IdentityTypeService(CMDBContext context) : base(context)
        {
        }
        public ICollection<IdentityType> ListAll()
        {
            var types = _context.IdentityTypes.ToList();
            return types;
        }
        public ICollection<IdentityType> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            var types = _context.IdentityTypes
                .Where(x => EF.Functions.Like(x.Description, searhterm) || EF.Functions.Like(x.Type, searchString))
                .ToList();
            return types;
        }
        public ICollection<IdentityType> GetByID(int id)
        {
            var types = _context.IdentityTypes
                .Where(x => x.TypeID == id)
                .ToList();
            return types;
        }
        public void Create(IdentityType identityType, string Table)
        {
            identityType.LastModfiedAdmin = Admin;
            _context.IdentityTypes.Add(identityType);
            _context.SaveChanges();
            string Value = "Identitytype created with type: " + identityType.Type + " and description: " + identityType.Description;
            LogCreate(Table, identityType.TypeID, Value);
        }
        public void Update(IdentityType identityType, string Type, string Description, string Table)
        {
            identityType.LastModfiedAdmin = Admin;
            if (String.Compare(identityType.Type, Type) != 0)
            {
                identityType.Type = Type;
                _context.SaveChanges();
                LogUpdate(Table, identityType.TypeID, "Type", identityType.Type, Type);
            }
            if (String.Compare(identityType.Description, Description) != 0)
            {
                identityType.Description = Description;
                _context.SaveChanges();
                LogUpdate(Table, identityType.TypeID, "Description", identityType.Description, Description);
            }
        }
        public void Deactivate(IdentityType identityType, string reason, string Table)
        {
            identityType.LastModfiedAdmin = Admin;
            identityType.DeactivateReason = reason;
            identityType.Active = "Inactive";
            _context.SaveChanges();
            string Value = "Account type created with type: " + identityType.Type + " and description: " + identityType.Description;
            LogDeactivate(Table, identityType.TypeID, Value, reason);
        }
        public void Activate(IdentityType identityType, string table)
        {
            identityType.LastModfiedAdmin = Admin;
            identityType.DeactivateReason = "";
            identityType.Active = "Active";
            _context.SaveChanges();
            string Value = "Account type created with type: " + identityType.Type + " and description: " + identityType.Description;
            LogActivate(table, identityType.TypeID, Value);
        }
        public bool IsExisting(IdentityType identityType, string Type = "", string Description = "")
        {
            bool result = false;
            if (String.IsNullOrEmpty(Type) && String.Compare(identityType.Type, Type) != 0)
            {
                var identypes = _context.IdentityTypes
                    .Where(x => x.Type == Type)
                    .ToList();
                if (identypes.Count > 0)
                    result = true;
            }
            if (String.IsNullOrEmpty(Description) && String.Compare(identityType.Description, Description) != 0)
            {
                var identypes = _context.IdentityTypes
                    .Where(x => x.Description == Description)
                    .ToList();
                if (identypes.Count > 0)
                    result = true;
            }
            return result;
        }
    }
}
