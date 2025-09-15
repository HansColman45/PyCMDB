package be.cmdb.pages;

import be.brightest.ScreenPlay.Abilities.OpenAWebSite;
import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * The Main Page object.
 */
public class CMDBPage extends OpenAWebSite {
    private final Locator newButton;
    private final Locator editButton;
    private final Locator deleteButton;
    private final Locator detailsButton;
    private final Locator searchInput;
    private final Locator activateButton;

    /**
     * The constructor for the CMDBPage class.
     * @param page the Playwright Page object
     */
    public CMDBPage(Page page) {
        super(page);
        newButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Add"));
        editButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Edit"));
        detailsButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Info"));
        deleteButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Deactivate"));
        searchInput = page.getByRole(AriaRole.TEXTBOX, new Page.GetByRoleOptions().setName("Search"));
        activateButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Activate"));
    }

    /**
     * This method navigates to the main page of the CMDB application.
     */
    public void navigate() {
        getPage().navigate("http://localhost:44313/");
    }

    /**
     * Global search method to search for a string in the CMDB application.
     * @param searchString the string to search for
     */
    public void search(String searchString) {
        searchInput.fill(searchString);
        searchInput.press("Enter");
    }

    /**
     * Gets the Locator for the new button.
     * @return Locator for the new button
     */
    public Locator newButton() {
        return newButton;
    }

    /**
     * Gets the Locator for the edit button.
     * @return Locator for the edit button
     */
    public Locator editButton() {
        return editButton;
    }

    /**
     * Gets the Locator for the delete button.
     * @return Locator for the delete button
     */
    public Locator deleteButton() {
        return deleteButton;
    }

    /**
     * Gets the Locator for the details button.
     * @return Locator for the details button
     */
    public Locator detailsButton() {
        return detailsButton;
    }

    /**
     * Gets the Locator for the activate button.
     * @return Locator for the activate button
     */
    public Locator activateButton(){
        return activateButton;
    }
}
