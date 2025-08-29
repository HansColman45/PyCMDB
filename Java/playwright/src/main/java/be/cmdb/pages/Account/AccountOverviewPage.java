package be.cmdb.pages.Account;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Page;

public class AccountOverviewPage extends CMDBPage {

    /**
     * The constructor for the CMDBPage class.
     * @param page the Playwright Page object
     */
    public AccountOverviewPage(Page page) {
        super(page);
    }

    public AccountDetailsPage openAccountDetails() {
        this.detailsButton().click();
        return new AccountDetailsPage(getPage());
    }

    public CreateAccountPage openCreateAccount() {
        this.editButton().click();
        return new CreateAccountPage(getPage());
    }

    public EditAccountPage openEditAccount() {
        this.editButton().click();
        return new EditAccountPage(getPage());
    }


}
