package be.cmdb.model;

import java.util.Date;
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
    private Date dateSet;
    @OneToMany(mappedBy = "adminId")
    private final List<Log> logs = new java.util.ArrayList<>();

    public int getAdminId() {
        return adminId;
    }

    public void setAdminId(int adminId) {
        this.adminId = adminId;
    }

    public int getAccountId() {
        return accountId;
    }

    public void setAccountId(int accountId) {
        this.accountId = accountId;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public Date getDateSet() {
        return dateSet;
    }

    public void setDateSet(Date dateSet) {
        this.dateSet = dateSet;
    }

    public void addLogs(Log log) {
        log.setAdminId(this);
        logs.add(log);
    }

    public List<Log> getLogs() {
        return logs;
    }
}
