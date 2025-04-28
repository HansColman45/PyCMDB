using Bright.ScreenPlay.Abilities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class WebPageFactory
    {
        /// <summary>
        /// A generic create method for creating a page object.
        /// </summary>
        /// <typeparam name="T">The type T of the page.  All pages inherit from the WebPage class which makes that each of the pages can be
        /// created as the expected type T.</typeparam>
        /// <param name="browser">The browser which holds the page that needs to be created.</param>
        /// <returns>An instance of the webpage of type T is returned.</returns>
        public static T Create<T>(IWebDriver driver) where T : OpenAWebPage
        {
            return (T)Activator.CreateInstance(typeof(T), [driver]);
        }
    }
}
