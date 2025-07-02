package be.cmdb.pages;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

import be.cmdb.pages.identity.IdentiyOverviewPage;

/**
 * Represents the main page of the CMDB application.
 */
public class MainPage extends CMDBPage {
    private final Locator identityMenu;
    private final Locator identitySubMenu;
    private final Locator identityOverview;

    /**
     * Constructor for MainPage.
     * @param page
     */
    public MainPage(Page page) {
        super(page);
        this.identityMenu = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Identity"));
        this.identitySubMenu = page.locator("#Identity2");
        this.identityOverview = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview"));
    }

    /**
     * Opens the Identity Overview page.
     * @return IdentiyOverviewPage
     */
    public IdentiyOverviewPage openIdentityOverview() {
        this.identityMenu.click();
        this.identitySubMenu.click();
        this.identityOverview.click();
        this.newButton().waitFor();
        return new IdentiyOverviewPage(getPage());
    }
}
