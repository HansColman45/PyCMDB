package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;

/**
 * Represents an application in the CMDB system.
 */
@Entity
public class Application extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int appId;
    private String name;
    @OneToMany(mappedBy = "application")
    private final List<Log> logs = new java.util.ArrayList<>();

    /**
     * Gets the application ID.
     * @return the application ID
     */
    public int getAppId() {
        return appId;
    }

    /**
     * Sets the application ID.
     * @param appId the application ID to set
     */
    public void setAppId(int appId) {
        this.appId = appId;
    }

    /**
     * Gets the name of the application.
     *
     * @return the application name
     */
    public String getName() {
        return name;
    }

    /**
     * Sets the name of the application.
     *
     * @param name the application name to set
     */
    public void setName(String name) {
        this.name = name;
    }

    /**
     * Gets the list of logs associated with this application.
     *
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Adds a log to the list of logs associated with this application.
     * @param log the log to add
     */
    public void addLogs(Log log) {
        log.setApplicationId(this);
        this.logs.add(log);
    }
}
