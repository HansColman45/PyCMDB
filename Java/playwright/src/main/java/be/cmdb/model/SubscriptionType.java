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

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public List<Log> getLogs() {
        return logs;
    }

    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getProvider() {
        return provider;
    }

    public void setProvider(String provider) {
        this.provider = provider;
    }

    public int getAssetCategoryId() {
        return assetCategoryId;
    }

    public void setAssetCategoryId(int assetCategoryId) {
        this.assetCategoryId = assetCategoryId;
    }

    @Override
    public String toString() {
        return this.provider + " " + this.type + " " + this.description;
    }
}
