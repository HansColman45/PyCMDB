package be.cmdb.pages.identity;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

import be.cmdb.pages.CMDBPage;

public class CreateIdentityPage extends CMDBPage {
    private final Locator firtName;
    private final Locator lastName;
    private final Locator email;
    private final Locator company;
    private final Locator userId;
    private final Locator createButton;
    private final Locator selectType;
    private final Locator languaLocator;


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

    public void setfirstName(String firstName) {
       this.firtName.fill(firstName);
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
        this.languaLocator.selectOption(language);
    }

    public void create(){
        this.createButton.click();
    }
}
