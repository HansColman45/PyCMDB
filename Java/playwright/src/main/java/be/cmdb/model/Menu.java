package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;
import jakarta.validation.constraints.NotNull;

@Entity
public class Menu {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int menuId;
    @NotNull
    private String label;
    private String URL;
    private int parentId;
    @OneToMany
    @JoinColumn(name = "menuId", nullable = true)
    private List<Log> logs;


    /**
     * Gets the menu ID.
     * @return the menu ID
     */
    public int getMenuId() {
        return menuId;
    }

    /**
     * Sets the menu ID.
     * @param menuId the menu ID to set
     */
    public void setMenuId(int menuId) {
        this.menuId = menuId;
    }

    /**
     * Gets the list of logs associated with this menu.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this menu.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }

    /**
     * Gets the label of the menu.
     * @return the label
     */
    public String getLabel() {
        return label;
    }

    /**
     * Sets the label of the menu.
     * @param label the label to set
     */
    public void setLabel(String label) {
        this.label = label;
    }

    /**
     * Gets the URL of the menu.
     * @return the URL
     */
    public String getURL() {
        return URL;
    }

    /**
     * Sets the URL of the menu.
     * @param URL the URL to set
     */
    public void setURL(String URL) {
        this.URL = URL;
    }

    /**
     * Gets the parent ID of the menu.
     * @return the parent ID
     */
    public int getParentId() {
        return parentId;
    }

    /**
     * Sets the parent ID of the menu.
     * @param parentId the parent ID to set
     */
    public void setParentId(int parentId) {
        this.parentId = parentId;
    }
}
