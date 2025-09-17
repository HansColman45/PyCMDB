package be.cmdb.pages;

import be.cmdb.pages.Account.AccountOverviewPage;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.pages.Type.TypeOverviewPage;
import be.cmdb.pages.Identity.IdentityOverviewPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * Represents the main page of the CMDB application.
 */
public class MainPage extends CMDBPage {

    /**
     * Constructor for MainPage.
     * @param page The Playwright Page object
     */
    public MainPage(Page page) {
        super(page);
    }

    /**
     * Opens the AccountType Overview page.
     * @return TypeOverviewPage
     */
    public TypeOverviewPage openAccountTypeOverview(){
        getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Types")).click();
        getPage().locator("xpath=//a[@id='Account Type34']").click();
        getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        this.newButton().waitFor();
        return new TypeOverviewPage(getPage());
    }

    /**
     * Opens the IdentityType Overview page.
     * @return TypeOverviewPage
     */
    public TypeOverviewPage openIdentityTypeOverviewPage(){
        getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Types")).click();
        getPage().locator("xpath=//a[@id='Identity Type32']").click();
        getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        this.newButton().waitFor();
        return new TypeOverviewPage(getPage());
    }

    /**
     * Opens the RoleType Overview page.
     * @return TypeOverviewPage
     */
    public TypeOverviewPage openRoleTypeOverviewPage() {
        getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Types")).click();
        getPage().locator("#Role Type36").click();
        getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        this.newButton().waitFor();
        return new TypeOverviewPage(getPage());
    }

    /**
     * Opens the Account Overview page.
     * @return AccountOverviewPage
     */
    public AccountOverviewPage openAccountOverview(){
        getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Account")).click();
        getPage().locator("xpath://a[@id='Account5']").click();
        getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        this.newButton().waitFor();
        return new AccountOverviewPage(getPage());
    }

    /**
     * Opens the AssetType Overview page.
     * @return AssetTypeOverviewPage
     */
    public AssetTypeOverviewPage openAssetTypeOverviewPage(){
        getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Type")).click();
        getPage().locator("xpath=//a[@id='Asset Type28']").click();
        getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        this.newButton().waitFor();
        return new AssetTypeOverviewPage(getPage());
    }
}
