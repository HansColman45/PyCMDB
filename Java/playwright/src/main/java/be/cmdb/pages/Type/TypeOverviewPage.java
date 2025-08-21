package be.cmdb.pages.Type;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Page;

public class TypeOverviewPage extends CMDBPage {

    /**
     * The constructor for the CMDBPage class.
     * @param page the Playwright Page object
     */
    public TypeOverviewPage(Page page) {
        super(page);
    }

    /**
     * Activates the selected Type.
     */
    public void activate(){
        this.activateButton().click();
    }

    public CreateTypePage openCreateTypePage() {
        this.newButton().click();
        return new CreateTypePage(getPage());
    }

    public TypeDetailPage openTypeDetailPage() {
        this.detailsButton().click();
        return new TypeDetailPage(getPage());
    }
}
