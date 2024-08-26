using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual ICollection<Device> Devices { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mobile> Mobiles { get; set; }
        [JsonIgnore]
        public virtual ICollection<Kensington> Kensingtons { get; set; }
        [JsonIgnore]
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        [JsonIgnore]
        public virtual ICollection<SubscriptionType> SubscriptionTypes { get; set; }
        [JsonIgnore]
        public virtual ICollection<Identity> Identities { get; set; }
        [JsonIgnore]
        public virtual ICollection<GeneralType> Types { get; set; }
        [JsonIgnore]
        public virtual ICollection<Account> Accounts { get; set; }
        [JsonIgnore]
        public virtual ICollection<AccountType> AccountTypes { get; set; }
        [JsonIgnore]
        public virtual ICollection<Admin> Admins { get; set; }
        [JsonIgnore]
        public virtual ICollection<Application> Applications { get; set; }
        [JsonIgnore]
        public virtual ICollection<AssetCategory> AssetCategories { get; set; }
        [JsonIgnore]
        public virtual ICollection<AssetType> AssetTypes { get; set; }
        [JsonIgnore]
        public virtual ICollection<IdenAccount> IdenAccounts { get; set; }
        [JsonIgnore]
        public virtual ICollection<Permission> Permissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<RolePerm> RolePerms { get; set; }
        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
