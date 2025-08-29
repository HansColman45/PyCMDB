package be.cmdb.builders;

import be.cmdb.model.Type;

public class AccountTypeBuilder extends GenericBuilder<Type> {
    private final Type type;

    public AccountTypeBuilder(){
        this.type = new Type();
        type.setDiscriminator("AccountType");
        type.setActive(1); // Default to active
        type.setType(getFaker().commerce().productName());
        type.setDescription(getFaker().lorem().sentence());
    }

    public  AccountTypeBuilder withLastModifiedAdminId(int lastModifiedAdminId){
        type.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public AccountTypeBuilder withActive(int active) {
        type.setActive(active);
        return this;
    }

    @Override
    public Type build() {
        return type;
    }
}
