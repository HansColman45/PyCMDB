package be.cmdb.builders;

import be.cmdb.model.Mobile;

/**
 * The {@code MobileBuilder} class is a builder for creating and configuring {@link Mobile} objects.
 */
public class MobileBuilder extends GenericBuilder<Mobile> {
    private final Mobile mobile;

    /**
     * Constructs a new {@code MobileBuilder} and initializes the {@link Mobile}.
     */
    public MobileBuilder() {
        this.mobile = new Mobile();
    }

    /**
     * Sets the last modified admin ID for the {@link Mobile}.
     * @param adminId The admin ID that last modified the Mobile
     * @return this builder instance
     */
    public MobileBuilder withLastModifiedAdminId(int adminId) {
        this.mobile.setLastModifiedAdminId(adminId);
        return this;
    }

    /**
     * Sets the active state for the {@link Mobile}.
     * @param active The active state to set (1 for active, 0 for inactive)
     * @return this builder instance
     */
    public MobileBuilder withActive(int active) {
        this.mobile.setActive(active);
        return this;
    }

    /**
     * Builds and returns the configured {@link Mobile} object.
     * @return the configured Mobile object
     */
    @Override
    public Mobile build() {
        return this.mobile;
    }
}
