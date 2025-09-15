package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;

@Entity
public class Subscription extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int subscriptionId;
    private int subscriptionTypeId;
    private String phoneNumber;
    private int identityId;
    private int mobileId;
    private int assetCategoryId;

    @OneToMany
    @JoinColumn(name = "subsriptionId", nullable = true)
    private List<Log> logs;

    /**
     * Gets the subscription type ID.
     * @return the subscription type ID
     */
    public int getSubscriptionTypeId() {
        return subscriptionTypeId;
    }

    /**
     * Sets the subscription type ID.
     * @param subscriptionTypeId the subscription type ID to set
     */
    public void setSubscriptionTypeId(int subscriptionTypeId) {
        this.subscriptionTypeId = subscriptionTypeId;
    }

    /**
     * Gets the phone number associated with the subscription.
     * @return the phone number
     */
    public String getPhoneNumber() {
        return phoneNumber;
    }

    /**
     * Sets the phone number associated with the subscription.
     * @param phoneNumber the phone number to set
     */
    public void setPhoneNumber(String phoneNumber) {
        this.phoneNumber = phoneNumber;
    }

    /**
     * Gets the identity ID associated with the subscription.
     * @return the identity ID
     */
    public int getIdentityId() {
        return identityId;
    }

    /**
     * Sets the identity ID associated with the subscription.
     * @param identityId the identity ID to set
     */
    public void setIdentityId(int identityId) {
        this.identityId = identityId;
    }

    /**
     * Gets the mobile ID associated with the subscription.
     * @return the mobile ID
     */
    public int getMobileId() {
        return mobileId;
    }

    /**
     * Sets the mobile ID associated with the subscription.
     * @param mobileId the mobile ID to set
     */
    public void setMobileId(int mobileId) {
        this.mobileId = mobileId;
    }

    /**
     * Gets the asset category ID associated with the subscription.
     * @return the asset category ID
     */
    public int getAssetCategoryId() {
        return assetCategoryId;
    }

    /**
     * Sets the asset category ID associated with the subscription.
     * @param assetCategoryId the asset category ID to set
     */
    public void setAssetCategoryId(int assetCategoryId) {
        this.assetCategoryId = assetCategoryId;
    }

    /**
     * Gets the subscription ID.
     * @return the subscription ID
     */
    public int getSubscriptionId() {
        return subscriptionId;
    }

    /**
     * Sets the subscription ID.
     * @param subscriptionId the subscription ID to set
     */
    public void setSubscriptionId(int subscriptionId) {
        this.subscriptionId = subscriptionId;
    }

    /**
     * Gets the list of logs associated with this subscription.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this subscription.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }
}
