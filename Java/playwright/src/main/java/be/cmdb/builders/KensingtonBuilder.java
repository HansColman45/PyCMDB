package be.cmdb.builders;

import be.cmdb.model.Kensington;

/**
 * Builder class for creating and configuring {@link Kensington} objects.
 * Provides fluent methods to set properties of the Kensington.
 */
public class KensingtonBuilder extends GenericBuilder<Kensington> {
    private final Kensington kensington;

    /**
     * Constructs a new {@code KensingtonBuilder} and initializes the {@link Kensington}.
     */
    public KensingtonBuilder() {
        this.kensington = new Kensington();
    }

    /**
     * Sets the lastModifiedAdminId for the {@link Kensington}.
     * @param lastModifiedAdminId The admin ID that last modified the Kensington
     * @return this builder instance
     */
    public KensingtonBuilder withLastModifiedAdminId(int lastModifiedAdminId){
        kensington.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the active state for the {@link Kensington}.
     * @param active The active state to set (1 for active, 0 for inactive)
     * @return this builder instance
     */
    public KensingtonBuilder withActive(int active) {
        kensington.setActive(active);
        return this;
    }

    /**
    * Builds and returns the configured {@link Kensington} object.
    * @return the configured Kensington object
    */
    @Override
    public Kensington build() {
        return this.kensington;
    }
}
