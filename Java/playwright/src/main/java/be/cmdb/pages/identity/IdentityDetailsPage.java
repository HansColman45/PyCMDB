package be.cmdb.pages.identity;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;

import be.cmdb.pages.CMDBPage;

public class IdentityDetailsPage extends CMDBPage {
    private final Locator lastLogline;

    public IdentityDetailsPage(Page page) {
        super(page);
        lastLogline = page.locator("//td[contains(text(),'identity')]");
    }

    public String getLastLogline() {
        return lastLogline.textContent();
    }

}
