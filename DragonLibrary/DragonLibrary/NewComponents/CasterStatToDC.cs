using DragonLibrary.BaseGameExtensions;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using UnityEngine;

namespace DragonLibrary.NewComponents;

[AllowMultipleComponents]
[AllowedOn(typeof(BlueprintFeature), false)]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowedOn(typeof(BlueprintUnit), false)]
[TypeId("3F44074E-6DB3-4714-B1B7-DC140682F451")]
[Serializable]
[ComponentName("Increase all spells DC by Caster Stat")]
public class CasterStatToDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
{
    public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
    {
        if (evt.Spell is not { IsSpell: true })
            return;
        StatType castingStat = evt.Spellbook!.Blueprint.CastingAttribute;
        int bonus = evt.Reason.Caster?.Stats.GetStat<ModifiableValueAttributeStat>(castingStat)?.Bonus ?? 0;
        if (bonus > 0)
            evt.AddBonusDC(bonus, evt.Reason.Fact, Descriptor);
    }

    public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
    {
    }

    [SerializeField]
    public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
}