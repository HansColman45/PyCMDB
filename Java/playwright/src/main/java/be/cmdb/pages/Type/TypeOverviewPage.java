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

    /**
     * Opens the create new Type page.
     * @return a new instance of the CreateTypePage
     */
    public CreateTypePage openCreateTypePage() {
        this.newButton().click();
        return new CreateTypePage(getPage());
    }

    /**
     * Opens the detail page for the selected Type.
     * @return a new instance of the TypeDetailPage
     */
    public TypeDetailPage openTypeDetailPage() {
        this.detailsButton().click();
        return new TypeDetailPage(getPage());
    }

    /**
     * Opens the delete page for the selected Type.
     * @return a new instance of the DeleteTypePage
     */
    public DeleteTypePage openDeleteTypePage(){
        this.deleteButton().click();
        return new DeleteTypePage(getPage());
    }

    /**
     * Opens the edit page for the selected Type.
     * @return a new instance of the EditTypePage
     */
    public EditTypePage openEditTypePage(){
        this.editButton().click();
        return new EditTypePage(getPage());
    }
}
