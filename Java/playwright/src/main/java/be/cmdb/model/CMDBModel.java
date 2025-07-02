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
    private String deactiveReason;

    /**
     * Gets the active status of the model.
     * @return
     */
    public int getActive() {
        return active;
    }

    /**
     * Sets the active status of the model.
     * @param active
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
     * @param lastModifiedAdminId
     */
    public void setLastModifiedAdminId(int lastModifiedAdminId) {
        this.lastModifiedAdminId = lastModifiedAdminId;
    }

    /**
     * Gets the reason for deactivation of the model.
     * @return
     */
    public String getDeactiveReason() {
        return deactiveReason;
    }

    /**
     * Sets the reason for deactivation of the model.
     * @param deactiveReason
     */
    public void setDeactiveReason(String deactiveReason) {
        this.deactiveReason = deactiveReason;
    }
}
