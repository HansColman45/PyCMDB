package be.cmdb.builders;

import com.github.javafaker.Faker;

public abstract class GenericBuilder<T> {
    private final Faker faker;

    /**
     * Constructor to create a new GenericBuilder instance.
     * Initializes the Faker instance for generating random data.
     */
    public GenericBuilder() {
        this.faker = new Faker();
    }

    /**
     * Getter for the Faker instance.
     * @return The Faker instance.
     */
    public Faker getFaker() {
        return faker;
    }

    /**
     * Abstract method to build and return the object of type T.
     * Must be implemented by subclasses to provide the actual building logic.
     * @return The built object of type T.
     */
    public abstract T build();
}
