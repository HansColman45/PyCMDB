package be.cmdb.pages.identity;

import com.microsoft.playwright.Page;

import be.cmdb.pages.CMDBPage;

/**
 * Represents the Identity Overview page of the CMDB application.
 */
public class IdentiyOverviewPage extends CMDBPage {

    /**
     * Constructor for IdentiyOverviewPage.
     * @param page
     */
    public IdentiyOverviewPage(Page page) {
        super(page);
    }

    /**
     * Opens the Create Identity page.
     * @return CreateIdentityPage
     */
    public CreateIdentityPage openCreateIdentity() {
        this.newButton().click();
        return new CreateIdentityPage(getPage());
    }

    /**
     * Opens the Identity Details page for the selected identity.
     * @return IdentityDetailsPage
     */
    public IdentityDetailsPage openIdentityDetails() {
        this.detailsButton().click();
        return new IdentityDetailsPage(getPage());
    }
}
