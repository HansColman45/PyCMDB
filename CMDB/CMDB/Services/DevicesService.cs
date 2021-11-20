using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMDB.Services
{
    public class DevicesService : LogService
    {
        public DevicesService(CMDBContext context) : base(context)
        {
        }
        public List<Device> ListAll(string category)
        {
            List<Device> devices = new();
            switch (category)
            {
                case "Laptop":
                    var laptops = _context.Laptops
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .ToList();
                    foreach (var laptop in laptops)
                    {
                        devices.Add(laptop);
                    }
                    break;
                case "Desktops":
                    var dekstops = _context.Desktops
                       .Include(x => x.Category)
                       .Include(x => x.Identity)
                       .ToList();
                    foreach (var desktop in dekstops)
                    {
                        devices.Add(desktop);
                    }
                    break;
                default:
                    break;
            }
            return devices;
        }
        public List<Device> ListAll(string category, string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<Device> devices = new();
            switch (category)
            {
                case "Laptop":
                    var laptops = _context.Laptops
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm))
                        .ToList();
                    foreach (var laptop in laptops)
                    {
                        devices.Add(laptop);
                    }
                    break;
                case "Desktops":
                    var dekstops = _context.Desktops
                       .Include(x => x.Category)
                       .Include(x => x.Identity)
                       .Where(x => EF.Functions.Like(x.SerialNumber, searhterm))
                       .ToList();
                    foreach (var desktop in dekstops)
                    {
                        devices.Add(desktop);
                    }
                    break;
                default:
                    break;
            }
            return devices;
        }
        public List<SelectListItem> ListRams()
        {
            List<SelectListItem> assettypes = new();
            var rams = _context.RAMs.ToList();
            foreach (var ram in rams)
            {
                assettypes.Add(new(ram.Display, ram.Value.ToString()));
            }
            return assettypes;
        }
        public void CreateNewDesktop(Desktop desktop, string table)
        {
            _context.Desktops.Add(desktop);
            _context.SaveChanges();
            string Value = String.Format("{0} with type {1}", desktop.Category.Category, desktop.Type.Vendor + " " + desktop.Type.Type);
            LogCreate(table, desktop.AssetTag, Value, Admin.Account.UserID);
        }
        public void CreateNewLaptop(Laptop laptop, string table)
        {
            _context.Laptops.Add(laptop);
            _context.SaveChanges();
            string Value = String.Format("{0} with type {1}", laptop.Category.Category, laptop.Type.Vendor + " " + laptop.Type.Type);
            LogCreate(table, laptop.AssetTag, Value, Admin.Account.UserID);
        }
        public void UpdateDesktop(Desktop desktop, string newRam, string newMAC, AssetType newAssetType, string newSerialNumber, string Table)
        {
            if (String.Compare(desktop.RAM, newRam) != 0)
            {
                desktop.RAM = newRam;
                _context.SaveChanges();
                LogUpdate(Table, desktop.AssetTag, "RAM", desktop.RAM, newRam, Admin.Account.UserID);
            }
            if (String.Compare(desktop.MAC, newMAC) != 0)
            {
                desktop.MAC = newMAC;
                _context.SaveChanges();
                LogUpdate(Table, desktop.AssetTag, "MAC", desktop.MAC, newMAC, Admin.Account.UserID);
            }
            if (String.Compare(desktop.SerialNumber, newSerialNumber) != 0)
            {
                desktop.SerialNumber = newSerialNumber;
                _context.SaveChanges();
                LogUpdate(Table, desktop.AssetTag, "SerialNumber", desktop.SerialNumber, newSerialNumber, Admin.Account.UserID);
            }
            if (desktop.Type.TypeID != newAssetType.TypeID)
            {
                desktop.Type = newAssetType;
                _context.SaveChanges();
                LogUpdate(Table, desktop.AssetTag, "Type", desktop.Type.Vendor + " " + desktop.Type, newAssetType.Vendor + " " + newAssetType.Type, Admin.Account.UserID);
            }
        }
        public void UpdateLaptop(Laptop laptop, string newRam, string newMAC, AssetType newAssetType, string newSerialNumber, string Table)
        {
            if (String.Compare(laptop.RAM, newRam) != 0)
            {
                laptop.RAM = newRam;
                _context.SaveChanges();
                LogUpdate(Table, laptop.AssetTag, "RAM", laptop.RAM, newRam, Admin.Account.UserID);
            }
            if (String.Compare(laptop.MAC, newMAC) != 0)
            {
                laptop.MAC = newMAC;
                _context.SaveChanges();
                LogUpdate(Table, laptop.AssetTag, "MAC", laptop.MAC, newMAC, Admin.Account.UserID);
            }
            if (String.Compare(laptop.SerialNumber, newSerialNumber) != 0)
            {
                laptop.SerialNumber = newSerialNumber;
                _context.SaveChanges();
                LogUpdate(Table, laptop.AssetTag, "SerialNumber", laptop.SerialNumber, newSerialNumber, Admin.Account.UserID);
            }
            if (laptop.Type.TypeID != newAssetType.TypeID)
            {
                laptop.Type = newAssetType;
                _context.SaveChanges();
                LogUpdate(Table, laptop.AssetTag, "Type", laptop.Type.Vendor + " " + laptop.Type, newAssetType.Vendor + " " + newAssetType.Type, Admin.Account.UserID);
            }
        }
        public void Deactivate(Device device, string Reason, string table)
        {
            device.DeactivateReason = Reason;
            device.Active = "Inactive";
            _context.SaveChanges();
            string Value = String.Format("{0} with type {1}", device.Category.Category, device.Type.Vendor + " " + device.Type.Type);
            LogDeactivated(table, device.AssetTag, Value, Reason);
        }
        public void Activate(Device device, string table)
        {
            device.DeactivateReason = "";
            device.Active = "Active";
            _context.SaveChanges();
            string Value = String.Format("{0} with type {1}", device.Category.Category, device.Type.Vendor + " " + device.Type.Type);
            LogActivate(table, device.AssetTag, Value);
        }
        public List<Desktop> ListDekstopByID(string assetTag)
        {
            var desktops = _context.Desktops
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToList();
            return desktops;
        }
        public List<Laptop> ListLaptopByID(string assetTag)
        {
            var laptops = _context.Laptops
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToList();
            return laptops;
        }
        public List<Docking> ListDockingByID(string assetTag)
        {
            var dockings = _context.Dockings
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToList();
            return dockings;
        }
        public List<Screen> ListScreensByID(string assetTag)
        {
            var screens = _context.Screens
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToList();
            return screens;
        }
        public List<Token> ListTokenByID(string assetTag)
        {
            var tokens = _context.Tokens
                 .Include(x => x.Category)
                 .Include(x => x.Identity)
                 .Where(x => x.AssetTag == assetTag)
                 .ToList();
            return tokens;
        }
        public bool IsLaptopExisting(Laptop device)
        {
            bool result = false;
            var devices = _context.Laptops.Where(x => x.AssetTag == device.AssetTag).ToList();
            if (devices.Count > 0)
                result = true;
            return result;
        }
        public bool IsDesktopExisting(Desktop device)
        {
            bool result = false;
            var devices = _context.Desktops.Where(x => x.AssetTag == device.AssetTag).ToList();
            if (devices.Count > 0)
                result = true;
            return result;
        }
        public List<SelectListItem> ListAssetTypes(string category)
        {
            List<SelectListItem> assettypes = new();
            var types = _context.AssetTypes.Include(x => x.Category).Where(x => x.Category.Category == category).ToList();
            foreach (var type in types)
            {
                assettypes.Add(new(type.Vendor + " " + type.Type, type.TypeID.ToString()));
            }
            return assettypes;
        }
        public List<AssetType> ListAssetTypeById(int id)
        {
            var devices = _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == id)
                .ToList();
            return devices;
        }
        public ICollection<Identity> GetIdentityByID(int id)
        {
            List<Identity> identities = _context.Identities
                .Include(x => x.Type)
                .Where(x => x.IdenId == id)
                .ToList();
            return identities;
        }
    }
}
