using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Actors;
using CMDB.UI.Specflow.Questions.DataContextAnswers;

namespace CMDB.UI.Specflow.StepDefinitions
{
    public class ActorRegistry
    {
        private readonly List<CMDBActor> _actors = new();

        public void RegisterActor(CMDBActor actor)
        {
            _actors.Add(actor);
        }
        public void Clear()
        {
            _actors.Clear();
        }
        public async Task DisposeActors()
        {
            foreach (var actor in _actors)
            {
                await actor.Perform(new DeleteAllItemsCreatedOrUpdatedByAdmin());
                actor.Dispose();
            }
        }
        public List<CMDBActor> Actors => _actors;
    }
}
