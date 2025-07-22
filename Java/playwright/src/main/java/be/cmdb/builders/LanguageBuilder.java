package be.cmdb.builders;

import be.cmdb.model.Language;

/**
 * Builder class for creating and configuring {@link Language} objects.
 * Provides fluent methods to set properties of the Language.
 */
public class LanguageBuilder {
    private final Language language;

    /**
     * Constructs a new {@code LanguageBuilder} and initializes the {@link Language}.
     */
    public LanguageBuilder() {
        language = new Language();
    }

    /**
     * Sets the code for the {@link Language}.
     * @param code the language code to set
     * @return this builder instance
     */
    public LanguageBuilder withCode(String code) {
        language.setCode(code);
        return this;
    }

    /**
     * Sets the description for the {@link Language}.
     * @param description the language description to set
     * @return this builder instance
     */
    public LanguageBuilder withDescription(String description) {
        language.setDescription(description);
        return this;
    }

    /**
     * Builds and returns the configured {@link Language} object.
     * @return the built {@link Language}
     */
    public Language build() {
        return language;
    }
}
