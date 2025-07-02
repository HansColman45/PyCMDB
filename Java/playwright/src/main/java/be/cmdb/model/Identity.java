package be.cmdb.model;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
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
    private int idenId;
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
    private int typeId;
    @NotNull
    private String languageCode;

    /**
     * Gets the identifier for the identity.
     * @return the identifier for the identity
     */
    public int getIdenId() {
        return this.idenId;
    }

    /**
     * Sets the identifier for the identity.
     * @param idenId
     */
    public void setIdenId(int idenId) {
        this.idenId = idenId;
    }

    /**
     * Gets the first name of the identity.
     * @return
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
     * @param firstName
     */
    public void setFirstName(String firstName) {
        this.firstName = firstName;
        this.name = this.firstName + ", " + this.lastName;
    }

    /**
     * Gets the last name of the identity.
     * @return
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
     * @param lastName
     */
    public void setLastName(String lastName) {
        this.lastName = lastName;
        this.name = this.firstName + ", " + this.lastName;
    }

    /**
     * Gets the email address of the identity.
     * @return
     */
    public String getEmail() {
        return email;
    }

    /**
     * Sets the email address of the identity.
     * @param email
     */
    public void setEmail(String email) {
        this.email = email;
    }

    /**
     * Gets the company name associated with the identity.
     * @return
     */
    public String getCompany() {
        return company;
    }

    /**
     * Sets the company name associated with the identity.
     * @param company
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
     * @param userId
     */
    public void setUserId(String userId) {
        this.userId = userId;
    }

    /**
     * Gets the name of the identity.
     * @return
     */
    public String getName() {
        return name;
    }

    /**
     * Sets the name of the identity.
     * @param name
     */
    public void setName(String name) {
        this.name = name;
    }

    /**
     * Gets the type ID of the identity.
     * @return The type ID of the identity
     */
    public int getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID of the identity.
     * @param typeId
     */
    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    /**
     * Gets the language code of the identity.
     * @return
     */
    public String getLanguageCode() {
        return languageCode;
    }

    /**
     * Sets the language code of the identity.
     * @param languageCode
     */
    public void setLanguageCode(String languageCode) {
        this.languageCode = languageCode;
    }
}
