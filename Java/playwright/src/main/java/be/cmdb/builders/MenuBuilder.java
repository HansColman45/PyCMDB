package be.cmdb.builders;

import be.cmdb.model.Menu;

/**
 * Builder class for creating and configuring {@link Menu} objects.
 * Provides fluent methods to set properties of the Menu.
 */
public class MenuBuilder extends GenericBuilder<Menu> {
    private final Menu menu;

    /**
     * Constructs a new {@code MenuBuilder} and initializes the {@link Menu}.
     */
    public MenuBuilder() {
        menu = new Menu();
    }

    /**
     * Return the built Menu instance.
     * @return the Menu instance
     */
    @Override
    public Menu build() {
        return menu;
    }
}
