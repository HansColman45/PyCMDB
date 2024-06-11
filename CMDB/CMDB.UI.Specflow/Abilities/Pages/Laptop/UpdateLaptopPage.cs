using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Laptop
{
    public class UpdateLaptopPage : MainPage
    {
        public UpdateLaptopPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string AssetTag
        {
            set => EnterInTextboxByXPath("//input[@id='AssetTag']", value);
            get => TekstFromTextBox("//input[@id='AssetTag']");
        }
        public string SerialNumber
        {
            set => EnterInTextboxByXPath("//input[@id='SerialNumber']", value);
            get => TekstFromTextBox("//input[@id='SerialNumber']");
        }
        public string Type
        {
            set => SelectValueInDropDownByXpath("//select[@id='Type_TypeID']", "7");
            get => GetSelectedValueFromDropDownByXpath("//select[@id='Type_TypeID']");
        }
        public string RAM
        {
            set => SelectTektInDropDownByXpath("//select[@id='RAM']", value);
            get => GetSelectedValueFromDropDownByXpath("//select[@id='RAM']");
        }
        public string MAC
        {
            set => EnterInTextboxByXPath("//input[@id='MAC']", value);
            get => TekstFromTextBox("//input[@id='MAC']");
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
