package be.cmdb.model;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class RAM {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private int value;
    private String display;

    /**
     * Gets the ID of the RAM.
     * @return the RAM ID
     */
    public int getId() {
        return id;
    }

    /**
     * Sets the ID of the RAM.
     * @param id the RAM ID to set
     */
    public void setId(int id) {
        this.id = id;
    }

    /**
     * Gets the value of the RAM.
     * @return the RAM value
     */
    public int getValue() {
        return value;
    }

    /**
     * Sets the value of the RAM.
     * @param value the RAM value to set
     */
    public void setValue(int value) {
        this.value = value;
    }

    /**
     * Gets the display string for the RAM.
     * @return the display string
     */
    public String getDisplay() {
        return display;
    }

    /**
     * Sets the display string for the RAM.
     * @param display the display string to set
     */
    public void setDisplay(String display) {
        this.display = display;
    }
}
