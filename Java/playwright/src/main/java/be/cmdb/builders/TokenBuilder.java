package be.cmdb.builders;

import be.cmdb.model.Device;

public class TokenBuilder extends GenericBuilder<Device> {
    private final Device token;

    public  TokenBuilder() {
        this.token = new Device();
        token.setDiscriminator("token");
        token.setActive(1); // Default to active
        token.setSerialNumber(getFaker().address().streetAddressNumber());
        token.setAssetTag("TOK" + getFaker().address().zipCode());
    }

    public TokenBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        token.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public TokenBuilder withAssetCategoryId(int categoryId) {
        token.setCategoryId(categoryId);
        return this;
    }

    public TokenBuilder withTypeId(int typeId) {
        token.setTypeId(typeId);
        return this;
    }

    public TokenBuilder withActive(int active) {
        token.setActive(active);
        return this;
    }

    @Override
    public Device build() {
        return token;
    }
}
