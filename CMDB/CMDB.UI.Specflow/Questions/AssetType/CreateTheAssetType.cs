using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;
using System;

namespace CMDB.UI.Specflow.Questions.AssetType
{
    /// <summary>
    /// This class is used to create an asset type
    /// </summary>
    public class CreateTheAssetType : Question<Task<Domain.Entities.AssetType>>
    {
        public override async Task<Domain.Entities.AssetType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            AssetCategory category = context.DBcontext.AssetCategories.FirstOrDefault();
            return await AssetTypeHelper.CreateSimpleAssetType(context.DBcontext, category, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive asset type
    /// </summary>
    public class CreateTheInactiveAssetType : Question<Task<Domain.Entities.AssetType>>
    {
        public override async Task<Domain.Entities.AssetType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            AssetCategory category = context.DBcontext.AssetCategories.FirstOrDefault();
            return await AssetTypeHelper.CreateSimpleAssetType(context.DBcontext, category, context.Admin,false);
        }
    }
}
