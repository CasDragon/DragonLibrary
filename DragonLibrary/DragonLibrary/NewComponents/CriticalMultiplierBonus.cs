using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using UnityEngine;

namespace DragonLibrary.NewComponents
{
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [TypeId("F83FDDD5-8E68-4473-B72D-8E75BD59FE66")]
    [Serializable]
    public class CriticalMultiplierBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        [SerializeField]
        public int bonus = 1;
        [SerializeField]
        public ModifierDescriptor stackType = ModifierDescriptor.UntypedStackable;
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            evt.AdditionalCriticalMultiplier.Add(new Modifier(bonus, base.Fact, stackType));
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }
}
