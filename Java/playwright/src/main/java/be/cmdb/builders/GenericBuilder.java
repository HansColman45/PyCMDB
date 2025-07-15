package be.cmdb.builders;

import com.github.javafaker.Faker;

public class GenericBuilder {
    private final Faker faker;

    public GenericBuilder() {
        this.faker = new Faker();
    }

    public Faker getFaker() {
        return faker;
    }
}
