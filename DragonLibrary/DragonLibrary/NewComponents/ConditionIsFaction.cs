using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using UnityEngine;

namespace DragonLibrary.NewComponents;

[TypeId("B3B6BE0F-FD36-450D-A0B4-2D97EAAC46B1")]
[Serializable]
public class ConditionIsFaction: ContextCondition
{
    public override string GetConditionCaption()
    {
        return "Is Faction";
    }

    public override bool CheckCondition()
    {
        UnitEntityData unit = Target.Unit;
        if (unit == null)
        {
            return false;
        }
        return unit.Faction == Faction;
    }

    [SerializeField]
    public BlueprintFaction Faction;
}