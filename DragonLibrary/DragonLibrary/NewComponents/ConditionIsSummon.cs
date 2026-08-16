using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using UnityEngine;

namespace DragonLibrary.NewComponents;

[TypeId("680E4821-99FB-43CD-A9EF-7981DF8B1613")]
[Serializable]
public class ConditionIsSummon: ContextCondition
{
    [SerializeField] 
    public new bool Not;


    public override string GetConditionCaption()
    {
        return "Is summon";
    }

    public override bool CheckCondition()
    {        
        UnitEntityData unit = Target.Unit;
        if (unit == null)
        {
            return false;
        }

        bool summ = unit.Buffs.HasFact(Game.Instance.BlueprintRoot.SystemMechanics.SummonedUnitBuff);
        if (Not)
            summ = !summ;
        return summ;
    }
}