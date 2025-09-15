package be.cmdb.pages.AssetType;

import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class EditAssetTypePage extends MainPage {
    private final Locator categorySelector;
    private final Locator vendorInput;
    private final Locator TypeInput;

    /**
     * Constructor for MainPage.
     * @param page The Playwright Page object
     */
    public EditAssetTypePage(Page page) {
        super(page);
        categorySelector = page.locator("//select[@id='AssetCategory']");
        vendorInput = page.locator("//input[@id='Vendor']");
        TypeInput = page.locator("//input[@id='Type']");
    }

    /**
     * Selects a category from the dropdown.
     * @param category The category to select
     */
    public void selectCategory(String category) {
        categorySelector.selectOption(category);
    }

    /**
     * Sets the vendor input field.
     * @param vendor The vendor to set
     */
    public void setVendor(String vendor) {
        vendorInput.fill(vendor);
    }

    /**
     * Sets the type input field.
     * @param type The type to set
     */
    public void setType(String type) {
        TypeInput.fill(type);
    }

    /**
     * Clicks the Edit button to save the asset type.
     */
    public void edit(){
        getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Edit")).click();
    }
}
