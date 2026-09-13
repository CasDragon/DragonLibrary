using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using UnityEngine;

namespace DragonLibrary.NewComponents;

[ComponentName("Increase spell CL while Conduit Surge is active")]
[AllowedOn(typeof(BlueprintUnitFact))]
[TypeId("d3d8b6f4b5f9481f8f5e5b6e5a6d0c11")]
public class IncreaseCasterLevelForSpells : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>
{
    [SerializeField]
    public ContextValue Value;
    [SerializeField]
    public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;

    public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
    {
        MechanicsContext context = evt.Reason.Context;
        if (context?.SourceAbility is not { IsSpell: true })
        {
            return;
        }

        evt.AddBonusCasterLevel(Value.Calculate(Context), Descriptor);
    }

    public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
    {
    }
}