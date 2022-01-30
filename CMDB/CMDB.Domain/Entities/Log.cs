using System;

namespace CMDB.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogText { get; set; }

        public Account Account { get; set; }
        public int? AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public int? AccountTypeId { get; set; }
        public Admin Admin { get; set; }
        public int? AdminId { get; set; }
        public Application Application { get; set; }
        public int? ApplicationId { get; set; }
        public AssetCategory Category { get; set; }
        public int? AssetCategoryId { get; set; }
        public AssetType AssetType { get; set; }
        public int? AssetTypeId { get; set; }
        public Device Device { get; set; }
        public string AssetTag { get; set; }
        public Identity Identity { get; set; }
        public int? IdentityId { get; set; }
        public IdentityType IdentityType { get; set; }
        public int? IdentityTypeId { get; set; }
        public Kensington Kensington { get; set; }
        public int? KensingtonId { get; set; }
        public Menu Menu { get; set; }
        public int? MenuId { get; set; }
        public Mobile Mobile { get; set; }
        public int? MobileId { get; set; }
        public Permission Permission { get; set; }
        public int? PermissionId { get; set; }
        public Subscription Subscription { get; set; }
        public int? SubsriptionId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int? SubscriptionTypeId { get; set; }
        public Role Role { get; set; }
        public int? RoleId { get; set; }
        public RoleType RoleType { get; set; }
        public int? RoleTypeId { get; set; }
    }
}