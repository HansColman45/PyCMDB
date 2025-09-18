package be.hans.cmdb;

import be.brightest.ScreenPlay.Abilities.IAbility;
import be.brightest.ScreenPlay.Abilities.OpenAWebSite;
import be.hans.cmdb.Actors.CMDBActor;
import com.microsoft.playwright.Page;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.extension.ExtensionContext;
import org.junit.jupiter.api.extension.RegisterExtension;
import org.junit.jupiter.api.extension.TestWatcher;

import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.ZoneOffset;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.time.format.DateTimeFormatter;

/**
 * Base class for Playwright tests, providing setup and teardown methods.
 */
class BaseTest {
    private final Random random = new Random();
    private final List<CMDBActor> actorList = new ArrayList<CMDBActor>();

    protected void registerActor(CMDBActor actor) {
        actorList.add(actor);
    }

    @RegisterExtension
    public TestWatcher watcher = new TestWatcher() {
        @Override
        public void testFailed(ExtensionContext context, Throwable cause) {
            for (CMDBActor actor : actorList) {
                for (IAbility ability : actor.getAbilities()) {
                    if (ability instanceof OpenAWebSite) {
                        Page page = ((OpenAWebSite) ability).getPage();
                        if(!page.isClosed()) {
                            String timestamp = DateTimeFormatter.ofPattern("yyyyMMdd_HHmmss")
                                .format(LocalDateTime.now(ZoneOffset.UTC));
                            page.screenshot(new Page.ScreenshotOptions()
                                .setPath(Paths.get("screenshots/" + context.getDisplayName() + "screenshot" + timestamp + ".png"))
                                .setFullPage(true));
                            page.close();
                        }
                    }
                }
            }
        }
    };

    @RegisterExtension
    public TestWatcher successWatcher = new TestWatcher() {
        @Override
        public void testSuccessful(ExtensionContext context) {
            for (CMDBActor actor : actorList) {
                for (IAbility ability : actor.getAbilities()) {
                    if (ability instanceof OpenAWebSite) {
                        ((OpenAWebSite) ability).close();
                    }
                }
            }
        }
    };

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
