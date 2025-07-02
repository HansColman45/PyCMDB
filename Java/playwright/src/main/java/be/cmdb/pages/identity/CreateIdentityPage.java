package be.cmdb.pages.identity;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

import be.cmdb.pages.CMDBPage;

/**
 * Represents the Create Identity page of the CMDB application.
 */
public class CreateIdentityPage extends CMDBPage {
    private final Locator firtName;
    private final Locator lastName;
    private final Locator email;
    private final Locator company;
    private final Locator userId;
    private final Locator createButton;
    private final Locator selectType;
    private final Locator languaLocator;

    /**
     * Constructor for CreateIdentityPage.
     * @param page the Playwright Page object
     */
    public CreateIdentityPage(Page page) {
        super(page);
        this.firtName = page.locator("//input[@id='FirstName']");
        this.lastName = page.locator("//input[@id='LastName']");
        this.email = page.locator("//input[@id='EMail']");
        this.company = page.locator("//input[@id='Company']");
        this.userId = page.locator("//input[@id='UserID']");
        this.selectType = page.locator("//select[@id='Type']");
        this.languaLocator = page.locator("//select[@id='Language']");
        this.createButton = page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Create"));
    }

    /**
     * Sets the first name for the identity.
     * @param firstName
     */
    public void setfirstName(String firstName) {
        this.firtName.fill(firstName);
    }

    /**
     * Sets the last name for the identity.
     * @param lastName
     */
    public void setLastName(String lastName) {
        this.lastName.fill(lastName);
    }

    /**
     * Sets the email for the identity.
     * @param email
     */
    public void setEmail(String email) {
        this.email.fill(email);
    }

    /**
     * Sets the company for the identity.
     * @param company
     */
    public void setCompany(String company) {
        this.company.fill(company);
    }

    /**
     * Sets the user ID for the identity.
     * @param userId
     */
    public void setUserId(String userId) {
        this.userId.fill(userId);
    }

    /**
     * Selects the type of identity from a dropdown.
     * @param type the type to select
     */
    public void selectType(String type) {
        this.selectType.selectOption(type);
    }

    /**
     * Selects the language for the identity.
     * @param language
     */
    public void selectLanguage(String language) {
        this.languaLocator.selectOption(language);
    }

    /**
     * Press the create button to submit the identity creation form.
     */
    public void create() {
        this.createButton.click();
    }
}
