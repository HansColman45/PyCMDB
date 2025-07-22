package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;

/**
 * Represents an Account in the CMDB application.
 */
@Entity
public class Account extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int accId;
    private int typeId;
    private int applicationId;
    private String userId;
    @OneToMany(mappedBy = "accountId")
    private final List<Log> logs = new java.util.ArrayList<>();

    public int getAccId() {
        return accId;
    }

    public void setAccId(int accId) {
        this.accId = accId;
    }

    public int getTypeId() {
        return typeId;
    }

    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    public int getApplicationId() {
        return applicationId;
    }

    public void setApplicationId(int applicationId) {
        this.applicationId = applicationId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public void addLogs(Log log){
        log.setAccountId(this);
        logs.add(log);
    }

    public List<Log> getLogs() {
        return logs;
    }
}
