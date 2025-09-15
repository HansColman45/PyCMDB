package be.cmdb.model;

import java.time.LocalDateTime;
import java.util.List;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;

/**
 * Represents an administrator in the CMDB system.
 */
@Entity
public class Admin extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Admin_id")
    private int adminId;
    private int accountId;
    private int level;
    private String password;
    private LocalDateTime dateSet;

    @OneToMany(mappedBy = "adminId")
    private final List<Log> logs = new java.util.ArrayList<>();

    /**
     * Gets the unique identifier of the admin.
     * @return the admin ID
     */
    public int getAdminId() {
        return adminId;
    }

    /**
     * Sets the unique identifier of the admin.
     * @param adminId the admin ID to set
     */
    public void setAdminId(int adminId) {
        this.adminId = adminId;
    }

    /**
     * Gets the account ID associated with the admin.
     * @return the account ID
     */
    public int getAccountId() {
        return accountId;
    }

    /**
     * Sets the account ID associated with the admin.
     * @param accountId the account ID to set
     */
    public void setAccountId(int accountId) {
        this.accountId = accountId;
    }

    /**
     * Gets the level of the admin.
     * @return the admin level
     */
    public int getLevel() {
        return level;
    }

    /**
     * Sets the level of the admin.
     * @param level the admin level to set
     */
    public void setLevel(int level) {
        this.level = level;
    }

    /**
     * Gets the password of the admin.
     * @return the admin password
     */
    public String getPassword() {
        return password;
    }

    /**
     * Sets the password of the admin.
     * @param password the admin password to set
     */
    public void setPassword(String password) {
        this.password = password;
    }

    /**
     * Gets the date and time when the password was set.
     * @return the date and time the password was set
     */
    public LocalDateTime getDateSet() {
        return dateSet;
    }

    /**
     * Sets the date and time when the password was set.
     * @param dateSet the date and time to set
     */
    public void setDateSet(LocalDateTime dateSet) {
        this.dateSet = dateSet;
    }

    /**
     * Adds a log entry associated with this admin.
     * @param log the log entry to add
     */
    public void addLogs(Log log) {
        log.setAdminId(this);
        logs.add(log);
    }

    /**
     * Gets the list of log entries associated with this admin.
     * @return the list of log entries
     */
    public List<Log> getLogs() {
        return logs;
    }
}
