package be.cmdb.builders;

import be.cmdb.model.Mobile;

public class MobileBuilder extends GenericBuilder<Mobile> {
    private Mobile mobile;

    public MobileBuilder() {
        this.mobile = new Mobile();
    }

    public MobileBuilder withLastModifiedAdminId(int adminId) {
        this.mobile.setLastModifiedAdminId(adminId);
        return this;
    }

    public MobileBuilder withActive(int active) {
        this.mobile.setActive(active);
        return this;
    }

    @Override
    public Mobile build() {
        return this.mobile;
    }
}
