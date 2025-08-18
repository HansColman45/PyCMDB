package be.cmdb.builders;

import be.cmdb.model.Kensington;

public class KeyBuilder extends GenericBuilder<Kensington> {
    private final Kensington kensington;

    public KeyBuilder() {
        this.kensington = new Kensington();
    }

    public KeyBuilder withLastModifiedAdminId(int lastModifiedAdminId){
        kensington.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public Kensington build() {
        return this.kensington;
    }
}
