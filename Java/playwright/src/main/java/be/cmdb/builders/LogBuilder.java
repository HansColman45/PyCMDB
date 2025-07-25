package be.cmdb.builders;

import java.time.LocalDateTime;
import java.time.ZoneOffset;
import be.cmdb.model.Log;
import be.cmdb.model.Account;
import be.cmdb.model.Admin;
import be.cmdb.model.Application;
import be.cmdb.model.AssetType;
import be.cmdb.model.Category;
import be.cmdb.model.Device;
import be.cmdb.model.Identity;
import be.cmdb.model.Key;
import be.cmdb.model.Menu;
import be.cmdb.model.Mobile;
import be.cmdb.model.Permission;
import be.cmdb.model.Role;
import be.cmdb.model.RolePermission;
import be.cmdb.model.Subscription;
import be.cmdb.model.SubscriptionType;
import be.cmdb.model.Type;

/**
 * Builder for creating Log entities with default values.
 */
public class LogBuilder extends GenericBuilder<Log> {
    private final Log log;

    /**
     * Constructor for LogBuilder.
     */
    public LogBuilder() {
        super();
        log = new Log();
        log.setLogDate(LocalDateTime.now(ZoneOffset.UTC));
    }

    /**
     * Sets the LogText for the log entry.
     * @param logText string representing the log text
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withLogText(String logText) {
        log.setLogText(logText);
        return this;
    }

    /**
     * Setts the Account associated with the log entry.
     * @param account Account instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withAccount(Account account) {
        log.setAccountId(account);
        return this;
    }
    /**
     * Sets The Admin associated with the log entry.
     * @param admin representing The Admin associated with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withAdmin(Admin admin) {
        log.setAdminId(admin);
        return this;
    }

    /**
     * Sets the Application associated with the log entry.
     * @param application Application instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withApplication(Application application) {
        log.setApplicationId(application);
        return this;
    }

    /**
     * Sets the AssetCategory associated with the log entry.
     * @param category AssetCategory instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withAssetCategory(Category category) {
        log.setAssetCategoryId(category);
        return this;
    }

    /**
     * Sets the AssetType associated with the log entry.
     * @param assetType AssetType instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public  LogBuilder withAssetType(AssetType assetType) {
        log.setAssetTypeId(assetType);
        return this;
    }

    /**
     * Sets the Device associated with the log entry.
     * @param device Device instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withDevice(Device device) {
        log.setAssetTag(device);
        return this;
    }

    /**
     * Sets the Identity associated with the log entry.
     * @param identity Identity instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withIdentity(Identity identity) {
        log.setIdentityId(identity);
        return this;
    }

    /**
     * Sets the Kensington associated with the log entry.
     * @param kensington Key instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withKensington(Key kensington) {
        log.setKensingtonId(kensington);
        return this;
    }

    /**
     * Sets the Menu associated with the log entry.
     * @param menu Menu instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withMenu(Menu menu) {
        log.setMenuId(menu);
        return this;
    }

    /**
     * Sets the Mobile associated with the log entry.
     * @param mobile Mobile instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withMobile(Mobile mobile) {
        log.setMobileId(mobile);
        return this;
    }

    /**
     * Sets the Permission associated with the log entry.
     * @param permission Permission instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withPermission(Permission permission) {
        log.setPermissionId(permission);
        return this;
    }

    /**
     * Sets the Role associated with the log entry.
     * @param role Role instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withRole(Role role) {
        log.setRoleId(role);
        return this;
    }

    /**
     * Sets the RolePermission associated with the log entry.
     * @param rolePermission RolePermission instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withRolePermission(RolePermission rolePermission) {
        log.setRolePermId(rolePermission);
        return this;
    }

    /**
     * Sets the subscription associated with the log entry.
     * @param subscription SubscriptionType instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withSubscription(Subscription subscription) {
        log.setSubsriptionId(subscription);
        return this;
    }

    /**
     * Sets the SubscriptionType associated with the log entry.
     * @param subscriptionType SubscriptionType instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withSubscriptionType(SubscriptionType subscriptionType) {
        log.setSubscriptionTypeId(subscriptionType);
        return this;
    }

    /**
     * Sets the Type associated with the log entry.
     * @param type Type instance to associate with the log entry
     * @return LogBuilder instance for method chaining
     */
    public LogBuilder withType(Type type) {
        log.setTypeId(type);
        return this;
    }

    /**
     * Builds the log entry.
     * @return Log instance
     */
    public Log build(){
        return log;
    }
}
