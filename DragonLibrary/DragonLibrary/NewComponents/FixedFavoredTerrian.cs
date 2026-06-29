using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.View;

namespace DragonLibrary.NewComponents;

[AllowedOn(typeof(BlueprintUnitFact))]
[AllowMultipleComponents]
[TypeId("C14AEBA7-AC8A-40E8-BBC3-248636337522")]
public class FixedFavoredTerrain : UnitFactComponentDelegate, ITeleportHandler, IGlobalSubscriber, ISubscriber, IAreaLoadingStagesHandler
{
	private bool CurrentAreaPartIsFavoredTerrain => AreaService.Instance.CurrentAreaSetting == this.Setting;

	public override void OnTurnOn()
	{
		base.OnTurnOn();
		base.Owner.Ensure<UnitPartFavoredTerrain>().AddEntry(this.Setting, base.Fact);
		this.UpdateModifiers();
	}

	public override void OnTurnOff()
	{
		base.OnTurnOff();
		base.Owner.Ensure<UnitPartFavoredTerrain>().RemoveEntry(base.Fact);
		this.DeactivateModifier();
	}

	private void UpdateModifiers()
	{
		if (this.CurrentAreaPartIsFavoredTerrain)
		{
			this.ActivateModifier();
			return;
		}
		this.DeactivateModifier();
	}

	private void ActivateModifier()
	{
		int num = 2 * base.Fact.GetRank();
		base.Owner.Stats.Initiative.AddModifierUnique(num, base.Runtime, ModifierDescriptor.None);
		base.Owner.Stats.SkillPerception.AddModifierUnique(num, base.Runtime, ModifierDescriptor.None);
		base.Owner.Stats.SkillStealth.AddModifierUnique(num, base.Runtime, ModifierDescriptor.None);
		base.Owner.Stats.SkillLoreNature.AddModifierUnique(num, base.Runtime, ModifierDescriptor.None);
	}

	private void DeactivateModifier()
	{
		base.Owner.Stats.Initiative.RemoveModifiersFrom(base.Runtime);
		base.Owner.Stats.SkillPerception.RemoveModifiersFrom(base.Runtime);
		base.Owner.Stats.SkillStealth.RemoveModifiersFrom(base.Runtime);
		base.Owner.Stats.SkillLoreNature.RemoveModifiersFrom(base.Runtime);
	}

	public void HandlePartyTeleport(AreaEnterPoint enterPoint)
	{
		this.UpdateModifiers();
	}

	public AreaSetting Setting;
	
	public void OnAreaScenesLoaded()
	{
		
	}

	public void OnAreaLoadingComplete()
	{
		this.UpdateModifiers();
	}
}