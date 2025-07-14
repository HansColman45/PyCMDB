package be.cmdb.model;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.validation.constraints.NotNull;

/**
 * Represents a language entity in the system.
 */
@Entity
public class Language {
    @Id
    @NotNull
    private String code;
    @NotNull
    private String description;

    /**
     * Gets the code of the language.
     * @return the language code
     */
    public String getCode() {
        return code;
    }

    /**
     * Sets the code of the language.
     * @param code the language code to set
     */
    public void setCode(String code) {
        this.code = code;
    }

    /**
     * Gets the description of the language.
     * @return the language description
     */
    public String getDescription() {
        return description;
    }

    /**
     * Sets the description of the language.
     * @param description the language description to set
     */
    public void setDescription(String description) {
        this.description = description;
    }
}
