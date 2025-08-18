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
    @JoinColumn(name = "accountId", referencedColumnName="accId")
    @ManyToOne(cascade= CascadeType.PERSIST)
    private Account accountId;
    @JoinColumn(name = "adminId", referencedColumnName="Admin_id")
    @ManyToOne(cascade= CascadeType.PERSIST)
    private Admin adminId;
    @JoinColumn(name = "applicationId", referencedColumnName="appId")
    @ManyToOne(cascade= CascadeType.PERSIST)
    private Application applicationId;
    @JoinColumn(name = "assetTypeId", referencedColumnName="typeId")
    @ManyToOne(cascade= CascadeType.PERSIST)
    private AssetType assetTypeId;
    @JoinColumn(name = "assetCategoryId", referencedColumnName="id")
    @ManyToOne(cascade= CascadeType.PERSIST)
    private Category assetCategoryId;
    @JoinColumn(name = "assetTag", referencedColumnName="assetTag")
    @ManyToOne(cascade= CascadeType.PERSIST)
    private Device assetTag;
    @JoinColumn(name = "identityId", referencedColumnName="idenId")
    @ManyToOne(cascade= CascadeType.PERSIST)
    private Identity identityId;
    @JoinColumn(name = "kensingtonId", referencedColumnName="keyId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Kensington kensingtonId;
    @JoinColumn(name = "menuId", referencedColumnName="menuId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Menu menuId;
    @JoinColumn(name = "mobileId", referencedColumnName="mobileId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Mobile mobileId;
    @JoinColumn(name = "permissionId", referencedColumnName="id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Permission permissionId;
    @JoinColumn(name = "roleId", referencedColumnName="roleId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Role roleId;
    @JoinColumn(name = "rolePermId", referencedColumnName="id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private RolePermission rolePermId;
    @JoinColumn(name = "subscriptionId", referencedColumnName="subscriptionId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Subscription subscriptionId;
    @JoinColumn(name = "subscriptionTypeId", referencedColumnName="id")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private SubscriptionType subscriptionTypeId;
    @JoinColumn(name = "typeId", referencedColumnName="typeId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Type typeId;

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

    public Account getAccountId() {
        return accountId;
    }

    public void setAccountId(Account account) {
        this.accountId = account;
    }

    public Admin getAdminId() {
        return adminId;
    }

    public void setAdminId(Admin admin) {
        this.adminId = admin;
    }

    public Application getApplicationId() {
        return applicationId;
    }

    public void setApplicationId(Application application) {
        this.applicationId = application;
    }

    public AssetType getAssetTypeId() {
        return assetTypeId;
    }

    public void setAssetTypeId(AssetType assetType) {
        this.assetTypeId = assetType;
    }

    public Category getAssetCategoryId() {
        return assetCategoryId;
    }

    public void setAssetCategoryId(Category category) {
        this.assetCategoryId = category;
    }

    public Device getAssetTag() {
        return assetTag;
    }

    public void setAssetTag(Device device) {
        this.assetTag = device;
    }

    public Identity getIdentityId() {
        return identityId;
    }

    public void setIdentityId(Identity identity) {
        this.identityId = identity;
    }

    public Kensington getKensingtonId() {
        return kensingtonId;
    }

    public void setKensingtonId(Kensington kensington) {
        this.kensingtonId = kensington;
    }

    public Menu getMenuId() {
        return menuId;
    }

    public void setMenuId(Menu menu) {
        this.menuId = menu;
    }

    public Mobile getMobileId() {
        return mobileId;
    }

    public void setMobileId(Mobile mobile) {
        this.mobileId = mobile;
    }

    public Permission getPermissionId() {
        return permissionId;
    }

    public void setPermissionId(Permission permission) {
        this.permissionId = permission;
    }

    public Role getRoleId() {
        return roleId;
    }

    public void setRoleId(Role roleId) {
        this.roleId = roleId;
    }

    public RolePermission getRolePermId() {
        return rolePermId;
    }

    public void setRolePermId(RolePermission rolePermission) {
        this.rolePermId = rolePermission;
    }

    public Subscription getSubsriptionId() {
        return subscriptionId;
    }

    public void setSubsriptionId(Subscription subsription) {
        this.subscriptionId = subsription;
    }

    public SubscriptionType getSubscriptionTypId() {
        return subscriptionTypeId;
    }

    public void setSubscriptionTypeId(SubscriptionType subscriptionType) {
        this.subscriptionTypeId = subscriptionType;
    }

    public Type getTypeId() {
        return typeId;
    }

    public void setTypeId(Type type) {
        this.typeId = type;
    }



}
