using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using UnityEngine;

namespace DragonLibrary.NewComponents
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("2f80d83c-70b7-402d-8dec-6aee57df9043")]
    internal class BonusToBuffDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public BlueprintBuff Buff
        {
            get
            {
                BlueprintBuffReference buff = this.m_Buff;
                if (buff == null)
                {
                    return null;
                }
                return buff.Get();
            }
        }
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (evt.Spell == this.Buff)
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
        private BlueprintBuffReference m_Buff;
        [SerializeField]
        public StatType Stat;
        [SerializeField]
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}
