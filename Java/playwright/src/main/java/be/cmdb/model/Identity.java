package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;
import jakarta.validation.constraints.NotNull;

/**
 * Represents an identity in the system.
 */
@Entity
@Table(name = "[Identity]")
public class Identity extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer idenId;
    @Transient
    private String firstName;
    @Transient
    private String lastName;
    @NotNull
    private String email;
    @NotNull
    private String company;
    @NotNull
    private String userId;
    @NotNull
    private String name;
    @NotNull
    private Integer typeId;
    @NotNull
    private String languageCode;
    @OneToMany
    @JoinColumn(name = "identityId", nullable = true)
    private List<Log> logs;

    /**
     * Gets the identifier for the identity.
     * @return the identifier for the identity
     */
    public Integer getIdenId() {
        return this.idenId;
    }

    /**
     * Sets the identifier for the identity.
     * @param idenId the identifier for the identity
     */
    public void setIdenId(Integer idenId) {
        this.idenId = idenId;
    }

    /**
     * Gets the first name of the identity.
     * @return the first name of the identity
     */
    public String getFirstName() {
        if ("Stock".equals(this.name)) {
            return this.name;
        } else {
            return this.name.split(",")[0];
        }
    }

    /**
     * Sets the first name of the identity.
     * @param firstName the first name of the identity
     */
    public void setFirstName(String firstName) {
        this.firstName = firstName;
        this.name = this.firstName + ", " + this.lastName;
    }

    /**
     * Gets the last name of the identity.
     * @return the last name of the identity
     */
    public String getLastName() {
        if ("Stock".equals(this.name)) {
            return this.name;
        } else {
            return this.name.split(",")[1].trim();
        }
    }

    /**
     * Sets the last name of the identity.
     * @param lastName the last name of the identity
     */
    public void setLastName(String lastName) {
        this.lastName = lastName;
        this.name = this.firstName + ", " + this.lastName;
    }

    /**
     * Gets the email address of the identity.
     * @return the email address of the identity
     */
    public String getEmail() {
        return email;
    }

    /**
     * Sets the email address of the identity.
     * @param email the email address of the identity
     */
    public void setEmail(String email) {
        this.email = email;
    }

    /**
     * Gets the company name associated with the identity.
     * @return the company name associated with the identity
     */
    public String getCompany() {
        return company;
    }

    /**
     * Sets the company name associated with the identity.
     * @param company the company name associated with the identity
     */
    public void setCompany(String company) {
        this.company = company;
    }

    /**
     * Gets the user ID of the identity.
     * @return the user ID of the identity
     */
    public String getUserId() {
        return userId;
    }

    /**
     * Sets the user ID of the identity.
     * @param userId the user ID of the identity
     */
    public void setUserId(String userId) {
        this.userId = userId;
    }

    /**
     * Gets the name of the identity.
     * @return the name of the identity
     */
    public String getName() {
        return name;
    }

    /**
     * Sets the name of the identity.
     * @param name the name of the identity
     */
    public void setName(String name) {
        this.name = name;
    }

    /**
     * Gets the type ID of the identity.
     * @return The type ID of the identity
     */
    public Integer getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID of the identity.
     * @param typeId the type ID of the identity
     */
    public void setTypeId(Integer typeId) {
        this.typeId = typeId;
    }

    /**
     * Gets the language code of the identity.
     * @return the language code of the identity
     */
    public String getLanguageCode() {
        return languageCode;
    }

    /**
     * Sets the language code of the identity.
     * @param languageCode the language code of the identity
     */
    public void setLanguageCode(String languageCode) {
        this.languageCode = languageCode;
    }

    public List<Log> getLogs() {
        return logs;
    }

    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }
}
