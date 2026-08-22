using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace DragonLibrary.NewComponents;

    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("5C1F023B-8822-4187-9358-65718D5C460D")]
    [Serializable]
    public class StatBonusToSpellDamage: UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            MechanicsContext context = evt.Reason.Context;
            if (context?.SourceAbility is not { IsSpell: true })
            {
                return;
            }
            foreach (var damage in evt.DamageBundle)
            {
                int bonus = Value.Calculate(context);
                damage.AddModifier(bonus, base.Fact);
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }
        
        public ContextValue Value;
    }
