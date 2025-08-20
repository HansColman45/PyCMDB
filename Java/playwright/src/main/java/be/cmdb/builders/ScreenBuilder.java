package be.cmdb.builders;

import be.cmdb.model.Device;

public class ScreenBuilder extends GenericBuilder<Device> {
    private final Device screen;

    public ScreenBuilder() {
        this.screen = new Device();
        screen.setDiscriminator("screen");
        screen.setActive(1); // Default to active
        screen.setSerialNumber(getFaker().address().streetAddressNumber());
        screen.setAssetTag("SCR" + getFaker().address().zipCode());
    }

    public ScreenBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        screen.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public ScreenBuilder withAssetCategoryId(int categoryId) {
        screen.setCategoryId(categoryId);
        return this;
    }

    public ScreenBuilder withTypeId(int typeId) {
        screen.setTypeId(typeId);
        return this;
    }

    public ScreenBuilder withActive(int active) {
        screen.setActive(active);
        return this;
    }

    @Override
    public Device build() {
        return this.screen;
    }
}
