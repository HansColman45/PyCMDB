package be.cmdb.builders;

import be.cmdb.model.Menu;

public class MenuBuilder extends GenericBuilder<Menu> {
    private final Menu menu;

    public MenuBuilder() {
        menu = new Menu();
    }

    @Override
    public Menu build() {
        return menu;
    }
}
