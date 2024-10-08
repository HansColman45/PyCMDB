using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<DeviceDTO>> ListAllFreeDevices();

    }
    public class DeviceRepository : GenericRepository, IDeviceRepository
    {
        public DeviceRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<IEnumerable<DeviceDTO>> ListAllFreeDevices()
        {
            List<DeviceDTO> devices = new();
            var Laptops = await _context.Devices.OfType<Laptop>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.IdentityId == 1)
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var laptop in Laptops)
            {
                devices.Add(laptop);
            }
            var Desktops = await _context.Devices.OfType<Desktop>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.IdentityId == 1)
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var desktop in Desktops)
            {
                devices.Add(desktop);
            }
            var screens = await _context.Devices.OfType<Screen>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.IdentityId == 1)
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var screen in screens)
            {
                devices.Add(screen);
            }
            var dockings = await _context.Devices.OfType<Docking>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.IdentityId == 1)
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var docking in dockings)
            {
                devices.Add(docking);
            }
            var tokens = await _context.Devices.OfType<Token>()
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.IdentityId == 1)
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var token in tokens)
            {
                devices.Add(token);
            }
            return devices;
        }

        public static DeviceDTO ConvertDevice(Device device)
        {
            return new()
            {
                AssetTag = device.AssetTag,
                DeactivateReason = device.DeactivateReason,
                Active = device.active,
                SerialNumber = device.SerialNumber,
                AssetType = new()
                {
                    Type = device.Type.Type,
                    Vendor = device.Type.Vendor,
                    Active = device.Type.active,
                    DeactivateReason = device.Type.DeactivateReason,
                    LastModifiedAdminId = device.Type.LastModifiedAdminId,
                    TypeID = device.Type.TypeID,
                },
                Category = new()
                { 
                    Category = device.Category.Category,
                    Prefix = device.Category.Prefix,
                    Active = device.Category.active,
                    LastModifiedAdminId= device.Category.LastModifiedAdminId,
                    DeactivateReason= device.Category.DeactivateReason,
                    Id = device.Category.Id
                }
            };
        }
    }
}
