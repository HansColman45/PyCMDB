package be.cmdb.builders;

import be.cmdb.model.Device;

public class DockingBuilder extends GenericBuilder<Device> {
    private final Device dockingStation;

    public DockingBuilder() {
        this.dockingStation = new Device();
        dockingStation.setDiscriminator("docking");
        dockingStation.setActive(1); // Default to active
        dockingStation.setSerialNumber(getFaker().address().streetAddressNumber());
        dockingStation.setAssetTag("DOC" + getFaker().address().zipCode());
    }

    public DockingBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        dockingStation.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public DockingBuilder withAssetCategoryId(int categoryId) {
        dockingStation.setCategoryId(categoryId);
        return this;
    }

    public DockingBuilder withAssetTypeId(int typeId) {
        dockingStation.setTypeId(typeId);
        return this;
    }

    public DockingBuilder withActive(int active) {
        dockingStation.setActive(active);
        return this;
    }

    @Override
    public Device build() {
        return this.dockingStation;
    }
}
