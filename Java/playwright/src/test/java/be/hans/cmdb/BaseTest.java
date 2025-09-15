package be.hans.cmdb;

import java.util.Random;

/**
 * Base class for Playwright tests, providing setup and teardown methods.
 */
class BaseTest {
    private final Random random = new Random();

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
