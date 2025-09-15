package be.cmdb.model;

import java.time.LocalDateTime;

import jakarta.persistence.CascadeType;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotNull;

/**
 * Represents a log entry in the system.
 */
@Entity
@Table(name = "Log")
public class Log {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    @NotNull
    private String logText;
    @NotNull
    private LocalDateTime logDate;
    /**
     * Foreign key relationships to other entities.
     */
    @JoinColumn(name = "accountId", referencedColumnName = "accId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Account accountId;
    @JoinColumn(name = "adminId", referencedColumnName = "Admin_id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Admin adminId;
    @JoinColumn(name = "applicationId", referencedColumnName = "appId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Application application;
    @JoinColumn(name = "assetTypeId", referencedColumnName = "typeId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private AssetType assetTypeId;
    @JoinColumn(name = "assetCategoryId", referencedColumnName = "id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Category assetCategoryId;
    @JoinColumn(name = "assetTag", referencedColumnName = "assetTag")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Device assetTag;
    @JoinColumn(name = "identityId", referencedColumnName = "idenId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Identity identityId;
    @JoinColumn(name = "kensingtonId", referencedColumnName = "keyId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Kensington kensingtonId;
    @JoinColumn(name = "menuId", referencedColumnName = "menuId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Menu menuId;
    @JoinColumn(name = "mobileId", referencedColumnName = "mobileId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Mobile mobileId;
    @JoinColumn(name = "permissionId", referencedColumnName = "id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Permission permissionId;

    @JoinColumn(name = "roleId", referencedColumnName = "roleId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Role roleId;

    @JoinColumn(name = "rolePermId", referencedColumnName = "id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private RolePermission rolePermId;

    @JoinColumn(name = "subscriptionId", referencedColumnName = "subscriptionId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Subscription subscriptionId;

    @JoinColumn(name = "subscriptionTypeId", referencedColumnName = "id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private SubscriptionType subscriptionTypeId;

    @JoinColumn(name = "typeId")
    @ManyToOne
    private Type type;

    /**
     * Gets the unique identifier of the log entry.
     * @return the ID of the log entry
    */
    public Integer getId() {
        return id;
    }

    /**
     * Sets the unique identifier of the log entry.
     * @param id the ID to set for the log entry
     */
    public void setId(Integer id) {
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
     * Gets the account associated with this log entry.
     * @return the account
     */
    public Account getAccountId() {
        return accountId;
    }

    /**
     * Sets the account associated with this log entry.
     * @param account the account to set
     */
    public void setAccountId(Account account) {
        this.accountId = account;
    }

    /**
     * Gets the admin associated with this log entry.
     * @return the admin
     */
    public Admin getAdminId() {
        return adminId;
    }

    /**
     * Sets the admin associated with this log entry.
     * @param admin the admin to set
     */
    public void setAdminId(Admin admin) {
        this.adminId = admin;
    }

    /**
     * Gets the application associated with this log entry.
     * @return the application
     */
    public Application getApplicationId() {
        return application;
    }

    /**
     * Sets the application associated with this log entry.
     * @param application the application to set
     */
    public void setApplicationId(Application application) {
        this.application = application;
    }

    /**
     * Gets the asset type associated with this log entry.
     * @return the asset type
     */
    public AssetType getAssetTypeId() {
        return assetTypeId;
    }

    /**
     * Sets the asset type associated with this log entry.
     * @param assetType the asset type to set
     */
    public void setAssetTypeId(AssetType assetType) {
        this.assetTypeId = assetType;
    }

    /**
     * Gets the asset category associated with this log entry.
     * @return the asset category
     */
    public Category getAssetCategoryId() {
        return assetCategoryId;
    }

    /**
     * Sets the asset category associated with this log entry.
     * @param category the asset category to set
     */
    public void setAssetCategoryId(Category category) {
        this.assetCategoryId = category;
    }

    /**
     * Gets the asset tag (device) associated with this log entry.
     * @return the asset tag (device)
     */
    public Device getAssetTag() {
        return assetTag;
    }

    /**
     * Sets the asset tag (device) associated with this log entry.
     * @param device the asset tag (device) to set
     */
    public void setAssetTag(Device device) {
        this.assetTag = device;
    }

    /**
     * Gets the identity associated with this log entry.
     * @return the identity
     */
    public Identity getIdentityId() {
        return identityId;
    }

    /**
     * Sets the identity associated with this log entry.
     * @param identity the identity to set
     */
    public void setIdentityId(Identity identity) {
        this.identityId = identity;
    }

    /**
     * Gets the kensington key associated with this log entry.
     * @return the kensington key
     */
    public Kensington getKensingtonId() {
        return kensingtonId;
    }

    /**
     * Sets the kensington key associated with this log entry.
     * @param kensington the kensington key to set
     */
    public void setKensingtonId(Kensington kensington) {
        this.kensingtonId = kensington;
    }

    /**
     * Gets the menu associated with this log entry.
     * @return the menu
     */
    public Menu getMenuId() {
        return menuId;
    }

    /**
     * Sets the menu associated with this log entry.
     * @param menu the menu to set
     */
    public void setMenuId(Menu menu) {
        this.menuId = menu;
    }

    /**
     * Gets the mobile device associated with this log entry.
     * @return the mobile device
     */
    public Mobile getMobileId() {
        return mobileId;
    }

    /**
     * Sets the mobile device associated with this log entry.
     * @param mobile the mobile device to set
     */
    public void setMobileId(Mobile mobile) {
        this.mobileId = mobile;
    }

    /**
     * Gets the permission associated with this log entry.
     * @return the permission
     */
    public Permission getPermissionId() {
        return permissionId;
    }

    /**
     * Sets the permission associated with this log entry.
     * @param permission the permission to set
     */
    public void setPermissionId(Permission permission) {
        this.permissionId = permission;
    }

    /**
     * Gets the role associated with this log entry.
     * @return the role
     */
    public Role getRoleId() {
        return roleId;
    }

    /**
     * Sets the role associated with this log entry.
     * @param roleId the role to set
     */
    public void setRoleId(Role roleId) {
        this.roleId = roleId;
    }

    /**
     * Gets the role permission associated with this log entry.
     * @return the role permission
     */
    public RolePermission getRolePermId() {
        return rolePermId;
    }

    /**
     * Sets the role permission associated with this log entry.
     * @param rolePermission the role permission to set
     */
    public void setRolePermId(RolePermission rolePermission) {
        this.rolePermId = rolePermission;
    }

    /**
     * Gets the subscription associated with this log entry.
     * @return the subscription
     */
    public Subscription getSubsriptionId() {
        return subscriptionId;
    }

    /**
     * Sets the subscription associated with this log entry.
     * @param subscription the subscription to set
     */
    public void setSubscriptionId(Subscription subscription) {
        this.subscriptionId = subscription;
    }

    /**
     * Gets the subscription type associated with this log entry.
     * @return the subscription type
     */
    public SubscriptionType getSubscriptionTypId() {
        return subscriptionTypeId;
    }

    /**
     * Sets the subscription type associated with this log entry.
     * @param subscriptionType the subscription type to set
     */
    public void setSubscriptionTypeId(SubscriptionType subscriptionType) {
        this.subscriptionTypeId = subscriptionType;
    }

    /**
     * Gets the type associated with this log entry.
     * @return the type
     */
    public Type getTypeId() {
        return type;
    }

    /**
     * Sets the type associated with this log entry.
     * @param type the type to set
     */
    public void setTypeId(Type type) {
        this.type = type;
    }

}
