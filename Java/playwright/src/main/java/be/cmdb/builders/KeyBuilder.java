package be.cmdb.builders;

import be.cmdb.model.Key;

public class KeyBuilder extends GenericBuilder<Key> {
    private final Key key;

    public KeyBuilder() {
        this.key = new Key();
    }

    public KeyBuilder withLastModifiedAdminId(int lastModifiedAdminId){
        key.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public Key build() {
        return this.key;
    }
}
