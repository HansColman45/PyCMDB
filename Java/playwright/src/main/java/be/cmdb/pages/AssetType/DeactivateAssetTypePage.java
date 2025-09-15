package be.cmdb.pages.AssetType;

import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class DeactivateAssetTypePage extends MainPage {
    private final Locator deactivateReason;
    private final Locator deleteButton;

    /**
     * Constructor for MainPage.
     * @param page The Playwright Page object
     */
    public DeactivateAssetTypePage(Page page) {
        super(page);
        this.deactivateReason = page.locator("//input[@id='reason']");
        this.deleteButton = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Delete"));
    }

    /**
     * Sets the reason for deactivation.
     * @param reason the reason to set
     */
    public void setReason(String reason){
        this.deactivateReason.fill(reason);
    }

    /**
     * Clicks the delete button to deactivate the identity.
     */
    public void deActivate() {
        this.deleteButton.click();
    }
}
