package be.brightest.ScreenPlay.Abilities;

import com.microsoft.playwright.Page;

/**
 * Ability to open a website using Playwright.
 */
public class OpenAWebSite extends Ability{
    private Page page;

    /**
     * Constructor to initialize the Playwright Page object.
     * @param page the Playwright Page object
     */
    public OpenAWebSite(Page page) {
        this.page = page;
    }

    /**
     * Get the Playwright Page object.
     * @return the Playwright Page object
     */
    public Page getPage() {
        return page;
    }

    /**
     * Set the Playwright Page object.
     * @param page the Playwright Page object
     */
    public void setPage(Page page) {
        this.page = page;
    }

    @Override
    public void close() throws Exception {
        page.close();
    }
}
