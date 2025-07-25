package be.cmdb.builders;

import com.github.javafaker.Faker;

public abstract class GenericBuilder<T> {
    private final Faker faker;

    public GenericBuilder() {
        this.faker = new Faker();
    }

    public Faker getFaker() {
        return faker;
    }

    public abstract T build();
}
