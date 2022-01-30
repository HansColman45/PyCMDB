using CMDB.Infrastructure;
using System.Collections.Generic;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class CMDBServices
    {
        protected CMDBContext _context;
        public Admin Admin
        {
            get
            {
                if (_context.Admin == null)
                    return null;
                else
                    return _context.Admin;
            }
            set
            {
                if (_context.Admin != null && value != null)
                {
                    if (_context.Admin.AdminId != value.AdminId)
                        _context.Admin = _context.Admins.Where(x => x.AdminId == value.AdminId).SingleOrDefault();
                }
                else if (value is null)
                    _context.Admin = null;
            }
        }

        public CMDBServices(CMDBContext context)
        {
            _context = context;
        }
        #region generic app things
        public string LogDateFormat
        {
            get
            {
                string format = "dd/MM/yyyy";
                Configuration config = _context.Configurations
                    .Where(x => x.Code == "General" && x.SubCode == "LogDateFormat").SingleOrDefault();
                format = config.CFN_Tekst;
                return format;
            }
        }
        public string DateFormat
        {
            get
            {
                string format = "dd/MM/yyyy";
                Configuration config = _context.Configurations
                    .Where(x => x.Code == "General" && x.SubCode == "DateFormat").SingleOrDefault();
                format = config.CFN_Tekst;
                return format;
            }
        }
        public string Company
        {
            get
            {
                string format = "";
                Configuration config = _context.Configurations
                    .Where(x => x.Code == "General" && x.SubCode == "Company").SingleOrDefault();
                format = config.CFN_Tekst;
                return format;
            }
        }
        #endregion

        public Admin Login(string userID, string pwd)
        {
            Admin admin = _context.Admins
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Where(x => x.Account.Application.Name == "CMDB" && x.Account.UserID == userID).FirstOrDefault();

            if (String.Equals(admin.Password, new PasswordHasher().EncryptPassword(pwd)))
            {
                return admin;
            }
            else
            {
                return null;
            }
        }
        protected static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        #region generic menu
        public async Task<ICollection<Menu>> ListFirstMenuLevel()
        {
            var menu = await _context.Menus.Where(x => x.ParentId == null)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
            return menu;
        }
        public async Task<ICollection<Menu>> ListSecondMenuLevel(int menuID)
        {
            var menu = await _context.Menus
                .Include(x => x.Parent)
                .Where(x => x.ParentId == menuID)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
            return menu;
        }
        public async Task<ICollection<Menu>> ListPersonalMenu(int level, int menuID)
        {
            var menu = await _context.RolePerms
                .Include(x => x.Menu)
                .ThenInclude(x => x.Children)
                .Include(x => x.Permission)
                .Where(x => x.Permission.Rights == "Read" || x.Level == level || x.Menu.MenuId == menuID)
                .SelectMany(x => x.Menu.Children).ToListAsync();
            return menu;
        }
        #endregion
    }
}
