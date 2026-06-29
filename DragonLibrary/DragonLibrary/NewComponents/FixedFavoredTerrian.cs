using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.View;

namespace DragonLibrary.NewComponents;

[AllowedOn(typeof(BlueprintUnitFact))]
[AllowMultipleComponents]
[TypeId("C14AEBA7-AC8A-40E8-BBC3-248636337522")]
public class FixedFavoredTerrain : FavoredTerrain, IAreaLoadingStagesHandler
{
	public void OnAreaScenesLoaded()
	{
		
	}

	public void OnAreaLoadingComplete()
	{
		this.DeactivateModifier();
		this.UpdateModifiers();
	}
}