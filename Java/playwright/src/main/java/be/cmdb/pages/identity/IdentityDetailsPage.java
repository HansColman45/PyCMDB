package be.cmdb.pages.identity;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;

/**
 * Represents the Identity Details page of the CMDB application.
 */
public class IdentityDetailsPage extends CMDBPage {
    private final Locator lastLogline;

    /**
     * Constructor for IdentityDetailsPage.
     * @param page the Playwright Page object
     */
    public IdentityDetailsPage(Page page) {
        super(page);
        lastLogline = page.locator("//td[contains(text(),'identity')]").first();
    }

    /**
     * Retrieves the last logline from the Identity Details page.
     * @return The last logline.
     */
    public String getLastLogline() {
        return lastLogline.textContent();
    }
}
