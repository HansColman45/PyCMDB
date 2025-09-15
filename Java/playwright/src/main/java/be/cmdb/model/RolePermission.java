package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;

@Entity(name = "roleperm")
public class RolePermission {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private int level;
    private int menuId;
    private int lastModifiedAdminId;
    private int permissionId;

    @OneToMany
    @JoinColumn(name = "rolePermId", nullable = true)
    private List<Log> logs;

    /**
     * Gets the level of the role permission.
     * @return the level
     */
    public int getLevel() {
        return level;
    }

    /**
     * Sets the level of the role permission.
     * @param level the level to set
     */
    public void setLevel(int level) {
        this.level = level;
    }

    /**
     * Gets the menu ID associated with the role permission.
     * @return the menu ID
     */
    public int getMenuId() {
        return menuId;
    }

    /**
     * Sets the menu ID associated with the role permission.
     * @param menuId the menu ID to set
     */
    public void setMenuId(int menuId) {
        this.menuId = menuId;
    }

    /**
     * Gets the last modified admin ID.
     * @return the last modified admin ID
     */
    public int getLastModifiedAdminId() {
        return lastModifiedAdminId;
    }

    /**
     * Sets the last modified admin ID.
     * @param lastModifiedAdminId the last modified admin ID to set
     */
    public void setLastModifiedAdminId(int lastModifiedAdminId) {
        this.lastModifiedAdminId = lastModifiedAdminId;
    }

    /**
     * Gets the permission ID associated with the role permission.
     * @return the permission ID
     */
    public int getPermissionId() {
        return permissionId;
    }

    /**
     * Sets the permission ID associated with the role permission.
     * @param permissionId the permission ID to set
     */
    public void setPermissionId(int permissionId) {
        this.permissionId = permissionId;
    }

    /**
     * Gets the ID of the role permission.
     * @return the ID
     */
    public int getId() {
        return id;
    }

    /**
     * Sets the ID of the role permission.
     * @param id the ID to set
     */
    public void setId(int id) {
        this.id = id;
    }

    /**
     * Gets the list of logs associated with this role permission.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this role permission.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }
}
