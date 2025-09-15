package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;

@Entity(name = "Kensington")
public class Kensington extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int keyId;
    private int amountOfKeys;
    private boolean hasLock;
    private int typeId;
    private int categoryId;
    private String assetTag;

    @OneToMany
    @JoinColumn(name = "kensingtonId", nullable = true)
    private List<Log> logs;

    /**
     * Gets the key ID of the Kensington lock.
     * @return the key ID
     */
    public int getKeyId() {
        return keyId;
    }

    /**
     * Sets the key ID of the Kensington lock.
     * @param keyId the key ID to set
     */
    public void setKeyId(int keyId) {
        this.keyId = keyId;
    }

    /**
     * Gets the amount of keys for the Kensington lock.
     * @return the amount of keys
     */
    public int getAmountOfKeys() {
        return amountOfKeys;
    }

    /**
     * Sets the amount of keys for the Kensington lock.
     * @param amountOfKeys the amount of keys to set
     */
    public void setAmountOfKeys(int amountOfKeys) {
        this.amountOfKeys = amountOfKeys;
    }

    /**
     * Checks if the Kensington lock has a lock.
     * @return true if it has a lock, false otherwise
     */
    public boolean isHasLock() {
        return hasLock;
    }

    /**
     * Sets whether the Kensington lock has a lock.
     * @param hasLock true if it has a lock, false otherwise
     */
    public void setHasLock(boolean hasLock) {
        this.hasLock = hasLock;
    }

    /**
     * Gets the type ID associated with the Kensington lock.
     * @return the type ID
     */
    public int getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID associated with the Kensington lock.
     * @param typeId the type ID to set
     */
    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    /**
     * Gets the category ID associated with the Kensington lock.
     * @return the category ID
     */
    public int getCategoryId() {
        return categoryId;
    }

    /**
     * Sets the category ID associated with the Kensington lock.
     * @param categoryId the category ID to set
     */
    public void setCategoryId(int categoryId) {
        this.categoryId = categoryId;
    }

    /**
     * Gets the asset tag of the Kensington lock.
     * @return the asset tag
     */
    public String getAssetTag() {
        return assetTag;
    }

    /**
     * Sets the asset tag of the Kensington lock.
     * @param assetTag the asset tag to set
     */
    public void setAssetTag(String assetTag) {
        this.assetTag = assetTag;
    }

    /**
     * Gets the list of logs associated with this Kensington lock.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this Kensington lock.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }
}
