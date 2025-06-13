using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    /// <summary>
    /// This class is used to create a laptop
    /// </summary>
    public class CreateTheLaptop : Question<Task<Domain.Entities.Laptop>>
    {
        public override async Task<Domain.Entities.Laptop> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var laptop =  await LaptopHelper.CreateSimpleLaptop(context.context, context.Admin);     
            return (Domain.Entities.Laptop) context.GetDevice(laptop.AssetTag);
        }
    }
    /// <summary>
    /// This class is used to create an inactive laptop
    /// </summary>
    public class CreateTheInactiveLaptop : Question<Task<Domain.Entities.Laptop>>
    {
        public override async Task<Domain.Entities.Laptop> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var laptop = await LaptopHelper.CreateSimpleLaptop(context.context, context.Admin, false);
            return (Domain.Entities.Laptop)context.GetDevice(laptop.AssetTag);
        }
    }
}
