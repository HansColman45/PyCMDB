package be.cmdb.pages.AssetType;

import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class CreateAssetTypePage extends MainPage {
    private final Locator categorySelector;
    private final Locator vendorInput;
    private final Locator TypeInput;

    /**
     * Constructor for MainPage.
     * @param page The Playwright Page object
     */
    public CreateAssetTypePage(Page page) {
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
     * Fills in the vendor input field.
     * @param vendor The vendor to fill in
     */
    public void setVendor(String vendor) {
        vendorInput.fill(vendor);
    }

    /**
     * Fills in the type input field.
     * @param type The type to fill in
     */
    public void setType(String type) {
        TypeInput.fill(type);
    }

    /**
     * Clicks the create button to create the asset type.
     */
    public void create(){
        getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Create")).click();
    }
}
