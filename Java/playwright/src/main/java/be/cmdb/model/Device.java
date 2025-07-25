package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotNull;

/**
 * The Device class represents an asset in the CMDB.
 */
@Entity(name = "asset")
@Table(name = "asset")
public class Device extends CMDBModel {

    @Id
    @NotNull
    private String assetTag;
    @NotNull
    private String serialNumber;
    @NotNull
    private String discriminator;
    private int typeId;
    private int identityId;
    private int categoryId;
    private String MAC;
    private String RAM;

    @OneToMany(mappedBy = "assetTag")
    private final List<Log> logs  = new java.util.ArrayList<>();

    /**
     * Returns the asset tag of the device.
     * @return string representing the asset tag
     */
    public String getAssetTag() {
        return assetTag;
    }

    /**
     * Sets the asset tag for the device.
     * @param assetTag The assetTag of the Device
     */
    public void setAssetTag(String assetTag) {
        this.assetTag = assetTag;
    }

    /**
     * Returns the serial number of the device.
     * @return string representing the serial number
     */
    public String getSerialNumber() {
        return serialNumber;
    }

    /**
     * Sets the serial number for the device.
     * @param serialNumber The serialNumber of the device
     */
    public void setSerialNumber(String serialNumber) {
        this.serialNumber = serialNumber;
    }

    /**
     * Returns the type ID of the device.
     * @return integer representing the type ID
     */
    public int getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID for the device.
     * @param typeId the TypeId of the device
     */
    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    /**
     * Returns the identity ID associated with the device.
     * @return integer representing the identity ID
     */
    public int getIdentityId() {
        return identityId;
    }

    /**
     * Sets the identity ID for the device.
     * @param identityId the identityId that is linked to the device
     */
    public void setIdentityId(int identityId) {
        this.identityId = identityId;
    }

    /**
     * Returns the category ID of the device.
     * @return integer representing the category ID
     */
    public int getCategoryId() {
        return categoryId;
    }

    /**
     * Sets the category ID for the device.
     * @param categoryId the categoryId of the device
     */
    public void setCategoryId(int categoryId) {
        this.categoryId = categoryId;
    }

    /**
     * Returns the discriminator for the device.
     * @return the discriminator
     */
    public String getDiscriminator() {
        return discriminator;
    }

    /**
     * Sets the discriminator for the device.
     * @param discriminator The discriminator
     */
    public void setDiscriminator(String discriminator) {
        this.discriminator = discriminator;
    }

    /**
     * Sets the MAC address for the device.
     * @param MAC The MAC address of the device
     */
    public void setMAC(String MAC) {
        this.MAC = MAC;
    }

    /**
     * Returns the MAC address of the device.
     * @return string representing the MAC address
     */
    public String getMAC() {
        return this.MAC;
    }

    /**
     * Sets the RAM for the device.
     * @param RAM The RAM of the device
     */
    public void setRAM(String RAM) {
        this.RAM = RAM;
    }

    /**
     * Returns the RAM of the device.
     * @return string representing the RAM
     */
    public String getRAM() {
        return this.RAM;
    }

    /**
     * Returns the list of logs associated with the device.
     * @return a list of Log objects
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Adds a log entry to the device.
     * @param log the Log object to be added
     */
    public void addLog(Log log) {
        log.setAssetTag(this);
        logs.add(log);
    }
}
