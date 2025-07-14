package be.cmdb.pages.identity;

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
     * @param page
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

    public void setFirstName(String firstName) {
        this.firstName.fill(firstName);
    }

    public void setLastName(String lastName) {
        this.lastName.fill(lastName);
    }

    public void setEmail(String email) {
        this.email.fill(email);
    }

    public void setCompany(String company) {
        this.company.fill(company);
    }

    public void setUserId(String userId) {
        this.userId.fill(userId);
    }

    public void selectType(String type) {
        this.selectType.selectOption(type);
    }

    public void selectLanguage(String language) {
        this.languageLocator.selectOption(language);
    }

    public void update() {
        this.updateButton.click();
    }

}
