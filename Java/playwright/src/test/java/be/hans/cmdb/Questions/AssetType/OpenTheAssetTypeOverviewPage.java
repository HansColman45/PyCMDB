package be.hans.cmdb.Questions.AssetType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * Question to open the Asset Type Overview page.
 */
public class OpenTheAssetTypeOverviewPage extends Question<AssetTypeOverviewPage> {
    /**
     * Opens the Asset Type Overview page.
     * @param actor the actor performing the action
     * @return AssetTypeOverviewPage
     */
    @Override
    public AssetTypeOverviewPage performAs(IActor actor) {
        MainPage ability = (MainPage) actor.getAbility(MainPage.class);
        ability.getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Type")).click();
        ability.getPage().locator("xpath=//a[@id='Asset Type28']").click();
        ability.getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        ability.newButton().waitFor();
        return WebPageFactory.create(AssetTypeOverviewPage.class, ability.getPage());
    }
}
