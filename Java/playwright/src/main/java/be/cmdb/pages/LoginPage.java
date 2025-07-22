package be.cmdb.pages;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * Represents the login page of the CMDB application.
 */
public class LoginPage extends CMDBPage {

    /**
     * Constructor for LoginPage.
     * @param page
     */
    public LoginPage(Page page) {
        super(page);
    }

    /**
     * Navigates to the login page.
     * @param username
     * @param password
     * @return MainPage
     */
    public MainPage login(String username, String password) {
        getPage().locator("xpath=//input[@type='text']").fill(username);
        getPage().locator("xpath=//input[@type='password']").fill(password);
        getPage().getByRole(AriaRole.BUTTON).click();
        getPage().waitForLoadState();
        return new MainPage(getPage());
    }

    /**
     * Checks if the user is logged in by looking for a welcome text.
     * @return true if the user is logged in, false otherwise
     */
    public boolean isUserLogedIn() {
        Locator welcomeText = getPage().getByText("Welcome on the Central");
        return welcomeText.isVisible();
    }
}
