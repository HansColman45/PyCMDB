package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;

@Entity
public class Mobile extends CMDBModel{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int mobileId;
    private long imei;
    private int typeId;
    private int identityId;
    private int categoryId;

    @OneToMany
    @JoinColumn(name = "mobileId", nullable = true)
    private List<Log> logs;

    /**
     * Gets the mobile ID.
     * @return the mobile ID
     */
    public int getMobileId() {
        return mobileId;
    }

    /**
     * Sets the mobile ID.
     * @param mobileId the mobile ID to set
     */
    public void setMobileId(int mobileId) {
        this.mobileId = mobileId;
    }

    /**
     * Gets the list of logs associated with this mobile.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this mobile.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }

    /**
     * Gets the IMEI number of the mobile.
     * @return the IMEI number
     */
    public long getImei() {
        return imei;
    }

    /**
     * Sets the IMEI number of the mobile.
     * @param imei the IMEI number to set
     */
    public void setImei(long imei) {
        this.imei = imei;
    }

    /**
     * Gets the type ID of the mobile.
     * @return the type ID
     */
    public int getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID of the mobile.
     * @param typeId the type ID to set
     */
    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    /**
     * Gets the identity ID associated with the mobile.
     * @return the identity ID
     */
    public int getIdentityId() {
        return identityId;
    }

    /**
     * Sets the identity ID associated with the mobile.
     * @param identityId the identity ID to set
     */
    public void setIdentityId(int identityId) {
        this.identityId = identityId;
    }

    /**
     * Gets the category ID of the mobile.
     * @return the category ID
     */
    public int getCategoryId() {
        return categoryId;
    }

    /**
     * Sets the category ID of the mobile.
     * @param categoryId the category ID to set
     */
    public void setCategoryId(int categoryId) {
        this.categoryId = categoryId;
    }
}
