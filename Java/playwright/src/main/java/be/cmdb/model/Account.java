package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.CascadeType;

/**
 * Represents an Account in the CMDB application.
 */
@Entity
public class Account extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer accId;
    private Integer applicationId;
    private String userId;

    @JoinColumn(name = "TypeId", referencedColumnName = "TypeId")
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Type type;

    @OneToMany(mappedBy = "accountId")
    private final List<Log> logs = new java.util.ArrayList<>();

    /**
     * Gets the unique identifier of the account.
     * @return the account ID
     */
    public int getAccId() {
        return accId;
    }

    /**
     * Sets the unique identifier of the account.
     * @param accId the account ID to set
     */
    public void setAccId(int accId) {
        this.accId = accId;
    }

    /**
     * Gets the user ID of the account.
     * @return the user ID
     */
    public String getUserId() {
        return userId;
    }

    /**
     * Sets the user ID of the account.
     * @param userId the user ID to set
     */
    public void setUserId(String userId) {
        this.userId = userId;
    }

    /**
     * Gets the Application associated with the account.
     * @return the associated Application
     */
    public Integer getApplicationId() {
        return applicationId;
    }

    /**
     * Sets the Application associated with the account.
     * @param applicationId the Application to set
     */
    public void setApplicationId(Integer applicationId) {
        this.applicationId = applicationId;
    }

    /**
     * Sets the Type associated with the account.
     * @param type the Type to set
     */
    public void setType(Type type) {
        this.type = type;
    }

    /**
     * Gets the Type associated with the account.
     * @return the associated Type
     */
    public Type getType() {
        return type;
    }

    /**
     * Adds a log entry to the account's log list.
     * @param log the Log entry to add
     */
    public void addLogs(Log log){
        log.setAccountId(this);
        logs.add(log);
    }

    /**
     * Gets the list of log entries associated with the account.
     * @return the list of Log entries
     */
    public List<Log> getLogs() {
        return logs;
    }
}
