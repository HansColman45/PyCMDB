package be.cmdb.builders;

import be.cmdb.model.Type;

public class IdentityTypeBuilder extends GenericBuilder<Type> {
    private final Type type;

    public IdentityTypeBuilder() {
        this.type = new Type();
        type.setDiscriminator("identitytype");
        type.setActive(1); // Default to active
        type.setType(getFaker().commerce().productName());
        type.setDescription(getFaker().lorem().sentence());
    }

    public IdentityTypeBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        type.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public IdentityTypeBuilder withActive(int active) {
        type.setActive(active);
        return this;
    }

    @Override
    public Type build() {
        return type;
    }
}
