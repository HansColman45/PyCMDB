package be.brightest.ScreenPlay.Abilities;

import com.microsoft.playwright.Page;

/**
 * Factory class to create instances of classes that extend OpenAWebSite.
 */
public class WebPageFactory {

    /**
     * Create an instance of a class that extends OpenAWebSite using the provided Page object.
     * The class must have a constructor that takes a Page as a parameter.
     *
     * @param clazz the class to instantiate
     * @param page  the Page object to pass to the constructor
     * @param <T>   the type of the class that extends OpenAWebSite
     * @return an instance of the class
     * @throws RuntimeException if the class cannot be instantiated
     */
    public static <T extends OpenAWebSite> T create(Class<T> clazz, Page page) {
        try {
            return clazz.getConstructor(Page.class).newInstance(page);
        } catch (Exception e) {
            throw new RuntimeException("Unable to create instance of " + clazz.getName(), e);
        }
    }
}
