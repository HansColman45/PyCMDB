package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;

@Entity
public class SubscriptionType extends CMDBModel{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private String type;
    private String description;
    private String provider;
    private int assetCategoryId;
    @OneToMany
    @JoinColumn(name = "subscriptionTypeId", nullable = true)
    private List<Log> logs;

    /**
     * Gets the ID of the subscription type.
     * @return the ID
     */
    public int getId() {
        return id;
    }

    /**
     * Sets the ID of the subscription type.
     * @param id the ID to set
     */
    public void setId(int id) {
        this.id = id;
    }

    /**
     * Gets the list of logs associated with this subscription type.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this subscription type.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }

    /**
     * Gets the type name of the subscription type.
     * @return the type name
     */
    public String getType() {
        return type;
    }

    /**
     * Sets the type name of the subscription type.
     * @param type the type name to set
     */
    public void setType(String type) {
        this.type = type;
    }

    /**
     * Gets the description of the subscription type.
     * @return the description
     */
    public String getDescription() {
        return description;
    }

    /**
     * Sets the description of the subscription type.
     * @param description the description to set
     */
    public void setDescription(String description) {
        this.description = description;
    }

    /**
     * Gets the provider of the subscription type.
     * @return the provider
     */
    public String getProvider() {
        return provider;
    }

    /**
     * Sets the provider of the subscription type.
     * @param provider the provider to set
     */
    public void setProvider(String provider) {
        this.provider = provider;
    }

    /**
     * Gets the asset category ID associated with this subscription type.
     * @return the asset category ID
     */
    public int getAssetCategoryId() {
        return assetCategoryId;
    }

    /**
     * Sets the asset category ID associated with this subscription type.
     * @param assetCategoryId the asset category ID to set
     */
    public void setAssetCategoryId(int assetCategoryId) {
        this.assetCategoryId = assetCategoryId;
    }

    @Override
    public String toString() {
        return this.provider + " " + this.type + " " + this.description;
    }
}
