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

    /**
     * Opens the Account Details page.
     * @return AccountDetailsPage
     */
    public AccountDetailsPage openAccountDetails() {
        this.detailsButton().click();
        return new AccountDetailsPage(getPage());
    }

    /**
     * Opens the Create Account page.
     * @return CreateAccountPage
     */
    public CreateAccountPage openCreateAccount() {
        this.editButton().click();
        return new CreateAccountPage(getPage());
    }

    /**
     * Opens the Edit Account page.
     * @return EditAccountPage
     */
    public EditAccountPage openEditAccount() {
        this.editButton().click();
        return new EditAccountPage(getPage());
    }

    /**
     * Opens the Account Details page.
     * @return AccountDetailsPage
     */
    public AccountDetailsPage openDetailPage() {
        this.detailsButton().click();
        return new AccountDetailsPage(getPage());
    }

}
