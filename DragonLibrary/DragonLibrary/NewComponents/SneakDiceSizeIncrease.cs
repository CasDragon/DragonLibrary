using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonLibrary.NewComponents
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("42544507-9771-461C-925C-558BEFA61C36")]
    [Serializable]
    public class SneakDiceSizeIncrease : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber
    {
        // Stolen shamelessly from Vek, shoutout to Vek for allowing me to be lazy
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
            evt?.ParentRule?.m_DamageBundle?
                .Where(damage => damage.Sneak)
                .ForEach(damage => {
                    var rolls = damage.Dice.ModifiedValue.Rolls;
                    var originalDice = damage.Dice.ModifiedValue.Dice;

                    DiceFormula? formula = originalDice switch
                    {
                        DiceType.D3 => new DiceFormula(rolls, DiceType.D4),
                        DiceType.D4 => new DiceFormula(rolls, DiceType.D6),
                        DiceType.D6 => new DiceFormula(rolls, DiceType.D8),
                        DiceType.D8 => new DiceFormula(rolls, DiceType.D10),
                        DiceType.D10 => new DiceFormula(rolls, DiceType.D12),
                        DiceType.D12 => new DiceFormula(rolls, DiceType.D20),
                        DiceType.D20 => new DiceFormula(rolls, DiceType.D100),
                        _ => null
                    };

                    if (formula is not null)
                    {
                        damage.Dice.Modify(formula.Value, base.Fact);
                    }
                });

        }
    }
}
