using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IDeviceRepository
    {
        /// <summary>
        /// This will return a list of devices that are not linked to a identity
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DeviceDTO>> ListAllFreeDevices();
        /// <summary>
        /// This will give a list of devices
        /// </summary>
        /// <param name="category">The category of the device</param>
        /// <returns></returns>
        Task<IEnumerable<DeviceDTO>> GetAll(string category);
        /// <summary>
        /// This will give a list of devices
        /// </summary>
        /// <param name="category">The category of the device</param>
        /// <param name="searchString">The search string</param>
        /// <returns></returns>
        Task<IEnumerable<DeviceDTO>> GetAll(string category,string searchString);
        /// <summary>
        /// This function will get the details of a Device
        /// </summary>
        /// <param name="category">The category of the device</param>
        /// <param name="assetTag">The AssetTag of the device</param>
        /// <returns></returns>
        Task<DeviceDTO?> GetByAssetTag(string category, string assetTag);
        /// <summary>
        /// This will create a new device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <returns></returns>
        DeviceDTO Create(DeviceDTO deviceDTO);
        /// <summary>
        /// This will update an existing Device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <returns></returns>
        Task<DeviceDTO> Update(DeviceDTO deviceDTO);
        /// <summary>
        /// This will deactivate an existing device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<DeviceDTO> Deactivate(DeviceDTO deviceDTO, string reason);
        /// <summary>
        /// This will activate an existing device
        /// </summary>
        /// <param name="deviceDTO"></param>
        /// <returns></returns>
        Task<DeviceDTO> Activate(DeviceDTO deviceDTO);
        /// <summary>
        /// This will return a list of RAMS
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RAM>> GetAllRams();
        Task LogPdfFile(string category, string pdfFile, string asassetTag);
        Task<DeviceDTO> AssignIdentity(DeviceDTO device);
        Task<DeviceDTO> ReleaseIdentity(DeviceDTO device);
        Task<bool> IsDeviceExising(DeviceDTO device);
    }
    public class DeviceRepository : GenericRepository, IDeviceRepository
    {
        public DeviceRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
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
        public async Task<IEnumerable<RAM>> GetAllRams()
        {
            return await _context.RAMs.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<DeviceDTO>> GetAll(string category)
        {
            List<DeviceDTO> devices = new();
            switch (category)
            {
                case "Laptop":
                    devices = await _context.Devices.OfType<Laptop>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                case "Desktop":
                    devices = await _context.Devices.OfType<Desktop>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                case "Docking station":
                    var dockings = await _context.Devices
                        .OfType<Docking>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                case "Token":
                    var tokens = await _context.Devices
                        .OfType<Token>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                case "Monitor":
                case "Screen":
                    var screens = await _context.Devices
                        .OfType<Screen>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                default:
                    throw new Exception($"{category} not found");
            }
            return devices;
        }
        public async Task<IEnumerable<DeviceDTO>> GetAll(string category,string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<DeviceDTO> devices = new();
            switch (category)
            {
                case "Laptop":
                    devices = await _context.Devices.OfType<Laptop>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                case "Desktop":
                    devices = await _context.Devices.OfType<Desktop>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .Select(x => ConvertDesktop(x))
                        .ToListAsync();
                    break;
                case "Docking station":
                    var dockings = await _context.Devices
                        .OfType<Docking>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                case "Token":
                    var tokens = await _context.Devices
                        .OfType<Token>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                case "Monitor":
                case "Screen":
                    var screens = await _context.Devices
                        .OfType<Screen>().AsNoTracking()
                        .Include(x => x.Category).AsNoTracking()
                        .Include(x => x.Identity).AsNoTracking()
                        .Include(x => x.Type).AsNoTracking()
                        .Where(x => EF.Functions.Like(x.SerialNumber, searhterm) || EF.Functions.Like(x.AssetTag, searhterm))
                        .Select(x => ConvertDevice(x))
                        .ToListAsync();
                    break;
                default:
                    throw new Exception($"{category} not found");
            }
            return devices;
        }
        public async Task<IEnumerable<DeviceDTO>> ListAllFreeDevices()
        {
            List<DeviceDTO> devices = new();
            var Laptops = await _context.Devices.OfType<Laptop>().AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
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
                .Where(x => x.IdentityId == 1).AsNoTracking()
                .Select(x => ConvertDesktop(x))
                .ToListAsync();
            foreach (var desktop in Desktops)
            {
                devices.Add(desktop);
            }
            var screens = await _context.Devices.OfType<Screen>().AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
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
                .Where(x => x.IdentityId == 1).AsNoTracking()
                .Select(x => ConvertDevice(x))
                .ToListAsync();
            foreach (var token in tokens)
            {
                devices.Add(token);
            }
            return devices;
        }
        public async Task<DeviceDTO?> GetByAssetTag(string category, string assetTag)
        {
            switch (category)
            {
                case "Laptop":
                    var laptop = await _context.Devices.OfType<Laptop>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if(laptop is not null)
                    {
                        await GetAssignedIdentity(laptop);
                        GetLogs(category, assetTag, laptop);
                    }
                    return laptop;
                case "Desktop":
                    var desktop = await _context.Devices.OfType<Desktop>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDesktop(x))
                    .FirstOrDefaultAsync();
                    if(desktop is not null) 
                    {
                        await GetAssignedIdentity(desktop);
                        GetLogs(category, assetTag, desktop);
                    }
                    return desktop;
                case "Docking station":
                    var docking = await _context.Devices.OfType<Docking>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if(docking is not null)
                    {
                        await GetAssignedIdentity(docking);
                        GetLogs(category, assetTag, docking);
                    }
                    return docking;
                case "Token":
                    var token = await _context.Devices.OfType<Token>().AsNoTracking()
                    .Include(x => x.Category).AsNoTracking()
                    .Include(x => x.Identity).AsNoTracking()
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
                    .Include(x => x.Identity).AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Where(x => x.AssetTag == assetTag).AsNoTracking()
                    .Select(x => ConvertDevice(x))
                    .FirstOrDefaultAsync();
                    if (screen is not null)
                    {
                        await GetAssignedIdentity(screen);
                        GetLogs(category, assetTag, screen);
                    }
                    return screen;
                default:
                    throw new Exception($"{category} not found");
            }
        }
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Add(token);
                    break;
                case "Monitor":
                case "Screen":
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
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Add(screen);
                    break;
                case "Docking station":
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
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Add(docking);
                    break;
                default:
                    throw new Exception($"{deviceDTO.Category.Category} not found");
            }
            return deviceDTO;
        }
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, reason, table)
                    });
                    break;
                case "Monitor":
                case "Screen":
                    var screen = await GetScreenByAssetTag(deviceDTO.AssetTag);
                    screen.active = 0;
                    screen.DeactivateReason = reason;
                    screen.LastModifiedAdminId= TokenStore.AdminId;
                    screen.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, reason, table)
                    });
                    break;
                case "Docking station":
                    var docking = await GetDockingByAssetTag(deviceDTO.AssetTag);
                    docking.active = 0;
                    docking.LastModifiedAdminId = TokenStore.AdminId;
                    docking.DeactivateReason = reason;
                    docking.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = GenericLogLineCreator.DeleteLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, reason, table)
                    });
                    break;
                default:
                    throw new Exception($"{deviceDTO.Category.Category} not found");
            }
            return deviceDTO;
        }
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    break;
                case "Monitor":
                case "Screen":
                    var screen = await GetScreenByAssetTag(deviceDTO.AssetTag);
                    screen.active = 1;
                    screen.DeactivateReason = "";
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    break;
                case "Docking station":
                    var docking = await GetDockingByAssetTag(deviceDTO.AssetTag);
                    docking.active = 1;
                    docking.LastModifiedAdminId = TokenStore.AdminId;
                    docking.DeactivateReason = "";
                    docking.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = GenericLogLineCreator.ActivateLogLine($"{deviceDTO.Category.Category} with type {deviceDTO.AssetType}",
                            TokenStore.Admin.Account.UserID, table)
                    });
                    break;
                default:
                    throw new Exception($"{deviceDTO.Category.Category} not found");
            }
            return deviceDTO;
        }
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
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
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Update(token);
                    break;
                case "Monitor":
                case "Screen":
                    var screen = await GetScreenByAssetTag(device.AssetTag);
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.IdentityId = device.Identity.IdenId;
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Update(screen);
                    break;
                case "Docking station":
                    var doc = await GetDockingByAssetTag(device.AssetTag);
                    doc.LastModifiedAdminId = TokenStore.AdminId;
                    doc.IdentityId = device.Identity.IdenId;
                    doc.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.UtcNow,
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
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine( ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow,
            });
            _context.Identities.Update(iden);
            return device;
        }
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
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "Desktop":
                    var desktop = await GetDesktopByAssetTag(device.AssetTag);
                    desktop.IdentityId = 1;
                    desktop.LastModifiedAdminId= TokenStore.AdminId;
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Update(desktop);
                    break;
                case "Token":
                    var token = await GetTokenByAssetTag(device.AssetTag);
                    token.IdentityId = 1;
                    token.LastModifiedAdminId = TokenStore.AdminId;
                    token.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Update(token);
                    break;
                case "Monitor":
                case "Screen":
                    var screen = await GetScreenByAssetTag(device.AssetTag);
                    screen.IdentityId = 1;
                    screen.LastModifiedAdminId = TokenStore.AdminId;
                    screen.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.UtcNow,
                    });
                    _context.Devices.Update(screen);
                    break;
                case "Docking station":
                    var doc = await GetDockingByAssetTag(device.AssetTag);
                    doc.IdentityId = 1;
                    doc.LastModifiedAdminId = TokenStore.AdminId;
                    doc.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                        LogDate = DateTime.UtcNow,
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
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow,
            });
            _context.Identities.Update(iden);
            return device;
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
                    LastModifiedAdminId = device.Identity.LastModifiedAdminId
                }
            };
        }
        public static DeviceDTO ConvertDesktop(Desktop desktop)
        {
            return new()
            {
                AssetTag = desktop.AssetTag,
                DeactivateReason = desktop.DeactivateReason,
                Active = desktop.active,
                SerialNumber = desktop.SerialNumber,
                RAM = desktop.RAM,
                AssetType = new()
                {
                    Type = desktop.Type.Type,
                    Vendor = desktop.Type.Vendor,
                    Active = desktop.Type.active,
                    DeactivateReason = desktop.Type.DeactivateReason,
                    LastModifiedAdminId = desktop.Type.LastModifiedAdminId,
                    TypeID = desktop.Type.TypeID,
                    CategoryId = desktop.Type.CategoryId,
                    AssetCategory = new()
                    {
                        Category = desktop.Category.Category,
                        Prefix = desktop.Category.Prefix,
                        Active = desktop.Category.active,
                        LastModifiedAdminId = desktop.Category.LastModifiedAdminId,
                        DeactivateReason = desktop.Category.DeactivateReason,
                        Id = desktop.Category.Id
                    }
                },
                Category = new()
                {
                    Category = desktop.Category.Category,
                    Prefix = desktop.Category.Prefix,
                    Active = desktop.Category.active,
                    LastModifiedAdminId = desktop.Category.LastModifiedAdminId,
                    DeactivateReason = desktop.Category.DeactivateReason,
                    Id = desktop.Category.Id
                },
                Identity = new()
                {
                    Active = desktop.Identity.active,
                    Company = desktop.Identity.Company,
                    DeactivateReason = desktop.Identity.DeactivateReason,
                    UserID = desktop.Identity.UserID,
                    EMail = desktop.Identity.EMail,
                    FirstName = desktop.Identity.FirstName,
                    LastName = desktop.Identity.LastName,
                    Name = desktop.Identity.Name,
                    IdenId = desktop.Identity.IdenId,
                    LastModifiedAdminId = desktop.Identity.LastModifiedAdminId
                }
            };
        }
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
                        LogDate = DateTime.UtcNow
                    });
                    _context.Devices.Update(screen);
                    break;
                case "token":
                    var token = await GetTokenByAssetTag(asassetTag);
                    token.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.UtcNow
                    });
                    _context.Devices.Update(token);
                    break;
                case "laptop":
                    var laptop = await GetLaptopByAssetTag(asassetTag);
                    laptop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.UtcNow
                    });
                    _context.Devices.Update(laptop);
                    break;
                case "desktop":
                    var desktop = await GetDesktopByAssetTag(asassetTag);
                    desktop.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.UtcNow
                    });
                    _context.Devices.Update(desktop);
                    break;
                case "docking":
                case "docking staion":
                    var docking = await GetDockingByAssetTag(asassetTag);
                    docking.Logs.Add(new()
                    {
                        LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                        LogDate = DateTime.UtcNow
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
                    LogDate = DateTime.UtcNow
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
                    LogDate = DateTime.UtcNow
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
                    LogDate = DateTime.UtcNow
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
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Devices.Update(desktop);
        }
        private async Task UpdateLaptop(DeviceDTO device)
        {
            var laptop = await GetLaptopByAssetTag(device.AssetTag);
            if (string.Compare(laptop.RAM, device.RAM) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("RAM", laptop.RAM, device.RAM, TokenStore.Admin.Account.UserID, "desktop");
                laptop.RAM = device.RAM;
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow
                });
            }
            if (string.Compare(laptop.MAC, device.MAC) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("MAC", laptop.MAC, device.MAC, TokenStore.Admin.Account.UserID, "desktop");
                laptop.MAC = device.MAC;
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow
                });
            }
            if (string.Compare(laptop.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", laptop.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "desktop");
                laptop.SerialNumber = device.SerialNumber;
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow
                });
            }
            if (laptop.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == laptop.TypeId).First();
                laptop.LastModifiedAdminId = TokenStore.AdminId;
                laptop.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "desktop"),
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Devices.Update(laptop);
        }
        private async Task UpdateToken(DeviceDTO device)
        {
            var token = await GetTokenByAssetTag(device.AssetTag);
            if (string.Compare(token.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", token.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "desktop");
                token.SerialNumber = device.SerialNumber;
                token.LastModifiedAdminId = TokenStore.AdminId;
                token.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow
                });
            }
            if (token.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == token.TypeId).First();
                token.LastModifiedAdminId = TokenStore.AdminId;
                token.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "desktop"),
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Devices.Update(token);
        }
        private async Task UpdateScreen(DeviceDTO device)
        {
            var screen = await GetScreenByAssetTag(device.AssetTag);
            if (string.Compare(screen.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", screen.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "desktop");
                screen.SerialNumber = device.SerialNumber;
                screen.LastModifiedAdminId = TokenStore.AdminId;
                screen.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow
                });
            }
            if (screen.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == screen.TypeId).First();
                screen.LastModifiedAdminId = TokenStore.AdminId;
                screen.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "desktop"),
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Devices.Update(screen);
        }
        private async Task UpdateDocing(DeviceDTO device)
        {
            var doc = await GetDockingByAssetTag(device.AssetTag);
            if (string.Compare(doc.SerialNumber, device.SerialNumber) != 0)
            {
                var logline = GenericLogLineCreator.UpdateLogLine("SerialNumber", doc.SerialNumber, device.SerialNumber, TokenStore.Admin.Account.UserID, "desktop");
                doc.SerialNumber = device.SerialNumber;
                doc.LastModifiedAdminId = TokenStore.AdminId;
                doc.Logs.Add(new()
                {
                    LogText = logline,
                    LogDate = DateTime.UtcNow
                });
            }
            if (doc.TypeId != device.AssetType.TypeID)
            {
                var oldType = _context.AssetTypes.Where(x => x.TypeID == doc.TypeId).First();
                doc.LastModifiedAdminId = TokenStore.AdminId;
                doc.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("Type", oldType.ToString(), device.AssetType.ToString(), TokenStore.Admin.Account.UserID, "desktop"),
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Devices.Update(doc);
        }
    }
}
