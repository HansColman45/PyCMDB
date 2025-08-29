package be.cmdb.pages.Type;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class EditTypePage extends CMDBPage {
    private final Locator typeInput;
    private final Locator descriptionInput;
    private final Locator editButton;

    /**
     * The constructor for the CMDBPage class.
     *
     * @param page the Playwright Page object
     */
    public EditTypePage(Page page) {
        super(page);
        typeInput = page.locator("xpath=//input[@id='Type']");
        descriptionInput = page.locator("xpath=//input[@id='Description']");
        editButton = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Edit"));
    }

    /**
     * Updates the type input field with the given type.
     * @param type the type to set in the input field
     */
    public void setType(String type){
        typeInput.fill(type);
    }

    /**
     * Gets the current value of the type input field.
     * @return the current type value
     */
    public String getType(){
        return typeInput.textContent();
    }

    /**
     * Updates the description input field with the given description.
     * @param description the description to set in the input field
     */
    public void setDescription(String description) {
        descriptionInput.fill(description);
    }

    /***
     * Gets the current value of the description input field.
     * @return the current description value
     */
    public String getDescription(){
        return descriptionInput.textContent();
    }

    /**
     * Clicks the edit button to submit the form.
     */
    public void edit() {
        editButton.click();
    }
}
