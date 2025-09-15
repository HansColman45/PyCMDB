package be.cmdb.pages.Account;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;

public class AccountDetailsPage extends CMDBPage {
    private final Locator lastLogline;

    /**
     * The constructor for the CMDBPage class.
     * @param page the Playwright Page object
     */
    public AccountDetailsPage(Page page) {
        super(page);
        lastLogline = page.locator("//td[contains(text(),'account')]").first();
    }

    /**
     * Gets the text content of the last log line.
     * @return the text content of the last log line
     */
    public String getLastLogline() {
        return lastLogline.textContent();
    }
}
