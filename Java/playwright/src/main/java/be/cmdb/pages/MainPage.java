package be.cmdb.pages;

import be.cmdb.pages.identity.IdentityOverviewPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * Represents the main page of the CMDB application.
 */
public class MainPage extends CMDBPage {
    private final Locator identityMenu;
    private final Locator identitySubMenu;
    private final Locator identityOverview;

    /**
     * Constructor for MainPage.
     * @param page The Playwright Page object
     */
    public MainPage(Page page) {
        super(page);
        this.identityMenu = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Identity"));
        this.identitySubMenu = page.locator("#Identity2");
        this.identityOverview = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview"));
    }

    /**
     * Opens the Identity Overview page.
     * @return IdentityOverviewPage
     */
    public IdentityOverviewPage openIdentityOverview() {
        this.identityMenu.click();
        this.identitySubMenu.click();
        this.identityOverview.click();
        this.newButton().waitFor();
        return new IdentityOverviewPage(getPage());
    }
}
