using Kingmaker.Blueprints;
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
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("b0b250f5-7957-482c-b46a-5188179eea28")]
    public class BonusToAbilityDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public BlueprintAbility Ability
        {
            get
            {
                BlueprintAbilityReference ability = this.m_Ability;
                if (ability == null)
                {
                    return null;
                }
                return ability.Get();
            }
            set
            {
                m_Ability = value.ToReference<BlueprintAbilityReference>(); ;
            }
        }
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (evt.Spell == this.Ability)
            {
                var context = evt.Reason.Context;
                int bonus = context.MaybeCaster?.Stats.GetStat(Stat)?.ModifiedValue ?? 0;
                if (bonus > 0)
                    evt.AddBonusDC(bonus, Descriptor);
            }
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
