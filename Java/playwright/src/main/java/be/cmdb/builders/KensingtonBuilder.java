package be.cmdb.builders;

import be.cmdb.model.Kensington;

public class KensingtonBuilder extends GenericBuilder<Kensington> {
    private final Kensington kensington;

    public KensingtonBuilder() {
        this.kensington = new Kensington();
    }

    public KensingtonBuilder withLastModifiedAdminId(int lastModifiedAdminId){
        kensington.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public KensingtonBuilder withActive(int active) {
        kensington.setActive(active);
        return this;
    }

    public Kensington build() {
        return this.kensington;
    }
}
