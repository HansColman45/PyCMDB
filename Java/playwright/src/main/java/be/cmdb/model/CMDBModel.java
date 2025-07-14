package be.cmdb.model;

import jakarta.persistence.Column;
import jakarta.validation.constraints.NotNull;

/**
 * Base model class for CMDB entities, providing common fields.
 */
public class CMDBModel {
    @NotNull
    private int active;
    private int lastModifiedAdminId;
    @Column(name = "Deactivate_reason")
    private String deactivateReason;

    /**
     * Gets the active status of the model.
     * @return the active status, where 1 means active and 0 means inactive
     */
    public int getActive() {
        return active;
    }

    /**
     * Sets the active status of the model.
     * @param active the active status to set, where 1 means active and 0 means inactive
     */
    public void setActive(int active) {
        this.active = active;
    }

    /**
     * Gets the ID of the last admin who modified the model.
     * @return the ID of the last admin
     */
    public int getLastModifiedAdminId() {
        return lastModifiedAdminId;
    }

    /**
     * Sets the ID of the last admin who modified the model.
     * @param lastModifiedAdminId The AdminID of the last modifier
     */
    public void setLastModifiedAdminId(int lastModifiedAdminId) {
        this.lastModifiedAdminId = lastModifiedAdminId;
    }

    /**
     * Gets the reason for deactivation of the model.
     * @return The reason for deactivation, if any
     */
    public String getDeactivateReason() {
        return deactivateReason;
    }

    /**
     * Sets the reason for deactivation of the model.
     * @param deactivateReason The reason for deactivation
     */
    public void setDeactivateReason(String deactivateReason) {
        this.deactivateReason = deactivateReason;
    }
}
