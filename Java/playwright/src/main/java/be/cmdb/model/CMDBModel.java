package be.cmdb.model;

import jakarta.persistence.Column;
import jakarta.persistence.MappedSuperclass;
import jakarta.validation.constraints.NotNull;

/**
 * Base model class for CMDB entities, providing common fields.
 */
@MappedSuperclass
public class CMDBModel {
    @NotNull
    private Integer active;
    @Column(nullable = false)
    private Integer lastModifiedAdminId;
    @Column(name = "Deactivate_reason", nullable = false)
    private String deactivateReason = null;

    /**
     * Gets the active status of the model.
     * @return the active status, where 1 means active and 0 means inactive
     */
    public Integer getActive() {
        return active;
    }

    /**
     * Sets the active status of the model.
     * @param active the active status to set, where 1 means active and 0 means inactive
     */
    public void setActive(Integer active) {
        this.active = active;
    }

    /**
     * Gets the ID of the last admin who modified the model.
     * @return the ID of the last admin
     */
    public Integer getLastModifiedAdminId() {
        return lastModifiedAdminId;
    }

    /**
     * Sets the ID of the last admin who modified the model.
     * @param lastModifiedAdminId The AdminID of the last modifier
     */
    public void setLastModifiedAdminId(Integer lastModifiedAdminId) {
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
