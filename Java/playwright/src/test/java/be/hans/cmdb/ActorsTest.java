package be.hans.cmdb;

import be.hans.cmdb.Questions.IsTheUserLoggedOn;
import be.cmdb.pages.Identity.IdentityOverviewPage;
import be.cmdb.pages.MainPage;
import be.hans.cmdb.Actors.CMDBActor;
import org.junit.jupiter.api.Test;

public class ActorsTest {

    @Test
    void canOpenIdentityPage(){
        CMDBActor hans = new CMDBActor("Hans");
        MainPage main = hans.doLogin("Root","796724Md");
        boolean loggedIn = hans.asksFor(new IsTheUserLoggedOn());
        assert loggedIn;
        IdentityOverviewPage overviewPage = hans.openIdentityOverviewPage();
    }
}
