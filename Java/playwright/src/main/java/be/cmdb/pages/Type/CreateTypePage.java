package be.cmdb.pages.Type;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class CreateTypePage extends CMDBPage {
    private final Locator typeInput;
    private final Locator descriptionInput;
    private final Locator createButton;

    /**
     * The constructor for the CMDBPage class.
     * @param page the Playwright Page object
     */
    public CreateTypePage(Page page) {
        super(page);
        typeInput = page.locator("xpath=//input[@id='Type']");
        descriptionInput = page.locator("xpath=//input[@id='Description']");
        createButton = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Create"));
    }

    /**
     * Updates the type input field with the given type.
     * @param type the type to set in the input field
     */
    public void setType(String type){
        typeInput.fill(type);
    }

    /**
     * Updates the description input field with the given description.
     * @param description the description to set in the input field
     */
    public void setDescription(String description) {
        descriptionInput.fill(description);
    }

    /**
     * Clicks the create button to submit the form.
     */
    public void clickCreateButton() {
        createButton.click();
    }
}
