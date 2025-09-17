package be.cmdb.pages.Account;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class CreateAccountPage extends CMDBPage {
    private final Locator userIdInput;
    private final Locator typeSelect;
    private final Locator applicationSelect;
    private final Locator createButton;
    /**
     * The constructor for the CMDBPage class.
     * @param page the Playwright Page object
     */
    public CreateAccountPage(Page page) {
        super(page);
        userIdInput = page.locator("xpath=//input[@id='UserID']");
        typeSelect = page.locator("xpath=//select[@id='Type']");
        applicationSelect = page.locator("xpath=//select[@id='Application']");
        createButton = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Create"));
    }

    /**
     * Updates the userId input field with the given userId.
     * @param userId the userId to fill in
     */
    public void setUserId(String userId) {
        userIdInput.fill(userId);
    }

    /**
     * Selects the given type from the type dropdown.
     * @param type the type to select
     */
    public void selectType(String type) {
        typeSelect.selectOption(type);
    }

    /**
     * Selects the given application from the application dropdown.
     * @param application the application to select
     */
    public void selectApplication(String application) {
        applicationSelect.selectOption(application);
    }

    /**
     * Clicks the create button to submit the form.
     */
    public void clickCreateButton() {
        createButton.click();
    }
}
