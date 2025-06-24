package be.cmdb.pages;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class LoginPage extends CMDBPage {

    public LoginPage(Page page) {
        super(page);
    }

    public void login(String username, String password) {
        page.locator("xpath=//input[@type='text']").fill(username);
        page.locator("xpath=//input[@type='password']").fill(password);
        page.getByRole(AriaRole.BUTTON).click();
    }

    public boolean isUserLogedIn() {
        Locator welcomeText = page.getByText("Welcome on the Central");
        return welcomeText.isVisible();
    }

}
