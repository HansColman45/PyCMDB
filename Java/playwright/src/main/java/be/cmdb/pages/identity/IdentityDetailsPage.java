package be.cmdb.pages.identity;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;

import be.cmdb.pages.CMDBPage;

/**
 * Represents the Identity Details page of the CMDB application.
 */
public class IdentityDetailsPage extends CMDBPage {
    private final Locator lastLogline;

    /**
     * Constructor for IdentityDetailsPage.
     * @param page
     */
    public IdentityDetailsPage(Page page) {
        super(page);
        lastLogline = page.locator("//td[contains(text(),'identity')]");
    }

    /**
     * Retrieves the last logline from the Identity Details page.
     * @return
     */
    public String getLastLogline() {
        return lastLogline.textContent();
    }

    /**
     * Opens the Edit Identity page from the Identity Details page.
     * @return EditIdentityPage
     */
	public EditIdentityPage openEditIdentity() {
        this.editButton().click();
        return new EditIdentityPage(getPage());
	}
}
