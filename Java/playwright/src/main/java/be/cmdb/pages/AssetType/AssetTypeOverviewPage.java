package be.cmdb.pages.AssetType;

import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Page;

public class AssetTypeOverviewPage extends MainPage {
    /**
     * Constructor for MainPage.
     * @param page The Playwright Page object
     */
    public AssetTypeOverviewPage(Page page) {
        super(page);
    }

    /**
     * Opens the AssetType Detail page.
     * @return AssetTypeDetailPage
     */
    public AssetTypeDetailPage openDetailPage() {
        this.detailsButton().click();
        return new AssetTypeDetailPage(getPage());
    }

    /**
     * Opens the Create AssetType page.
     * @return CreateAssetTypePage
     */
    public CreateAssetTypePage openCreateAssetTypePage() {
        this.newButton().click();
        return new CreateAssetTypePage(getPage());
    }

    /**
     * Opens the Edit AssetType page.
     * @return EditAssetTypePage
     */
    public EditAssetTypePage openEditAssetTypePage() {
        this.editButton().click();
        return new EditAssetTypePage(getPage());
    }

    /**
     * Opens the Deactivate AssetType page.
     * @return DeactivateAssetTypePage
     */
    public DeactivateAssetTypePage openDeactivateAssetTypePage() {
        this.deleteButton().click();
        return new DeactivateAssetTypePage(getPage());
    }

    /**
     * Activates the AssetType and waits for the New button to be visible.
     */
    public void activate(){
        this.activateButton().click();
        this.newButton().waitFor();
    }
}
