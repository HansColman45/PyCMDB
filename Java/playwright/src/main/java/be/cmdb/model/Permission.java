package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;

@Entity
public class Permission {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private String rights;
    private String description;
    private int lastModifiedAdminId;
    @OneToMany
    @JoinColumn(name = "permissionId", nullable = true)
    private List<Log> logs;

    /**
     * Gets the rights associated with the permission.
     * @return the rights
     */
    public String getRights() {
        return rights;
    }

    /**
     * Sets the rights associated with the permission.
     * @param rights the rights to set
     */
    public void setRights(String rights) {
        this.rights = rights;
    }

    /**
     * Gets the description of the permission.
     * @return the description
     */
    public String getDescription() {
        return description;
    }

    /**
     * Sets the description of the permission.
     * @param description the description to set
     */
    public void setDescription(String description) {
        this.description = description;
    }

    /**
     * Gets the last modified admin ID for the permission.
     * @return the last modified admin ID
     */
    public int getLastModifiedAdminId() {
        return lastModifiedAdminId;
    }

    /**
     * Sets the last modified admin ID for the permission.
     * @param lastModifiedAdminId the last modified admin ID to set
     */
    public void setLastModifiedAdminId(int lastModifiedAdminId) {
        this.lastModifiedAdminId = lastModifiedAdminId;
    }

    /**
     * Gets the ID of the permission.
     * @return the permission ID
     */
    public int getId() {
        return id;
    }

    /**
     * Sets the ID of the permission.
     * @param id the permission ID to set
     */
    public void setId(int id) {
        this.id = id;
    }

    /**
     * Gets the list of logs associated with this permission.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this permission.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }
}
