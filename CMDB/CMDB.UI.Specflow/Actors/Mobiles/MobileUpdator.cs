using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Mobile;

namespace CMDB.UI.Specflow.Actors.Mobiles
{
    public class MobileUpdator : MobileActor
    {
        public MobileUpdator(ScenarioContext scenarioContext, string name = "MobileUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Mobile> CreateMobile(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheMobile());
            else
                return await Perform(new CreateTheInactiveMobile());
        }
        public async Task<Mobile> UpdateMobile(Mobile mobile, string field, string value)
        {
            rndNr = rnd.Next();
            switch (field)
            {
                case "IMEI":
                    var page = Perform(new OpenTheMobileEditPage());
                    page.WebDriver = Driver;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_editPage");
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, mobile.IMEI.ToString(),value + rndNr.ToString(),admin.Account.UserID,Table);
                    page.IMEI = value + rndNr.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IMEI");
                    mobile.IMEI = long.Parse(value + rndNr);
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_edited");
                    break;
                case "Type":
                    string Vendor, Type;
                    Vendor = value.Split(" ")[0];
                    Type = value.Split(" ")[1];
                    var assetType = await GetOrCreateAssetType("Mobile", Vendor, Type);
                    page = Perform(new OpenTheMobileEditPage());
                    page.WebDriver = Driver;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_editPage");
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, $"{mobile.MobileType}", value, admin.Account.UserID, Table);
                    page.Type = assetType.TypeID.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_type");
                    mobile.MobileType = assetType;
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_edited");
                    break;
                default:
                    log.Fatal($"The update for the field {field} is not implemented");
                    throw new NotImplementedException($"The update for the field {field} is not implemented");
            }
            return mobile;
        }
        public void DeactivateMoble(Mobile mobile, string reason)
        {
            var page = Perform(new OpenTheMobileDeactivatePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deactivatePage");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deleted");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Mobile with type {mobile.MobileType}", admin.Account.UserID, reason, Table);
        }
        public void ActivateMobile(Mobile mobile)
        {
            var page = GetAbility<MobileOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Mobile with type {mobile.MobileType}", admin.Account.UserID, Table);
        }
    }
}
