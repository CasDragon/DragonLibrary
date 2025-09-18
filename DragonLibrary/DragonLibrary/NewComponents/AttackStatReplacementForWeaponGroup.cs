using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace DragonLibrary.NewComponents
{
    /// <summary>
    /// Component that provdes attack stat replacement for set fighter weapon group flags
    /// </summary>
    [ComponentName("Replace attack stat for fighter weapon group")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("C218F7AF-7CBC-448F-BB23-FAD602A3465D")]
    [Serializable]
    public class AttackStatReplacementForWeaponGroup :
        UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        ISubscriber,
        IInitiatorRulebookSubscriber
    {
        [SerializeField]
        public StatType ReplacementStat;
        [SerializeField]
        public WeaponFighterGroupFlags FighterGroupFlag;


        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            bool isReplacementStatHigher = base.Owner.Stats.GetStat(this.ReplacementStat) is ModifiableValueAttributeStat modifiableValueAttributeStat2 && base.Owner.Stats.GetStat(evt.AttackBonusStat) is ModifiableValueAttributeStat modifiableValueAttributeStat && modifiableValueAttributeStat2.Bonus >= modifiableValueAttributeStat.Bonus;
            if (!isReplacementStatHigher)
            {
                return;
            }
            if ((evt.Weapon.Blueprint.FighterGroup & FighterGroupFlag) != 0)
            {
                evt.AttackBonusStat = this.ReplacementStat;
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }
}
