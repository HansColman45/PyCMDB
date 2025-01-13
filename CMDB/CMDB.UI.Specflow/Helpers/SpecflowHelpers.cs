namespace CMDB.UI.Specflow.Helpers
{
    public class Identity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string Language { get; set; }
    }
    public class Account
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Application { get; set; }
    }
    public class Device
    {
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
    }
    public class Laptop : Device
    {
        public string MAC { get; set; }
        public string RAM { get; set; }
    }
    public class Desktop : Device
    {
        public string MAC { get; set; }
        public string RAM { get; set; }
    }
    public class DockingStation : Device
    {
    }
    public class Monitor : Device
    {
    }
    public class Token : Device
    {
    }
    public class IdentiyType
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
    public class RoleType
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
    public class AccountType
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
    public class AssetType
    {
        public string Category { get; set; }
        public string Vendor { get; set; }
        public string Type { get; set; }
    }
    public class Mobile
    {
        public string IMEI { get; set; }
        public string Type { get; set; }
    }
    public class SubscriptionType
    {
        public string Category { get; set; }
        public string Provider { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
    public class Subscription
    {
        public string Type { get; set; }
        public string PhoneNumber { get; set; }
    }
}
