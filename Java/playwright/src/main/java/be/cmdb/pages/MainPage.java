package be.cmdb.pages;

import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.pages.Type.TypeOverviewPage;
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
