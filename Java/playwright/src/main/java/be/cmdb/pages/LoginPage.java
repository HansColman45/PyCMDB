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
     * @param username the username to log in with
     * @param password the password to log in with
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
     * Sets the user ID in the login form.
     * @param userId the user ID to set
     */
    public void setUserId(String userId) {
        getPage().locator("xpath=//input[@type='text']").fill(userId);
    }

    /**
     * Sets the password in the login form.
     * @param password the password to set
     */
    public void setPassword(String password) {
        getPage().locator("xpath=//input[@type='password']").fill(password);
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
