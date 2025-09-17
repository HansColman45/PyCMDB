package be.hans.cmdb;

import be.brightest.ScreenPlay.Actors.IActor;
import be.hans.cmdb.Actors.CMDBActor;
import org.junit.jupiter.api.AfterEach;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

/**
 * Base class for Playwright tests, providing setup and teardown methods.
 */
class BaseTest {
    private final Random random = new Random();
    private final List<CMDBActor> actorList = new ArrayList<CMDBActor>();

    protected void registerActor(CMDBActor actor) {
        actorList.add(actor);
    }

    @AfterEach
    void closeContext() {
        for (CMDBActor actor : actorList) {
            actor.deleteAllObjectsCreatedByAdmin(actor.getSession());
        }
    }
    /**
     * This will return a random integer.
     * @return String representation of a random integer
     */
    protected String getRandomInt() {
        int rnd = random.nextInt(1000);
        return String.valueOf(rnd);
    }

    protected String defaultPassword() {
        return "1234";
    }
}
