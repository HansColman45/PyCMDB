package be.hans.cmdb.Questions;

import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.LoginPage;

public class IsTheUserLoggedOn extends Question<Boolean> {

    @Override
    public Boolean performAs(IActor actor) {
        LoginPage main = (LoginPage) actor.getAbility(LoginPage.class);
        return main.isUserLogedIn();
    }
}
