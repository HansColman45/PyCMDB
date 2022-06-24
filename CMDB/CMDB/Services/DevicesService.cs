using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class DevicesService : LogService
    {
        public DevicesService(CMDBContext context) : base(context)
        {
        }
        public async Task<List<Device>> ListAll(string category)
        {
            List<Device> devices = new();
            switch (category)
            {
                case "Laptop":
                    var laptops = await _context.Devices
                        .OfType<Laptop>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .ToListAsync();
                    foreach (var laptop in laptops)
                    {
                        devices.Add(laptop);
                    }
                    break;
                case "Desktop":
                    var dekstops = await _context.Devices
                        .OfType<Desktop>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .ToListAsync();
                    foreach (var desktop in dekstops)
                    {
                        devices.Add(desktop);
                    }
                    break;
                case "Docking station":
                    var dockings = await _context.Devices
                        .OfType<Docking>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .ToListAsync();
                    foreach (var docking in dockings)
                    {
                        devices.Add(docking);
                    }
                    break;
                case "Token":
                    var tokens = await _context.Devices
                        .OfType<Token>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .ToListAsync();
                    foreach (var token in tokens)
                    {
                        devices.Add(token);
                    }
                    break;
                case "Monitor":
                    var screens = await _context.Devices
                        .OfType<Screen>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .ToListAsync();
                    foreach (var screen in screens)
                    {
                        devices.Add(screen);
                    }
                    break;
                default:
                    break;
            }
            return devices;
        }
        public async Task<List<Device>> ListAll(string category, string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<Device> devices = new();
            switch (category)
            {
                case "Laptop":
                    var laptops = await _context.Devices
                        .OfType<Laptop>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .ToListAsync();
                    foreach (var laptop in laptops)
                    {
                        devices.Add(laptop);
                    }
                    break;
                case "Desktop":
                    var dekstops = await _context.Devices
                        .OfType<Desktop>()
                       .Include(x => x.Category)
                       .Include(x => x.Identity)
                       .Include(x => x.Type)
                       .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                       .ToListAsync();
                    foreach (var desktop in dekstops)
                    {
                        devices.Add(desktop);
                    }
                    break;
                case "Docking station":
                    var dockings = await _context.Devices
                        .OfType<Docking>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .ToListAsync();
                    foreach (var docking in dockings)
                    {
                        devices.Add(docking);
                    }
                    break;
                case "Token":
                    var tokens = await _context.Devices
                        .OfType<Token>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .ToListAsync();
                    foreach (var token in tokens)
                    {
                        devices.Add(token);
                    }
                    break;
                case "Monitor":
                    var screens = await _context.Devices
                        .OfType<Screen>()
                        .Include(x => x.Category)
                        .Include(x => x.Identity)
                        .Include(x => x.Type)
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .ToListAsync();
                    foreach (var screen in screens)
                    {
                        devices.Add(screen);
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
        public async Task CreateNewDevice(Device device, string table)
        {
            device.LastModfiedAdmin = Admin;
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            string value = $"{device.Category.Category} with type {device.Type}";
            await LogCreate(table,device.AssetTag,value);
        }
        public async Task UpdateDesktop(Desktop desktop, string newRam, string newMAC, AssetType newAssetType, string newSerialNumber, string Table)
        {
            desktop.LastModfiedAdmin = Admin;
            string oldRam, oldMac, oldSerial;
            oldMac = desktop.MAC;
            oldRam = desktop.RAM;
            oldSerial = desktop.SerialNumber;
            AssetType oldType = desktop.Type;
            if (String.Compare(desktop.RAM, newRam) != 0)
            {
                desktop.RAM = newRam;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, desktop.AssetTag, "RAM", oldRam, newRam);
            }
            if (String.Compare(desktop.MAC, newMAC) != 0)
            {
                desktop.MAC = newMAC;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, desktop.AssetTag, "MAC", oldMac, newMAC);
            }
            if (String.Compare(desktop.SerialNumber, newSerialNumber) != 0)
            {
                desktop.SerialNumber = newSerialNumber;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, desktop.AssetTag, "SerialNumber", oldSerial, newSerialNumber);
            }
            if (desktop.Type.TypeID != newAssetType.TypeID)
            {
                desktop.Type = newAssetType;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, desktop.AssetTag, "Type", oldType.ToString(), newAssetType.ToString());
            }
        }
        public async Task UpdateLaptop(Laptop laptop, string newRam, string newMAC, AssetType newAssetType, string newSerialNumber, string Table)
        {
            laptop.LastModfiedAdmin = Admin;
            string oldRam, oldMac, oldSerial;
            oldMac = laptop.MAC;
            oldRam = laptop.RAM;
            oldSerial = laptop.SerialNumber;
            AssetType oldType = laptop.Type;
            if (String.Compare(laptop.RAM, newRam) != 0)
            {
                laptop.RAM = newRam;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, laptop.AssetTag, "RAM", oldRam, newRam);
            }
            if (String.Compare(laptop.MAC, newMAC) != 0)
            {
                laptop.MAC = newMAC;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, laptop.AssetTag, "MAC", oldMac, newMAC);
            }
            if (String.Compare(laptop.SerialNumber, newSerialNumber) != 0)
            {
                laptop.SerialNumber = newSerialNumber;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, laptop.AssetTag, "SerialNumber", oldSerial, newSerialNumber);
            }
            if (laptop.Type.TypeID != newAssetType.TypeID)
            {
                laptop.Type = newAssetType;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, laptop.AssetTag, "Type", oldType.ToString(), newAssetType.ToString());
            }
        }
        public async Task UpdateScreen(Screen screen, string newSerialNumber, AssetType newAssetType, string Table)
        {
            screen.LastModfiedAdmin = Admin;
            string oldSerial = screen.SerialNumber;
            AssetType oldType = screen.Type;
            if (String.Compare(screen.SerialNumber, newSerialNumber) != 0)
            {
                screen.SerialNumber = newSerialNumber;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, screen.AssetTag, "SerialNumber", oldSerial, newSerialNumber);
            }
            if (screen.Type.TypeID != newAssetType.TypeID)
            {
                screen.Type = newAssetType;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, screen.AssetTag, "Type", oldType.ToString(), newAssetType.ToString());
            }
        }

        public async Task UpdateDocking(Docking docking, string newSerialNumber, AssetType newAssetType, string Table)
        {
            docking.LastModfiedAdmin = Admin;
            string oldSerial = docking.SerialNumber;
            AssetType oldType = docking.Type;
            if (String.Compare(docking.SerialNumber, newSerialNumber) != 0)
            {
                docking.SerialNumber = newSerialNumber;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, docking.AssetTag, "SerialNumber", oldSerial, newSerialNumber);
            }
            if (docking.Type.TypeID != newAssetType.TypeID)
            {
                docking.Type = newAssetType;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, docking.AssetTag, "Type", oldType.ToString(), newAssetType.ToString());
            }
        }
        public async Task UpdateToken(Token token, string newSerialNumber, AssetType newAssetType,string table)
        {
            token.LastModfiedAdmin = Admin;
            string oldSerial = token.SerialNumber;
            AssetType oldType = token.Type;
            if (String.Compare(token.SerialNumber, newSerialNumber) != 0)
            {
                token.SerialNumber = newSerialNumber;
                await _context.SaveChangesAsync();
                await LogUpdate(table, token.AssetTag, "SerialNumber", oldSerial, newSerialNumber);
            }
            if (token.Type.TypeID != newAssetType.TypeID)
            {
                token.Type = newAssetType;
                await _context.SaveChangesAsync();
                await LogUpdate(table, token.AssetTag, "Type", oldType.ToString(), newAssetType.ToString());
            }
        }
        public async Task Deactivate(Device device, string Reason, string table)
        {
            device.LastModfiedAdmin = Admin;
            device.DeactivateReason = Reason;
            device.Active = State.Inactive;
            await _context.SaveChangesAsync();
            string Value = $"{device.Category.Category} with type {device.Type}";
            await LogDeactivated(table, device.AssetTag, Value, Reason);
        }
        public async Task Activate(Device device, string table)
        {
            device.LastModfiedAdmin = Admin;
            device.DeactivateReason = "";
            device.Active = State.Active;
            await _context.SaveChangesAsync();
            string Value = $"{device.Category.Category} with type {device.Type}";
            await LogActivate(table, device.AssetTag, Value);
        }
        public async Task<List<Desktop>> ListDekstopByID(string assetTag)
        {
            var desktops = await _context.Devices
                .OfType<Desktop>()
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToListAsync();
            return desktops;
        }
        public async Task<List<Laptop>> ListLaptopByID(string assetTag)
        {
            var laptops = await _context.Devices
                .OfType<Laptop>()
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToListAsync();
            return laptops;
        }
        public async Task<List<Docking>> ListDockingByID(string assetTag)
        {
            var dockings = await _context.Devices
                .OfType<Docking>()
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToListAsync();
            return dockings;
        }
        public async Task<List<Screen>> ListScreensByID(string assetTag)
        {
            var screens = await _context.Devices
                .OfType<Screen>()
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToListAsync();
            return screens;
        }
        public async Task<List<Token>> ListTokenByID(string assetTag)
        {
            var tokens = await _context.Devices
                .OfType<Token>()
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.AssetTag == assetTag)
                .ToListAsync();
            return tokens;
        }
        public bool IsDeviceExisting(Device device) 
        {
            bool result = false;
            var devices = _context.Devices.Where(x => x.AssetTag == device.AssetTag).ToList();
            if (devices.Count > 0)
                result = true;
            return result;
        }
        public bool IsDeviceFree(Device device)
        {
            bool result = false;
            var devices = _context.Devices.Where(x => x.AssetTag == device.AssetTag).First();
            if (devices.Identity is null)
                result = true;
            return result;
        }
        public List<SelectListItem> ListFreeIdentities()
        {
            List<SelectListItem> identites = new();
            var idens = _context.Identities
                .Include(x => x.Devices)
                .Where(x => !x.Devices.Any())
                .Where(x => x.IdenId != 1)
                .ToList();
            foreach (var identity in idens)
            {
                identites.Add(new(identity.Name + " "+ identity.UserID, identity.IdenId.ToString()));
            }
            return identites;
        }
        public AssetType ListAssetTypeById(int id)
        {
            var assettype = _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == id)
                .First();
            return assettype;
        }
        public void GetAssignedIdentity(Device device)
        {
            device.Identity =  _context.Devices
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.AssetTag == device.AssetTag)
                .Select(x => x.Identity).First();
        }
        public Identity GetAssignedIdentity(int idenId)
        {
            Identity identity = _context.Identities.FirstOrDefault(x => x.IdenId == idenId);
            return identity;
        }
        public async Task AssignIdentity2Device(Identity identity, Device device,string table)
        {
            identity.LastModfiedAdmin = Admin;
            device.LastModfiedAdmin = Admin;
            identity.Devices.Add(device);
            await _context.SaveChangesAsync();
            await LogAssignIdenity2Device(table, identity, device);
            await LogAssignDevice2Identity("identity", device, identity);
        }
        public async Task ReleaseIdenity(Device device, Identity identity, string table)
        {
            identity.LastModfiedAdmin = Admin;
            device.LastModfiedAdmin = Admin;
            identity.Devices.Remove(device);
            device.Identity = null;
            await _context.SaveChangesAsync();
            await LogReleaseIdentityFromDevice(table, identity, device);
            await LogReleaseDeviceFromIdenity("identity", device, identity);
        }
    }
}
