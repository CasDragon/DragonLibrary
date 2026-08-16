using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.FactLogic;

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
		DeactivateModifier();
		UpdateModifiers();
	}
}