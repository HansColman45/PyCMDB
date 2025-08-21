package be.cmdb.pages.Type;

import be.cmdb.pages.CMDBPage;
import com.microsoft.playwright.Page;

public class TypeDetailPage extends CMDBPage {
    /**
     * The constructor for the CMDBPage class.
     *
     * @param page the Playwright Page object
     */
    public TypeDetailPage(Page page) {
        super(page);
    }

    /**
     * Retrieves the last logline from the Identity Details page.
     * @return The last logline.
     */
    public String getLastLogline(String type) {
        return getPage().locator("//td[contains(text(),'" + type + "')]").first().textContent();
    }
}
