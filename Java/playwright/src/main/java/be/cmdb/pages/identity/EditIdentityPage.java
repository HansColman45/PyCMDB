package be.cmdb.pages.Identity;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * Represents the Edit Identity page of the CMDB application.
 */
public class EditIdentityPage extends CMDBPage {
    private final Locator firstName;
    private final Locator lastName;
    private final Locator email;
    private final Locator company;
    private final Locator userId;
    private final Locator updateButton;
    private final Locator selectType;
    private final Locator languageLocator;

    /**
     * Constructor for EditIdentityPage.
     * @param page the Playwright Page object
     */
    public EditIdentityPage(Page page) {
        super(page);
        this.firstName = page.locator("//input[@id='FirstName']");
        this.lastName = page.locator("//input[@id='LastName']");
        this.email = page.locator("//input[@id='EMail']");
        this.company = page.locator("//input[@id='Company']");
        this.userId = page.locator("//input[@id='UserID']");
        this.selectType = page.locator("//select[@id='Type']");
        this.languageLocator = page.locator("//select[@id='Language']");
        this.updateButton = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Update"));
    }

    /**
     * Sets the first name in the FirstName input field.
     * @param firstName the first name to set
     */
    public void setFirstName(String firstName) {
        this.firstName.fill(firstName);
    }

    /**
     * Sets the last name in the LastName input field.
     * @param lastName the last name to set
     */
    public void setLastName(String lastName) {
        this.lastName.fill(lastName);
    }

    /**
     * Sets the email in the EMail input field.
     * @param email the email to set
     */
    public void setEmail(String email) {
        this.email.fill(email);
    }

    /**
     * Sets the company in the Company input field.
     * @param company the company to set
     */
    public void setCompany(String company) {
        this.company.fill(company);
    }

    /**
     * Sets the user ID in the UserID input field.
     * @param userId the user ID to set
     */
    public void setUserId(String userId) {
        this.userId.fill(userId);
    }

    /**
     * Selects the type from the Type dropdown.
     * @param type the type to select
     */
    public void selectType(String type) {
        this.selectType.selectOption(type);
    }

    /**
     * Selects the language from the Language dropdown.
     * @param language the language to select
     */
    public void selectLanguage(String language) {
        this.languageLocator.selectOption(language);
    }

    /**
     * Clicks the Update button to submit the form.
     */
    public void update() {
        this.updateButton.click();
    }

}
