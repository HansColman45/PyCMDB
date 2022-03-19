using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    public class Admin : Model
    {
        public Admin()
        {
            Devices = new List<Device>();
            Mobiles = new List<Mobile>();
            Kensingtons = new List<Kensington>();
            Subscriptions = new List<Subscription>();
            SubscriptionTypes = new List<SubscriptionType>();
            Identities = new List<Identity>();
            IdenAccounts = new List<IdenAccount>();
            Admins = new List<Admin>();
            AssetCategories = new List<AssetCategory>();
            AssetTypes = new List<AssetType>();
            AccountTypes = new List<AccountType>();
            Applications = new List<Application>();
            RolePerms = new List<RolePerm>();
            Roles = new List<Role>();
            Permissions = new List<Permission>();
            Types = new List<GeneralType>();
        }
        [Column("Admin_id")]
        [Key]
        public int AdminId { get; set; }
        [Required(ErrorMessage = "Please select an account")]
        public Account Account { get; set; }
        public int? AccountId { get; set; }
        [Required(ErrorMessage = "Please select an level")]
        public int Level { get; set; }
        public string Password { get; set; }
        public DateTime DateSet { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Mobile> Mobiles { get; set; }
        public virtual ICollection<Kensington> Kensingtons { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<SubscriptionType> SubscriptionTypes { get; set; }
        public virtual ICollection<Identity> Identities { get; set; }
        public virtual ICollection<GeneralType> Types { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<AccountType> AccountTypes { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<AssetCategory> AssetCategories { get; set; }
        public virtual ICollection<AssetType> AssetTypes { get; set; }
        public virtual ICollection<IdenAccount> IdenAccounts { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<RolePerm> RolePerms { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
