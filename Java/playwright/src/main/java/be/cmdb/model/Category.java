package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;

@Entity(name = "category")
public class Category extends CMDBModel{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @OneToMany(mappedBy = "assetCategoryId")
    private final List<Log> logs = new java.util.ArrayList<>();

    public int getId() {
        return id;
    }

    public void setId(int Id) {
        this.id = Id;
    }

    public List<Log> getLogs() {
        return logs;
    }

    public void addLog(Log log) {
        log.setAssetCategoryId(this);
        logs.add(log);
    }
}
