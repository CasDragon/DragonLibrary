using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using UnityEngine;

namespace DragonLibrary.NewComponents
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("b0b250f5-7957-482c-b46a-5188179eea28")]
    [ComponentName("Bonus to Ability DC")]
    public class BonusToAbilityDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public BlueprintAbility Ability
        {
            get
            {
                BlueprintAbilityReference ability = m_Ability;
                return ability?.Get();
            }
            set => m_Ability = value.ToReference<BlueprintAbilityReference>();
        }
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (evt.Spell != Ability) return;
            int bonus = evt.Reason.Caster?.Stats.GetStat<ModifiableValueAttributeStat>(Stat)?.Bonus ?? 0;
            if (bonus > 0)
                evt.AddBonusDC(bonus, Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }

        [SerializeField]
        private BlueprintAbilityReference m_Ability;
        [SerializeField]
        public StatType Stat;
        [SerializeField]
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}
