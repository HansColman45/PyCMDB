using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMDB.API.Services
{
    /// <summary>
    /// This is the Device Repository
    /// </summary>
    public class DeviceRepository : GenericRepository, IDeviceRepository
    {
        /// <summary>
        /// The constructor for the Device Repository
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public DeviceRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc/>
        public async Task<bool> IsDeviceExising(DeviceDTO device)
        {
            bool result = false;
            var devices = await _context.Devices.AsNoTracking()
                .Where(x => x.AssetTag == device.AssetTag).AsNoTracking()
                .ToListAsync();
            if (devices.Count > 0)
                result = true;
            return result;
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<RAM>> GetAllRams()
        {
            return await _context.RAMs.AsNoTracking().ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<DeviceDTO>> GetAll(string category)
        {
            List<DeviceDTO> devices = new();
            devices = category switch
            {
                "Laptop" => await _context.Devices.OfType<Laptop>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Desktop" => await _context.Devices.OfType<Desktop>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Docking station" => await _context.Devices
                                        .OfType<Docking>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Token" => await _context.Devices
                                        .OfType<Token>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Monitor" or "Screen" => await _context.Devices
                                        .OfType<Screen>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                _ => throw new Exception($"{category} not found"),
            };
            return devices;
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<DeviceDTO>> GetAll(string category,string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<DeviceDTO> devices = new();
            devices = category switch
            {
                "Laptop" => await _context.Devices.OfType<Laptop>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Desktop" => await _context.Devices.OfType<Desktop>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Docking station" => await _context.Devices
                                        .OfType<Docking>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Token" => await _context.Devices
                                        .OfType<Token>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                "Monitor" or "Screen" => await _context.Devices
                                        .OfType<Screen>().AsNoTracking()
                                        .Include(x => x.Category).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Include(x => x.Identity)
                                        .ThenInclude(x => x.Language).AsNoTracking()
                                        .Include(x => x.Type).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Category).AsNoTracking()
                                        .Include(x => x.Kensington)
                                        .ThenInclude(x => x.Type).AsNoTracking()
                                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                                        .Select(x => ConvertDevice(x))
                                        .ToListAsync(),
                _ => throw new Exception($"{category} not found"),
            };
            return devices;
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<DeviceDTO>> ListAllFreeDevices(string sitePart)
        {
            return sitePart switch
            {
                "Kensington" => await _context.Devices
                    .Include(x => x.Category)
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language)
                    .Include(x => x.Type)
                    .Where(x => x.IdentityId > 1)
                    .Where(x => x.Kensington == null).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .ToListAsync(),
                _ => throw new NotImplementedException($"{sitePart} not implemented"),
            };
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<DeviceDTO>> ListAllFreeDevices()
        {
            List<DeviceDTO> devices = new();
            var Laptops = await _context.Devices.OfType<Laptop>().AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Where(x => x.IdentityId == 1).AsNoTracking()
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var laptop in Laptops)
            {
                devices.Add(laptop);
            }
            var Desktops = await _context.Devices.OfType<Desktop>().AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Where(x => x.IdentityId == 1).AsNoTracking()
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var desktop in Desktops)
            {
                devices.Add(desktop);
            }
            var screens = await _context.Devices.OfType<Screen>().AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Where(x => x.IdentityId == 1).AsNoTracking()
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var screen in screens)
            {
                devices.Add(screen);
            }
            var dockings = await _context.Devices.OfType<Docking>().AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Where(x => x.IdentityId == 1).AsNoTracking()
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var docking in dockings)
            {
                devices.Add(docking);
            }
            var tokens = await _context.Devices.OfType<Token>().AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Where(x => x.IdentityId == 1).AsNoTracking()
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var token in tokens)
            {
                devices.Add(token);
            }
            return devices;
        }
        /// <inheritdoc/>
        public async Task<DeviceDTO> GetByAssetTag(string category, string assetTag)
        {
            switch (category)
            {
                case "Laptop":
                    var laptop = await _context.Devices.OfType<Laptop>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if(laptop is not null)
                    {
                        await GetAssignedIdentity(laptop);
                        await GetAssignedKeys(laptop);
                        GetLogs(category, assetTag, laptop);
                    }
                    return laptop;
                case "Desktop":
                    var desktop = await _context.Devices.OfType<Desktop>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if(desktop is not null) 
                    {
                        await GetAssignedIdentity(desktop);
                        await GetAssignedKeys(desktop);
                        GetLogs(category, assetTag, desktop);
                    }
                    return desktop;
                case "Docking station":
                    var docking = await _context.Devices.OfType<Docking>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if(docking is not null)
                    {
                        await GetAssignedIdentity(docking);
                        await GetAssignedKeys(docking);
                        GetLogs(category, assetTag, docking);
                    }
                    return docking;
                case "Token":
                    var token = await _context.Devices.OfType<Token>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if(token is not null)
                    {
                        await GetAssignedIdentity(token);
                        GetLogs(category, assetTag, token);
                    }
                    return token;
                case "Monitor":
                case "Screen":
                    var screen = await _context.Devices.OfType<Screen>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type).AsNoTracking()
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if (screen is not null)
                    {
                        await GetAssignedIdentity(screen);
                        await GetAssignedKeys(screen);
                        GetLogs(category, assetTag, screen);
                    }
                    return screen;
                default:
                    throw new Exception($"{category} not found");
            }
        }
        /// <inheritdoc/>
        public async Task<DeviceDTO> GetByAssetTag(string assetTag)
        {
            return await _context.Devices.AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Where(x => x.AssetTag == assetTag).AsNoTracking()
                .Select(x => ConvertDevice(x))
                .FirstOrDefaultAsync();
        }
        /// <inheritdoc/>
        public DeviceDTO Create(DeviceDTO deviceDTO)
        {
            string table = deviceDTO.Category.Category.ToLower();
            switch (deviceDTO.Category.Category)
            {
                case "Laptop":
                    Laptop device = new()
                    {
                        TypeId = deviceDTO.AssetType.TypeID,
                        IdentityId = 1,
                        LastModifiedAdminId = TokenStore.AdminId,
                        CategoryId = deviceDTO.Category.Id,
                        active = 1,
                        AssetTag = deviceDTO.AssetTag,
                        DeactivateReason = "",
                        SerialNumber = deviceDTO.SerialNumber,
                        MAC  = deviceDTO.MAC,
                        RAM = deviceDTO.RAM,
                    };
                    device.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.CreateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}", TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Add(device);
                    break;
                case "Desktop":
                    Desktop desktop = new()
                    {
                        TypeId = deviceDTO.AssetType.TypeID,
                        IdentityId = 1,
                        LastModifiedAdminId = TokenStore.AdminId,
                        CategoryId = deviceDTO.Category.Id,
                        active = 1,
                        AssetTag = deviceDTO.AssetTag,
                        DeactivateReason = "",
                        SerialNumber = deviceDTO.SerialNumber,
                        MAC = deviceDTO.MAC,
                        RAM = deviceDTO.RAM,
                    };
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.CreateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}", TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Add(desktop);
                    break;
                case "Token":
                    Token token = new()
                    {
                        active = 1,
                        IdentityId = 1,
                        CategoryId = deviceDTO.Category.Id,
                        AssetTag = deviceDTO.AssetTag,
                        LastModifiedAdminId = TokenStore.AdminId,
                        SerialNumber= deviceDTO.SerialNumber,
                        TypeId = deviceDTO.AssetType.TypeID,
                    };
                    token.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.CreateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}", TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Add(token);
                    break;
                case "Monitor":
                case "Screen":
                    table = "screen";
                    Screen screen = new()
                    {
                        active = 1,
                        IdentityId = 1,
                        CategoryId = deviceDTO.Category.Id,
                        AssetTag = deviceDTO.AssetTag,
                        LastModifiedAdminId = TokenStore.AdminId,
                        SerialNumber = deviceDTO.SerialNumber,
                        TypeId = deviceDTO.AssetType.TypeID,
                    };
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.CreateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}", TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Add(screen);
                    break;
                case "Docking station":
                    table = "docking";
                    Docking docking = new()
                    {
                        active = 1,
                        IdentityId = 1,
                        CategoryId = deviceDTO.Category.Id,
                        AssetTag = deviceDTO.AssetTag,
                        LastModifiedAdminId = TokenStore.AdminId,
                        SerialNumber = deviceDTO.SerialNumber,
                        TypeId = deviceDTO.AssetType.TypeID,
                    };
                    docking.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.CreateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}", TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Add(docking);
                    break;
                default:
                    throw new Exception($"{deviceDTO.Category.Category} not found");
            }
            return deviceDTO;
        }
        /// <inheritdoc/>
        public async Task<DeviceDTO> Update(DeviceDTO deviceDTO)
        {
            switch (deviceDTO.Category.Category)
            {
                case "Laptop":
                    await UpdateLaptop(deviceDTO);
                    break;
                case "Desktop":
                    await UpdateDesktop(deviceDTO);
                    break;
                case "Token":
                    await UpdateToken(deviceDTO);
                    break;
                case "Monitor":
                case "Screen":
                    await UpdateScreen(deviceDTO);
                    break;
                case "Docking station":
                    await UpdateDocing(deviceDTO);
                    break;
                default:
                    throw new Exception($"{deviceDTO.Category.Category} not found");
            };
            return deviceDTO;
        }
        /// <inheritdoc/>
        public async Task<DeviceDTO> Deactivate(DeviceDTO deviceDTO, string reason)
        {
            string table = deviceDTO.Category.Category.ToLower();
            switch (deviceDTO.Category.Category)
            {
                case "Laptop":
                    var laptop = await GetLaptopByAssetTag(deviceDTO.AssetTag);
                    laptop.active = 0;
                    laptop.DeactivateReason = reason;
                    laptop.LastModifiedAdminId = TokenStore.AdminId;
                    laptop.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}", 
                            TokenStore.Admin.Account.UserID, reason,table)
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "Desktop":
                    var desktop = await GetDesktopByAssetTag(deviceDTO.AssetTag);
                    desktop.active = 0;
                    desktop.DeactivateReason = reason;
                    desktop.LastModifiedAdminId = TokenStore.AdminId;
                    desktop.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, reason, table)
                    });
                    break;
                case "Token":
                    var token = await GetTokenByAssetTag(deviceDTO.AssetTag);
                    token.active = 0;
                    token.DeactivateReason = reason;
                    token.LastModifiedAdminId = TokenStore.AdminId;
                    token.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, reason, table)
                    });
                    break;
                case "Monitor":
                case "Screen":
                    table = "screen";
                    var screen = await GetScreenByAssetTag(deviceDTO.AssetTag);
                    screen.active = 0;
                    screen.DeactivateReason = reason;
                    screen.LastModifiedAdminId= TokenStore.AdminId;
                    screen.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, reason, table)
                    });
                    break;
                case "Docking station":
                    table = "docking";
                    var docking = await GetDockingByAssetTag(deviceDTO.AssetTag);
                    docking.active = 0;
                    docking.LastModifiedAdminId = TokenStore.AdminId;
                    docking.DeactivateReason = reason;
                    docking.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, reason, table)
                    });
                    break;
                default:
                    throw new Exception($"{deviceDTO.Category.Category} not found");
            }
            return deviceDTO;
        }
        /// <inheritdoc/>
        public async Task<DeviceDTO> Activate(DeviceDTO deviceDTO)
        {
            string table = deviceDTO.Category.Category.ToLower();
            switch (deviceDTO.Category.Category)
            {
                case "Laptop":
                    var laptop = await GetLaptopByAssetTag(deviceDTO.AssetTag);
                    laptop.active = 1;
                    laptop.DeactivateReason = "";
                    laptop.LastModifiedAdminId = TokenStore.AdminId;
                    laptop.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "Desktop":
                    var desktop = await GetDesktopByAssetTag(deviceDTO.AssetTag);
                    desktop.active = 1;
                    desktop.DeactivateReason = "";
                    desktop.LastModifiedAdminId = TokenStore.AdminId;
                    desktop.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    break;
                case "Token":
                    var token = await GetTokenByAssetTag(deviceDTO.AssetTag);
                    token.active = 1;
                    token.DeactivateReason = "";
                    token.LastModifiedAdminId = TokenStore.AdminId;
                    token.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    break;
                case "Monitor":
                case "Screen":
                    table = "screen";
                    var screen = await GetScreenByAssetTag(deviceDTO.AssetTag);
                    screen.active = 1;
                    screen.DeactivateReason = "";
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    break;
                case "Docking station":
                    table = "docking";
                    var docking = await GetDockingByAssetTag(deviceDTO.AssetTag);
                    docking.active = 1;
                    docking.LastModifiedAdminId = TokenStore.AdminId;
                    docking.DeactivateReason = "";
                    docking.Logs.Add(new()
                    {
                        LogDate = DateTime.Now,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    break;
                default:
                    throw new Exception($"{deviceDTO.Category.Category} not found");
            }
            return deviceDTO;
        }
        /// <inheritdoc/>
        public async Task AssignKensington(DeviceDTO device)
        {
            string table = device.Category.Category.ToLower();
            string deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
            var kensington = await _context.Kensingtons.Where(x => x.KeyID == device.Kensington.KeyID).FirstAsync();
            string keyinfo = $"Kensington with serial number: {device.Kensington?.SerialNumber}";
            kensington.LastModifiedAdminId = TokenStore.AdminId;
            kensington.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(keyinfo,deviceinfo, TokenStore.Admin.Account.UserID, "kensington"),
                LogDate = DateTime.Now
            });
            _context.Kensingtons.Update(kensington);
            switch (device.Category.Category)
            {
                case "Laptop":
                    var laptop = await GetLaptopByAssetTag(device.AssetTag);
                    laptop.LastModifiedAdminId = TokenStore.AdminId;
                    laptop.Kensington = kensington;
                    laptop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo,  TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "Desktop":
                    var desktop = await GetDesktopByAssetTag(device.AssetTag);
                    desktop.LastModifiedAdminId = TokenStore.AdminId;
                    desktop.Kensington = kensington; 
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(desktop);
                    break;
                case "Monitor":
                case "Screen":
                    table = "screen";
                    var screen = await GetScreenByAssetTag(device.AssetTag);
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.Kensington = kensington; ;
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(screen);
                    break;
                case "Docking station":
                    table = "docking";
                    var docking = await GetDockingByAssetTag(device.AssetTag);
                    docking.LastModifiedAdminId = TokenStore.AdminId;
                    docking.Kensington = kensington;
                    docking.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(docking);
                    break;
                default:
                    throw new NotImplementedException($"{device.Category.Category} not implemented");
            }
        }
        /// <inheritdoc/>
        public async Task ReleaseKensington(DeviceDTO device)
        {
            string table = device.Category.Category.ToLower();
            string deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
            var kensington = await _context.Kensingtons.Where(x => x.KeyID == device.Kensington.KeyID).FirstAsync();
            string keyinfo = $"Kensington with serial number: {device.Kensington?.SerialNumber}";
            kensington.LastModifiedAdminId = TokenStore.AdminId;
            kensington.AssetTag = null;
            kensington.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(keyinfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now
            });
            _context.Kensingtons.Update(kensington);
            switch (device.Category.Category)
            {
                case "Laptop":
                    var laptop = await GetLaptopByAssetTag(device.AssetTag);
                    laptop.LastModifiedAdminId = TokenStore.AdminId;
                    laptop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "Desktop":
                    var desktop = await GetDesktopByAssetTag(device.AssetTag);
                    desktop.LastModifiedAdminId = TokenStore.AdminId;
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(desktop);
                    break;
                case "Monitor":
                case "Screen":
                    table = "screen";
                    var screen = await GetScreenByAssetTag(device.AssetTag);
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(screen);
                    break;
                case "Docking station":
                    table = "docking";
                    var docking = await GetDockingByAssetTag(device.AssetTag);
                    docking.LastModifiedAdminId = TokenStore.AdminId;
                    docking.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(docking);
                    break;
                default:
                    throw new NotImplementedException($"{device.Category.Category} not implemented");
            }
        }
        /// <inheritdoc/>
        public async Task<DeviceDTO> AssignIdentity(DeviceDTO device)
        {
            string table = device.Category.Category.ToLower();
            string ideninfo = $"Identity with name: {device.Identity.Name}";
            string deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
            switch (device.Category.Category)
            {
                case "Laptop":
                    var laptop = await GetLaptopByAssetTag(device.AssetTag);
                    laptop.IdentityId = device.Identity.IdenId;
                    laptop.LastModifiedAdminId = TokenStore.AdminId;
                    laptop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now,
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "Desktop":
                    var desktop = await GetDesktopByAssetTag(device.AssetTag);
                    desktop.IdentityId = device.Identity.IdenId;
                    desktop.LastModifiedAdminId = TokenStore.AdminId;
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now,
                    });
                    _context.Devices.Update(desktop);
                    break;
                case "Token":
                    var token = await GetTokenByAssetTag(device.AssetTag);
                    token.LastModifiedAdminId = TokenStore.AdminId;
                    token.IdentityId = device.Identity.IdenId;
                    token.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(token);
                    break;
                case "Monitor":
                case "Screen":
                    table = "screen";
                    var screen = await GetScreenByAssetTag(device.AssetTag);
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.IdentityId = device.Identity.IdenId;
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(screen);
                    break;
                case "Docking station":
                    table = "docking";
                    var doc = await GetDockingByAssetTag(device.AssetTag);
                    doc.LastModifiedAdminId = TokenStore.AdminId;
                    doc.IdentityId = device.Identity.IdenId;
                    doc.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(doc);
                    break;
                default:
                    throw new Exception($"{device.Category.Category} not found");
            }
            var iden = await _context.Identities.Where(x => x.IdenId == device.Identity.IdenId).FirstAsync();
            iden.LastModifiedAdminId = TokenStore.AdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine( ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, "identity"),
                LogDate = DateTime.Now
            });
            _context.Identities.Update(iden);
            return device;
        }
        /// <inheritdoc/>
        public async Task<DeviceDTO> ReleaseIdentity(DeviceDTO device)
        {
            string table = device.Category.Category.ToLower();
            string ideninfo = $"Identity with name: {device.Identity.Name}";
            string deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
            switch (device.Category.Category)
            {
                case "Laptop":
                    var laptop = await GetLaptopByAssetTag(device.AssetTag);
                    laptop.IdentityId = 1;
                    laptop.LastModifiedAdminId = TokenStore.AdminId;
                    laptop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "Desktop":
                    var desktop = await GetDesktopByAssetTag(device.AssetTag);
                    desktop.IdentityId = 1;
                    desktop.LastModifiedAdminId= TokenStore.AdminId;
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(desktop);
                    break;
                case "Token":
                    var token = await GetTokenByAssetTag(device.AssetTag);
                    token.IdentityId = 1;
                    token.LastModifiedAdminId = TokenStore.AdminId;
                    token.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(token);
                    break;
                case "Monitor":
                case "Screen":
                    table = "screen";
                    var screen = await GetScreenByAssetTag(device.AssetTag);
                    screen.IdentityId = 1;
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(screen);
                    break;
                case "Docking station":
                    table = "docking";
                    var doc = await GetDockingByAssetTag(device.AssetTag);
                    doc.IdentityId = 1;
                    doc.LastModifiedAdminId = TokenStore.AdminId;
                    doc.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(doc);
                    break;
                default:
                    throw new Exception($"{device.Category.Category} not found");
            }
            var iden = await _context.Identities.Where(x => x.IdenId == device.Identity.IdenId).FirstAsync();
            iden.LastModifiedAdminId = TokenStore.AdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, "identity"),
                LogDate = DateTime.Now
            });
            _context.Identities.Update(iden);
            return device;
        }
        /// <summary>
        /// This method converts a device to a DeviceDTO object.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static DeviceDTO ConvertDevice(Device device)
        {
            if (device.Kensington is not null)
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
                        CategoryId = device.Type.CategoryId,
                        AssetCategory = new()
                        {
                            Category = device.Category.Category,
                            Prefix = device.Category.Prefix,
                            Active = device.Category.active,
                            LastModifiedAdminId = device.Category.LastModifiedAdminId,
                            DeactivateReason = device.Category.DeactivateReason,
                            Id = device.Category.Id
                        }
                    },
                    Category = new()
                    {
                        Category = device.Category.Category,
                        Prefix = device.Category.Prefix,
                        Active = device.Category.active,
                        LastModifiedAdminId = device.Category.LastModifiedAdminId,
                        DeactivateReason = device.Category.DeactivateReason,
                        Id = device.Category.Id
                    },
                    Identity = new()
                    {
                        Active = device.Identity.active,
                        Company = device.Identity.Company,
                        DeactivateReason = device.Identity.DeactivateReason,
                        UserID = device.Identity.UserID,
                        EMail = device.Identity.EMail,
                        FirstName = device.Identity.FirstName,
                        LastName = device.Identity.LastName,
                        Name = device.Identity.Name,
                        IdenId = device.Identity.IdenId,
                        LastModifiedAdminId = device.Identity.LastModifiedAdminId,
                        Type = new()
                        {
                            Active = device.Identity.Type.active,
                            DeactivateReason = device.Identity.Type.DeactivateReason,
                            Description = device.Identity.Type.Description,
                            Type = device.Identity.Type.Type,
                            TypeId = device.Identity.Type.TypeId,
                            LastModifiedAdminId = device.Identity.LastModifiedAdminId
                        },
                        Language = new()
                        {
                            Code = device.Identity.Language.Code,
                            Description = device.Identity.Type.Description
                        }
                    },
                    Kensington = new()
                    {
                        KeyID = device.Kensington.KeyID,
                        SerialNumber = device.Kensington.SerialNumber,
                        AssetTag = device.Kensington.AssetTag,
                        LastModifiedAdminId = device.Kensington.LastModifiedAdminId,
                        DeactivateReason = device.Kensington.DeactivateReason,
                        Active = device.Kensington.active,
                        Type = new()
                        {
                            Type = device.Kensington.Type.Type,
                            Vendor = device.Kensington.Type.Vendor,
                            Active = device.Kensington.Type.active,
                            DeactivateReason = device.Kensington.Type.DeactivateReason,
                            LastModifiedAdminId = device.Kensington.Type.LastModifiedAdminId,
                            TypeID = device.Kensington.Type.TypeID,
                            CategoryId = device.Kensington.Type.CategoryId,
                            AssetCategory = new()
                            {
                                Category = device.Kensington.Category.Category,
                                Prefix = device.Kensington.Category.Prefix,
                                Active = device.Kensington.Category.active,
                                LastModifiedAdminId = device.Kensington.Category.LastModifiedAdminId,
                                DeactivateReason = device.Kensington.Category.DeactivateReason,
                                Id = device.Kensington.Category.Id
                            }
                        }
                    }
                };
            }
            else
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
                        CategoryId = device.Type.CategoryId,
                        AssetCategory = new()
                        {
                            Category = device.Category.Category,
                            Prefix = device.Category.Prefix,
                            Active = device.Category.active,
                            LastModifiedAdminId = device.Category.LastModifiedAdminId,
                            DeactivateReason = device.Category.DeactivateReason,
                            Id = device.Category.Id
                        }
                    },
                    Category = new()
                    {
                        Category = device.Category.Category,
                        Prefix = device.Category.Prefix,
                        Active = device.Category.active,
                        LastModifiedAdminId = device.Category.LastModifiedAdminId,
                        DeactivateReason = device.Category.DeactivateReason,
                        Id = device.Category.Id
                    },
                    Identity = new()
                    {
                        Active = device.Identity.active,
                        Company = device.Identity.Company,
                        DeactivateReason = device.Identity.DeactivateReason,
                        UserID = device.Identity.UserID,
                        EMail = device.Identity.EMail,
                        FirstName = device.Identity.FirstName,
                        LastName = device.Identity.LastName,
                        Name = device.Identity.Name,
                        IdenId = device.Identity.IdenId,
                        LastModifiedAdminId = device.Identity.LastModifiedAdminId,
                        Type = new()
                        {
                            Active = device.Identity.Type.active,
                            DeactivateReason = device.Identity.Type.DeactivateReason,
                            Description = device.Identity.Type.Description,
                            Type = device.Identity.Type.Type,
                            TypeId = device.Identity.Type.TypeId,
                            LastModifiedAdminId = device.Identity.LastModifiedAdminId
                        },
                        Language = new()
                        {
                            Code = device.Identity.Language.Code,
                            Description = device.Identity.Type.Description
                        }
                    }
                };
        }
        /// <inheritdoc/>
        public async Task LogPdfFile(string category, string pdfFile, string asassetTag)
        {
            switch (category)
            {
                case "screen":
                case "monitor":
                    var screen = await GetScreenByAssetTag(asassetTag);
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(screen);
                    break;
                case "token":
                    var token = await GetTokenByAssetTag(asassetTag);
                    token.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(token);
                    break;
                case "laptop":
                    var laptop = await GetLaptopByAssetTag(asassetTag);
                    laptop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "desktop":
                    var desktop = await GetDesktopByAssetTag(asassetTag);
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(desktop);
                    break;
                case "docking":
                case "docking staion":
                    var docking = await GetDockingByAssetTag(asassetTag);
                    docking.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.Now
                    });
                    _context.Devices.Update(docking);
                    break;
            }
        }
        private async Task<Token> GetTokenByAssetTag(string assetTag)
        {
            return await _context.Devices.OfType<Token>().Where(x => x.AssetTag == assetTag).FirstAsync();
        }
        private async Task<Laptop> GetLaptopByAssetTag(string assetTag)
        {
            return await _context.Devices.OfType<Laptop>().Where(x => x.AssetTag == assetTag).FirstAsync();
        }
        private async Task<Desktop> GetDesktopByAssetTag(string assetTag)
        {
            return await _context.Devices.OfType<Desktop>().Where(x => x.AssetTag == assetTag).FirstAsync();
        }
        private async Task<Screen> GetScreenByAssetTag(string assetTag)
        {
            return await _context.Devices.OfType<Screen>().Where(x => x.AssetTag == assetTag).FirstAsync();
        }
        private async Task<Docking> GetDockingByAssetTag(string assetTag)
        {
            return await _context.Devices.OfType<Docking>().Where(x => x.AssetTag == assetTag).FirstAsync();
        }
        private async Task GetAssignedIdentity(DeviceDTO device)
        {
            device.Identity = await _context.Devices.AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Where(x => x.AssetTag == device.AssetTag).AsNoTracking()
                .Select(x => x.Identity)
                .Select(x => IdentityRepository.ConvertIdentity(x))
                .FirstAsync();
        }
        private async Task GetAssignedKeys(DeviceDTO device)
        {
            device.Kensington = await _context.Kensingtons
                .Include(x => x.Type)
                .ThenInclude(x => x.Category)
                .Where(x => x.AssetTag == device.AssetTag).AsNoTracking()
                .Select(x => KensingtonRepository.Convert2DTO(x)).FirstOrDefaultAsync();
        }
        private async Task UpdateDesktop(DeviceDTO device)
        {
            var desktop = await GetDesktopByAssetTag(device.AssetTag);
            if (string.Compare(desktop.RAM, device.RAM) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("RAM", desktop.RAM, device.RAM, TokenStore.Admin.Account.UserID, "desktop");
                desktop.RAM = device.RAM;
                desktop.LastModifiedAdminId = TokenStore.AdminId;
                desktop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (string.Compare(desktop.MAC, device.MAC) != 0) 
            {
                var logline = GenericLogLineCreator.UpdateLogLine("MAC", desktop.MAC, device.MAC, TokenStore.Admin.Account.UserID, "desktop");
                desktop.MAC = device.MAC;
                desktop.LastModifiedAdminId = TokenStore.AdminId;
                desktop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (string.Compare(desktop.SerialNumber, device.SerialNumber) != 0) 
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", desktop.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "desktop");
                desktop.SerialNumber = device.SerialNumber;
                desktop.LastModifiedAdminId = TokenStore.AdminId;
                desktop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (desktop.TypeId != device.AssetType.TypeID) 
            { 
                var oldType = _context.AssetTypes.Where(x => x.TypeID == desktop.TypeId).First();
                desktop.TypeId = device.AssetType.TypeID;
                desktop.LastModifiedAdminId = TokenStore.AdminId;
                desktop.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "desktop"),
                    LogDate = DateTime.Now
                });
            }
            _context.Devices.Update(desktop);
        }
        private async Task UpdateLaptop(DeviceDTO device)
        {
            var laptop = await GetLaptopByAssetTag(device.AssetTag);
            if (string.Compare(laptop.RAM, device.RAM) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("RAM", laptop.RAM, device.RAM, TokenStore.Admin.Account.UserID, "laptop");
                laptop.RAM = device.RAM;
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (string.Compare(laptop.MAC, device.MAC) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("MAC", laptop.MAC, device.MAC, TokenStore.Admin.Account.UserID, "laptop");
                laptop.MAC = device.MAC;
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (string.Compare(laptop.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", laptop.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "laptop");
                laptop.SerialNumber = device.SerialNumber;
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (laptop.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == laptop.TypeId).First();
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "laptop"),
                    LogDate = DateTime.Now
                });
            }
            _context.Devices.Update(laptop);
        }
        private async Task UpdateToken(DeviceDTO device)
        {
            var token = await GetTokenByAssetTag(device.AssetTag);
            if (string.Compare(token.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", token.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "token");
                token.SerialNumber = device.SerialNumber;
                token.LastModifiedAdminId = TokenStore.AdminId;
                token.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (token.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == token.TypeId).First();
                token.TypeId = device.AssetType.TypeID;
                token.LastModifiedAdminId = TokenStore.AdminId;
                token.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "token"),
                    LogDate = DateTime.Now
                });
            }
            _context.Devices.Update(token);
        }
        private async Task UpdateScreen(DeviceDTO device)
        {
            var screen = await GetScreenByAssetTag(device.AssetTag);
            if (string.Compare(screen.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", screen.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "screen");
                screen.SerialNumber = device.SerialNumber;
                screen.LastModifiedAdminId = TokenStore.AdminId;
                screen.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (screen.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == screen.TypeId).First();
                screen.LastModifiedAdminId = TokenStore.AdminId;
                screen.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "screen"),
                    LogDate = DateTime.Now
                });
            }
            _context.Devices.Update(screen);
        }
        private async Task UpdateDocing(DeviceDTO device)
        {
            var doc = await GetDockingByAssetTag(device.AssetTag);
            if (string.Compare(doc.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", doc.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "docking");
                doc.SerialNumber = device.SerialNumber;
                doc.LastModifiedAdminId = TokenStore.AdminId;
                doc.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.Now
                });
            }
            if (doc.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == doc.TypeId).First();
                doc.LastModifiedAdminId = TokenStore.AdminId;
                doc.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "docking"),
                    LogDate = DateTime.Now
                });
            }
            _context.Devices.Update(doc);
        }
    }
}
