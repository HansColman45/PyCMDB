package be.cmdb.pages;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;


public class CMDBPage {
    protected Locator newButton;
    protected Locator editButton;
    protected Locator deleteButton;
    protected Locator detailsButton;
    protected Locator searchInput;

    protected final Page page;

    public CMDBPage(Page page) {
        this.page = page;
        newButton = page.getByRole(AriaRole.LINK,
            new Page.GetByRoleOptions().setName("New")
        );
        editButton = page.getByRole(AriaRole.LINK,
            new Page.GetByRoleOptions().setName("Edit")
        );
        detailsButton = page.getByRole(AriaRole.LINK,
            new Page.GetByRoleOptions().setName("Info")
        );
        deleteButton = page.getByRole(AriaRole.LINK,
            new Page.GetByRoleOptions().setName("Deactivate")
        );
    }

    public void navigate() {
        page.navigate("http://localhost:44313/");
    } 

}
