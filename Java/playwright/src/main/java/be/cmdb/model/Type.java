package be.cmdb.model;

import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.validation.constraints.NotNull;

/**
 * Represents a general type in the CMDB system.
 */
@Entity(name = "Type")
public class Type extends CMDBModel {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private int typeId;
    @NotNull
    private String discriminator;
    private String type;
    private String description;

    @OneToMany(mappedBy = "type")
    private List<Log> logs;

    @OneToMany(mappedBy = "parentType")
    private List<Type> childTypes;

    @ManyToOne
    @JoinColumn(name = "parentTypeId")
    private Type parentType;
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
     * Gets the list of child types associated with this type.
     * @return the list of child types
     */
    public List<Type> getChildTypes() {
        return childTypes;
    }

    /**
     * Sets the list of child types associated with this type.
     * @param childTypes the list of child types to set
     */
    public void setChildTypes(List<Type> childTypes) {
        this.childTypes = childTypes;
    }

    /**
     * Gets the parent type of this type.
     * @return the parent type
     */
    public Type getParentType() {
        return parentType;
    }

    /**
     * Sets the parent type of this type.
     * @param parentType the parent type to set
     */
    public void setParentType(Type parentType) {
        this.parentType = parentType;
    }
}
