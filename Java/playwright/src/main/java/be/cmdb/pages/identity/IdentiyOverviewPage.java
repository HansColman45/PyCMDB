package be.cmdb.pages.identity;
import com.microsoft.playwright.Page;

import be.cmdb.pages.CMDBPage;

public class IdentiyOverviewPage extends CMDBPage {

    public IdentiyOverviewPage(Page page) {
        super(page);
    }
    public CreateIdentityPage openCreateIdentity() {
        this.newButton.click();
        return new CreateIdentityPage(page);
    }
    public IdentityDetailsPage openIdentityDetails() {
        this.detailsButton.click();
        return new IdentityDetailsPage(page);
    }
}
