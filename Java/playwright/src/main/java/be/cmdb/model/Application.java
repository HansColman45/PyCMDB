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
    @OneToMany(mappedBy = "applicationId")
    private final List<Log> logs = new java.util.ArrayList<>();

    public int getAppId() {
        return appId;
    }

    public void setAppId(int appId) {
        this.appId = appId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public List<Log> getLogs() {
        return logs;
    }

    public void addLogs(Log log) {
        log.setApplicationId(this);
        this.logs.add(log);
    }
}
