using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.Testing.Helpers;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to delete all items created or updated by the admin
    /// </summary>
    public class DeleteAllItemsCreatedOrUpdatedByAdmin : Question<Task<List<object>>>
    {
        private NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public override async Task<List<object>> PerformAs(IPerformer actor)
        {
            List<object> objects = new();
            var context = actor.GetAbility<DataContext>();
            var admin = context.Admin;
            try
            {
                var tuchedobjects = await AdminHelper.DeleteCascading(context.context, admin);
                foreach (var item in tuchedobjects)
                {
                    objects.Add(item.Value);
                }
                return objects;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}
