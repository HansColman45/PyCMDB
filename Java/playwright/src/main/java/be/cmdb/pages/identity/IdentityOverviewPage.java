package be.cmdb.pages.Identity;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Page;

/**
 * Represents the Identity Overview page of the CMDB application.
 */
public class IdentityOverviewPage extends CMDBPage {

    /**
     * Constructor for IdentityOverviewPage.
     * @param page the Playwright Page object
     */
    public IdentityOverviewPage(Page page) {
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

    /**
     * Opens the Edit Identity page from the Identity Details page.
     * @return EditIdentityPage
     */
    public EditIdentityPage openEditIdentity() {
        this.editButton().click();
        return new EditIdentityPage(getPage());
    }

    public DeleteIdentityPage openDeleteIdentity() {
        this.deleteButton().click();
        return new DeleteIdentityPage(getPage());
    }

    public void activate(){
        this.activateButton().click();
    }
}
