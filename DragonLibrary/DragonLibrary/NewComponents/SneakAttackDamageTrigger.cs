using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace DragonLibrary.NewComponents
{
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [TypeId("0d5b6fdf-0ee1-43f9-a1f2-a1361a8c08ca")]
    [Serializable]
    public class AddSneakAttackRollTrigger : EntityFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public ActionList Action;

        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {

        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsSneakAttack && evt.IsHit)
            {
                base.Fact.RunActionInContext(Action, evt.Target);
            }
        }
    }
}
