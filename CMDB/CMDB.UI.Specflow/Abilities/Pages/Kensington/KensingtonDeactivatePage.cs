using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    public class KensingtonDeactivatePage : MainPage
    {
        public string Reason
        {
            set => EnterInTextboxByXPath("//input[@id='reason']", value);
        }
        public void Delete()
        {
            ClickElementByXpath("//button[.='Delete']");
        }
    }
}
