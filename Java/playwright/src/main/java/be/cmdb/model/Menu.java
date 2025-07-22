package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;

@Entity
public class Menu {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int menuId;
    @OneToMany
    @JoinColumn(name = "menuId", nullable = true)
    private List<Log> logs;
    @OneToMany
    @JoinColumn(name = "parentId", nullable = true)
    private List<Menu> childeren;

    public int getMenuId() {
        return menuId;
    }

    public void setMenuId(int menuId) {
        this.menuId = menuId;
    }

    public List<Log> getLogs() {
        return logs;
    }

    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }

    public List<Menu> getChilderen() {
        return childeren;
    }

    public void setChilderen(List<Menu> childeren) {
        this.childeren = childeren;
    }

}
