using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AdminService : LogService
    {
        public AdminService(CMDBContext context) : base(context)
        {
        }
        public async Task<List<Admin>> ListAll()
        {
            List<Admin> admins = await _context.Admins
                .Include(x => x.Account)
                .ToListAsync();
            return admins;
        }
        public async Task<List<Admin>> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<Admin> admins = await _context.Admins
                .Include(x => x.Account)
                .Where(x => EF.Functions.Like(x.Level.ToString(), searhterm) || EF.Functions.Like(x.Account.UserID, searhterm))
                .ToListAsync();
            return admins;
        }
        public async Task<List<Admin>> GetByID(int id)
        {
            List<Admin> admins = await _context.Admins
                .Include(x => x.Account)
                .Where(x => x.AdminId == id)
                .ToListAsync();
            return admins;
        }
        public List<SelectListItem> ListAllLevels()
        {
            List<SelectListItem> Levels = new();
            for (int i = 0; i < 9; i++)
            {
                Levels.Add(new SelectListItem(i.ToString(), i.ToString()));
            }
            return Levels;
        }
        public async Task<List<SelectListItem>> ListActiveCMDBAccounts()
        {
            List<SelectListItem> Levels = new();
            var accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Identities)
                .Where(x => x.Application.Name == "CMDB")
                .ToListAsync();
            foreach (var account in accounts)
            {
                foreach (var idenAcc in account.Identities.Where(x => DateTime.Now <= x.ValidFrom && x.ValidUntil >= DateTime.Now))
                {
                    Levels.Add(new SelectListItem(idenAcc.Account.UserID, idenAcc.Account.AccID.ToString()));
                }
            }
            return Levels;
        }
        public async Task Create(Admin admin, string Table)
        {
            string pwd = new PasswordHasher().EncryptPassword("cmdb");
            admin.Password = pwd;
            admin.DateSet = DateTime.Now;
            admin.LastModfiedAdmin = Admin;
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            string Value = "Admin with UserID: " + admin.Account.UserID + " and level: " + admin.Level.ToString();
            await LogCreate(Table, Admin.AdminId, Value);
        }
        public async Task Update(Admin admin, int level, string Table)
        {
            if (admin.Level != level)
            {
                admin.Level = level;
                admin.LastModfiedAdmin = Admin;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, admin.AdminId, "Level", admin.Level.ToString(), level.ToString());
            }
        }
        public async Task Deactivate(Admin admin, string reason, string table)
        {
            admin.Active = State.Inactive;
            admin.DeactivateReason = reason;
            admin.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Admin with UserID: " + admin.Account.UserID + " and level: " + admin.Level.ToString();
            await LogDeactivate(table, admin.AdminId, Value, reason);
        }
        public async Task Activate(Admin admin, string table)
        {
            admin.Active = State.Active;
            admin.DeactivateReason = "";
            admin.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Admin with UserID: " + admin.Account.UserID + " and level: " + admin.Level.ToString();
            await LogActivate(table, admin.AdminId, Value);
        }
        public bool IsExisting(Admin admin)
        {
            bool result = false;
            var admins = _context.Admins.Include(x => x.Account).Where(x => x.Account.UserID == admin.Account.UserID);
            if(admins.Any())
                result = true;
            return result;
        }
        public async Task<List<Account>> GetAccountByID(int ID)
        {
            List<Account> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == ID)
                .ToListAsync();
            return accounts;
        }
    }
}
