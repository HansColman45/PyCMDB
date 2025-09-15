package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;

@Entity
public class AssetType extends CMDBModel{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int typeId;
    private String vendor;
    private String type;
    private int categoryId;

    @OneToMany(mappedBy = "assetTypeId")
    private final List<Log> logs = new java.util.ArrayList<>();

    /**
     * Gets the type ID of the asset type.
     * @return the type ID
     */
    public int getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID of the asset type.
     * @param typeId the type ID to set
     */
    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    /**
     * Gets the list of logs associated with this asset type.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Adds a log to the list of logs associated with this asset type.
     * @param log the log to add
     */
    public void addLog(Log log) {
        log.setAssetTypeId(this);
        logs.add(log);
    }

    /**
     * Gets the vendor of the asset type.
     * @return the vendor
     */
    public String getVendor() {
        return vendor;
    }

    /**
     * Sets the vendor of the asset type.
     * @param vendor the vendor to set
     */
    public void setVendor(String vendor) {
        this.vendor = vendor;
    }

    /**
     * Gets the type name of the asset type.
     * @return the type name
     */
    public String getType() {
        return type;
    }

    /**
     * Sets the type name of the asset type.
     * @param type the type name to set
     */
    public void setType(String type) {
        this.type = type;
    }

    /**
     * Gets the category ID of the asset type.
     * @return the category ID
     */
    public int getCategoryId() {
        return categoryId;
    }

    /**
     * Sets the category ID of the asset type.
     * @param categoryId the category ID to set
     */
    public void setCategoryId(int categoryId) {
        this.categoryId = categoryId;
    }

    @Override
    public String toString() {
        return getVendor() + " " + getType();
    }
}
