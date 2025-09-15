package be.cmdb.pages.AssetType;

import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;

public class AssetTypeDetailPage extends MainPage {
    private final Locator lastLogline;

    /**
     * Constructor for MainPage.
     * @param page The Playwright Page object
     */
    public AssetTypeDetailPage(Page page) {
        super(page);
        lastLogline = page.locator("//td[contains(text(),'assettype')]").first();
    }

    /**
     * Get the last log line from the detail page.
     * @return The text content of the last log line
     */
    public String getLastLogLine(){
        return lastLogline.textContent();
    }
}
