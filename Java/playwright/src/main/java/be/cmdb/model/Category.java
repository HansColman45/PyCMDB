package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.OneToMany;


@Entity()
@Table(name = "category")
public class Category extends CMDBModel{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private String category;
    private String prefix;

    @OneToMany(mappedBy = "assetCategoryId")
    private final List<Log> logs = new java.util.ArrayList<>();

    /**
     * Gets the ID of the category.
     * @return the category ID
     */
    public int getId() {
        return id;
    }

    /**
     * Sets the ID of the category.
     * @param Id the category ID to set
     */
    public void setId(int Id) {
        this.id = Id;
    }

    /**
     * Gets the name of the category.
     * @return the category name
     */
    public String getCategory() {
        return category;
    }

    /**
     * Sets the name of the category.
     * @param category the category name to set
     */
    public void setCategory(String category) {
        this.category = category;
    }

    /**
     * Gets the prefix of the category.
     * @return the category prefix
     */
    public String getPrefix() {
        return prefix;
    }

    /**
     * Sets the prefix of the category.
     * @param prefix the category prefix to set
     */
    public void setPrefix(String prefix) {
        this.prefix = prefix;
    }

    /**
     * Gets the list of logs associated with this category.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Adds a log to the list of logs associated with this category.
     * @param log the log to add
     */
    public void addLog(Log log) {
        log.setAssetCategoryId(this);
        logs.add(log);
    }
}
