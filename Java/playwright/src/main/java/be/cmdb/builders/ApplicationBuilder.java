package be.cmdb.builders;

import be.cmdb.model.Application;

/**
 * Builder class for creating and configuring {@link Application} objects.
 * Provides fluent methods to set properties of the Application.
 */
public class ApplicationBuilder extends GenericBuilder<Application> {
    private final Application application;

    /**
     * Constructs a new {@code ApplicationBuilder} and initializes the {@link Application}.
     */
    public ApplicationBuilder() {
        application = new Application();
    }

    /**
     * Sets the application ID for the {@link Application}.
     * @param appId the application ID to set
     * @return this builder instance
     */
    public ApplicationBuilder withAppId(int appId) {
        application.setAppId(appId);
        return this;
    }

    /**
     * Sets the name for the {@link Application}.
     * @param name the application name to set
     * @return this builder instance
     */
    public ApplicationBuilder withName(String name) {
        application.setName(name);
        return this;
    }

    /**
     * Sets the last modified admin ID for the {@link Application}.
     * @param lastModifiedAdminId the ID of the admin who last modified the application
     * @return this builder instance
     */
    public ApplicationBuilder withLastModifiedAdminId(int lastModifiedAdminId){
        application.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Builds and returns the configured {@link Application} object.
     * @return the built {@link Application}
     */
    public Application build() {
        return application;
    }
}
