package be.cmdb.model;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotNull;
import java.time.LocalDateTime;

/**
 * Represents a log entry in the system.
 */
@Entity
@Table(name = "Log")
public class Log {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @NotNull
    private String logText;
    @NotNull
    private LocalDateTime logDate;
    private int accountId;
    private int typeId;
    private int adminId;
    private int applicationId;
    private int assetCategoryId;
    private int assetTypeId;
    private String assetTag;
    private int identityId;
    private int kensingtonId;
    private int menuId;
    private int mobileId;
    private int permissionId;
    private int subscriptionId;
    private int subscriptionTypeId;
    private int roleId;
    private int rolePermId;

    /**
     * Gets the unique identifier of the log entry.
     * @return the ID of the log entry
    */
    public int getId() {
        return id;
    }

    /**
     * Sets the unique identifier of the log entry.
     * @param id the ID to set for the log entry
     */
    public void setId(int id) {
        this.id = id;
    }

    /**
     * Gets the text of the log entry.
     * @return the log text
     */
    public String getLogText() {
        return logText;
    }

    /**
     * Sets the text of the log entry.
     * @param logText the text to set for the log entry
     */
    public void setLogText(String logText) {
        this.logText = logText;
    }

    /**
     * Gets the date and time when the log entry was created.
     * @return the date of the log entry
     */
    public LocalDateTime getLogDate() {
        return logDate;
    }

    /**
     * sets the date and time when the log entry was created.
     * @param logDate the date to set for the log entry
     */
    public void setLogDate(LocalDateTime logDate) {
        this.logDate = logDate;
    }

    /**
     * Gets the account ID associated with the log entry.
     * @return the account ID of the log entry
     */
    public int getAccountId() {
        return accountId;
    }

    /**
     * Sets the account ID associated with the log entry.
     * @param accountId the account ID to set for the log entry
     */
    public void setAccountId(int accountId) {
        this.accountId = accountId;
    }

    /**
     * Gets the type ID associated with the log entry.
     * @return the type ID of the log entry
     */
    public int getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID associated with the log entry.
     * @param typeId the type ID to set for the log entry
     */
    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    /**
     * Gets the admin ID associated with the log entry.
     * @return the admin ID of the log entry
     */
    public int getAdminId() {
        return adminId;
    }

    /**
     * Sets the admin ID associated with the log entry.
     * @param adminId the admin ID to set for the log entry
     */
    public void setAdminId(int adminId) {
        this.adminId = adminId;
    }

    /**
     * Gets the application ID associated with the log entry.
     * @return the application ID of the log entry
     */
    public int getApplicationId() {
        return applicationId;
    }

    /**
     * Sets the application ID associated with the log entry.
     * @param applicationId the application ID to set for the log entry
     */
    public void setApplicationId(int applicationId) {
        this.applicationId = applicationId;
    }

    /**
     * Gets the asset category ID associated with the log entry.
     * @return the asset category ID of the log entry
     */
    public int getAssetCategoryId() {
        return assetCategoryId;
    }

    /**
     * Sets the asset category ID associated with the log entry.
     * @param assetCategoryId the asset category ID to set for the log entry
     */
    public void setAssetCategoryId(int assetCategoryId) {
        this.assetCategoryId = assetCategoryId;
    }

    /**
     * Gets the asset type ID associated with the log entry.
     * @return the asset type ID of the log entry
     */
    public int getAssetTypeId() {
        return assetTypeId;
    }

    /**
     * Sets the asset type ID associated with the log entry.
     * @param assetTypeId the asset type ID to set for the log entry
     */
    public void setAssetTypeId(int assetTypeId) {
        this.assetTypeId = assetTypeId;
    }

    /**
     * Gets the asset tag associated with the log entry.
     * @return the asset tag of the log entry
     */
    public String getAssetTag() {
        return assetTag;
    }

    /**
     * Sets the asset tag associated with the log entry.
     * @param assetTag the asset tag to set for the log entry
     */
    public void setAssetTag(String assetTag) {
        this.assetTag = assetTag;
    }

    /**
     * Gets the identity ID associated with the log entry.
     * @return the identity ID of the log entry
     */
    public int getIdentityId() {
        return identityId;
    }

    /**
     * Sets the identity ID associated with the log entry.
     * @param identityId the identity ID to set for the log entry
     */
    public void setIdentityId(int identityId) {
        this.identityId = identityId;
    }

    /**
     * Gets the Kensington ID associated with the log entry.
     * @return the Kensington ID of the log entry
     */
    public int getKensingtonId() {
        return kensingtonId;
    }

    /**
     * Sets the Kensington ID associated with the log entry.
     * @param kensingtonId the Kensington ID to set for the log entry
     */
    public void setKensingtonId(int kensingtonId) {
        this.kensingtonId = kensingtonId;
    }

    /**
     * Gets the menu ID associated with the log entry.
     * @return the menu ID of the log entry
     */
    public int getMenuId() {
        return menuId;
    }

    /**
     * Sets the menu ID associated with the log entry.
     * @param menuId the menu ID to set for the log entry
     */
    public void setMenuId(int menuId) {
        this.menuId = menuId;
    }

    /**
     * Gets the mobile ID associated with the log entry.
     * @return the mobile ID of the log entry
     */
    public int getMobileId() {
        return mobileId;
    }

    /**
     * Sets the mobile ID associated with the log entry.
     * @param mobileId the mobile ID to set for the log entry
     */
    public void setMobileId(int mobileId) {
        this.mobileId = mobileId;
    }

    /**
     * Gets the permission ID associated with the log entry.
     * @return the permission ID of the log entry
     */
    public int getPermissionId() {
        return permissionId;
    }

    /**
     * sets the permission ID associated with the log entry.
     * @param permissionId the permission ID to set for the log entry
     */
    public void setPermissionId(int permissionId) {
        this.permissionId = permissionId;
    }

    /**
     * Gets the subscription ID associated with the log entry.
     * @return the subscription ID of the log entry
     */
    public int getSubscriptionId() {
        return subscriptionId;
    }

    /**
     * Sets the subscription ID associated with the log entry.
     * @param subscriptionId the subscription ID to set for the log entry
     */
    public void setSubscriptionId(int subscriptionId) {
        this.subscriptionId = subscriptionId;
    }

    /**
     * Gets the subscription type ID associated with the log entry.
     * @return the subscription type ID of the log entry
     */
    public int getSubscriptionTypeId() {
        return subscriptionTypeId;
    }

    /**
     * Sets the subscription type ID associated with the log entry.
     * @param subscriptionTypeId the subscription type ID to set for the log entry
     */
    public void setSubscriptionTypeId(int subscriptionTypeId) {
        this.subscriptionTypeId = subscriptionTypeId;
    }

    /**
     * Gets the role ID associated with the log entry.
     * @return the role ID of the log entry
     */
    public int getRoleId() {
        return roleId;
    }

    /**
     * Sets the role ID associated with the log entry.
     * @param roleId the role ID to set for the log entry
     */
    public void setRoleId(int roleId) {
        this.roleId = roleId;
    }

    /**
     * Gets the role permission ID associated with the log entry.
     * @return the role permission ID of the log entry
     */
    public int getRolePermId() {
        return rolePermId;
    }

    /**
     * Sets the role permission ID associated with the log entry.
     * @param rolePermId the role permission ID to set for the log entry
     */
    public void setRolePermId(int rolePermId) {
        this.rolePermId = rolePermId;
    }
}
