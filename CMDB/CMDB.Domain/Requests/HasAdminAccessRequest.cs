namespace CMDB.Domain.Requests
{
    /// <summary>
    /// The request to check if the admin has access to a site
    /// </summary>
    public class HasAdminAccessRequest
    {
        /// <summary>
        /// The admin ID
        /// </summary>
        public required int AdminId { get; set; }
        /// <summary>
        /// The part of the site requesting acces to
        /// </summary>
        public required string Site { get; set; }
        /// <summary>
        /// The permission the admin is trying to perform
        /// </summary>
        public required Permission Permission { get; set; }
    }
    public enum Permission
    {
        Read = 1,
        Update = 2,
        Delete = 3,
        Activate = 4,
        Add = 5,
        AssignAccount = 6,
        AccountOverview = 7,
        AssignDevice = 8,
        DeviceOverview = 9,
        AssignMobile = 10,
        MobileOverview = 11,
        AssignSubscription = 12,
        SubscriptionOverview = 13,
        AssignIdentity = 14,
        IdentityOverview = 15,
        AssignAppRole = 16,
        AppRoleOverview = 17,
        AssignSysRole = 18,
        SysRoleOverview = 19,
        AssignApplication = 20,
        ApplicationOverview = 21,
        AssignSystem = 22,
        SystemOverview = 23,
        AssignKensington = 24,
        KeyOverview = 25,
        ReleaseDevice = 26,
        ReleaseAccount = 27,
        ReleaseMobile = 28,
        ReleaseSubscription = 29,
        ReleaseIdentity = 30,
        ReleaseAppRole = 31,
        ReleaseSysRole = 32,
        ReleaseApplication = 33,
        ReleaseSystem = 34,
        ReleaseKensington = 35,
        AssignLevel = 36,
        PermissionOverview = 37,
        MenuOverview = 38,
        RorePermOverview = 39
    }
}
