package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;

@Entity
public class AssetType {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int typeId;
    @OneToMany(mappedBy = "assetTypeId")
    private final List<Log> logs = new java.util.ArrayList<>();

    public int getTypeId() {
        return typeId;
    }

    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    public List<Log> getLogs() {
        return logs;
    }

    public void addLog(Log log) {
        log.setAssetTypeId(this);
        logs.add(log);
    }
}
