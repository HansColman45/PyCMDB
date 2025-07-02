package be.cmdb.pages;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * The Main Page object.
 */
public class CMDBPage {
    private final Locator newButton;
    private final Locator editButton;
    private final Locator deleteButton;
    private final Locator detailsButton;
    private final Locator searchInput;
    private final Page page;

    /**
     * The constructor for the CMDBPage class.
     * @param page
     */
    public CMDBPage(Page page) {
        this.page = page;
        newButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Add"));
        editButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Edit"));
        detailsButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Info"));
        deleteButton = page.getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Deactivate"));
        searchInput = page.getByRole(AriaRole.TEXTBOX, new Page.GetByRoleOptions().setName("Search"));
    }

    /**
     * This method navigates to the main page of the CMDB application.
     */
    public void navigate() {
        page.navigate("http://localhost:44313/");
    }

    /**
     * Generc search method to search for a string in the CMDB application.
     */
    public void search(String searchString) {
        searchInput.fill(searchString);
        searchInput.press("Enter");
    }

	/**
	 * Gets the Locator for the new button.
	 * @return Locator for the new button
	 */
    protected Locator newButton() {
		return newButton;
	}

	/**
	 * Gets the Locator for the edit button.
	 * @return Locator for the edit button
	 */
    protected Locator editButton() {
		return editButton;
	}

	/**
	 * Gets the Locator for the delete button.
	 * @return Locator for the delete button
	 */
    protected Locator deleteButton() {
		return deleteButton;
	}

	/**
	 * Gets the Locator for the details button.
	 * @return Locator for the details button
	 */
    protected Locator detailsButton() {
		return detailsButton;
	}

	/**
	 * Gets the Playwright Page object associated with this CMDBPage.
	 * @return Playwright Page object
	 */
    protected Page getPage() {
		return page;
	}
}
