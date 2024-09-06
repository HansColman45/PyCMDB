using CMDB.API.Models;
using CMDB.Domain.Entities;

namespace CMDB.API.Services
{
    public interface ILogService
    {
        void GetLogs(string table, int ID, ModelDTO model);
        void GetLogs(string table, string AssetTag, ModelDTO model);
        Task LogCreate(string table, int ID, string Value);
        Task LogCreate(string table, string AssetTag, string Value);
        Task LogUpdate(string table, int ID, string field, string oldValue, string newValue);
        Task LogUpdate(string table, string AssetTag, string field, string oldValue, string newValue);
        Task LogDeactivate(string table, int ID, string value, string reason);
        Task LogDeactivated(string table, string AssetTag, string value, string reason);
        Task LogActivate(string table, int ID, string value);
        Task LogActivate(string table, string AssetTag, string value);
        Task LogAssignIden2Account(string table, int ID, Identity identity, Account account);
        Task LogAssignAccount2Identity(string table, int ID, Account account, Identity identity);
        Task LogReleaseAccountFromIdentity(string table, int IdenId, Identity identity, Account account);
        Task LogReleaseIdentity4Account(string table, int AccId, Identity identity, Account account);
        Task LogAssignDevice2Identity(string table, Device device, Identity identity);
        Task LogAssignMobile2Identity(string table, Mobile mobile, Identity identity);
        Task LogAssignIdentity2Device(string table, Identity identity, Device device);
        Task LogAssignIdentity2Mobile(string table, Identity identity, Mobile mobile);
        Task LogReleaseDeviceFromIdenity(string table, Device device, Identity identity);
        Task LogReleaseIdentityFromDevice(string table, Identity identity, Device device);
        Task LogReleaseMobileFromIdenity(string table, Mobile mobile, Identity identity);
        Task LogReleaseIdentityFromMobile(string table, Identity identity, Mobile mobile);
        Task LogAssignIdentity2Subscription(string table, Identity identity, Subscription subscription);
        Task LogReleaseIdentityFromSubscription(string table, Identity identity, Subscription subscription);
        Task LogAssignSubsciption2Identity(string table, Subscription subscription, Identity identity);
        Task LogReleaseSubscriptionFromIdentity(string table, Subscription subscription, Identity identity);
        Task LogAssignSubscription2Mobile(string table, Mobile mobile, Subscription subscription);
        Task LogReleaseSubscriptionFromMobile(string table, Mobile mobile, Subscription subscription);
        Task LogAssignMobile2Subscription(string table, Subscription subscription, Mobile mobile);
        Task LogReleaseMobileFromSubscription(string table, Subscription subscription, Mobile mobile);
        Task LogPdfFile(string table, int Id, string pdfFile);
        Task LogPdfFile(string table, string AssetTag, string pdfFile);
    }
}
