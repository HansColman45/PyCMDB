package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;

/**
 * Represents a general type in the CMDB system.
 */
@Entity(name = "Type")
public class Type extends CMDBModel {
    @Column(name = "TypeId")
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int typeId;

    @Column(name = "Type", nullable = false)
    private String type;

    @Column(name = "Description", nullable = false)
    private String description;

    @Column(name = "Discriminator", nullable = false)
    private String discriminator;

    @OneToMany(mappedBy = "type")
    private List<Log> logs;

    @OneToMany(mappedBy = "type")
    private List<Identity> identities;

    @OneToMany(mappedBy = "type")
    private List<Account> accounts;
    /**
     * Gets the type ID.
     * @return the type ID
     */
    public int getTypeId() {
        return typeId;
    }

    /**
     * Sets the type ID.
     * @param typeId the type ID to set
     */
    public void setTypeId(int typeId) {
        this.typeId = typeId;
    }

    /**
     * Gets the type name.
     * @return the type name
     */
    public String getType() {
        return type;
    }

    /**
     * Sets the type name.
     * @param type the type name to set
     */
    public void setType(String type) {
        this.type = type;
    }

    /**
     * Gets the description of the type.
     * @return the description
     */
    public String getDescription() {
        return description;
    }

    /**
     * Sets the description of the type.
     * @param description the description to set
     */
    public void setDescription(String description) {
        this.description = description;
    }

    /**
     * Gets the discriminator value.
     * @return the discriminator
     */
    public String getDiscriminator() {
        return discriminator;
    }

    /**
     * Sets the discriminator value.
     * @param discriminator the discriminator to set
     */
    public void setDiscriminator(String discriminator) {
        this.discriminator = discriminator;
    }

    /**
     * Gets the list of logs associated with this type.
     * @return the list of logs
     */
    public List<Log> getLogs() {
        return logs;
    }

    /**
     * Sets the list of logs associated with this type.
     * @param logs the list of logs to set
     */
    public void setLogs(List<Log> logs) {
        this.logs = logs;
    }

    /**
     * Gets the list of Identities associated with this type.
     * @return the list of identities
     */
    public List<Identity> getIdentities(){
        return identities;
    }

    /**
     * Sets the list of Identities associated with this type.
     * @param identities the list of identities to set
     */
    public void setIdentities(List<Identity> identities){
        this.identities = identities;
    }

    /**
     * Gets the list of Accounts associated with this type.
     * @return the list of accounts
     */
    public List<Account> getAccounts(){
        return accounts;
    }

    /**
     * Sets the list of Accounts associated with this type.
     * @param accounts
     */
    public void setAccounts(List<Account> accounts){
        this.accounts = accounts;
    }
}
