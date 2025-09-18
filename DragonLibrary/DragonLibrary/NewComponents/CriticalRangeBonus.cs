using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using UnityEngine;

namespace DragonLibrary.NewComponents
{
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [TypeId("7F59444B-8623-45EA-8456-5B768E6587C1")]
    [Serializable]
    public class CriticalRangeBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        [SerializeField]
        public int bonus = 1;
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            evt.CriticalEdgeBonus += bonus;
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }
}
