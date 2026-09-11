using DragonLibrary.Utils;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace DragonLibrary.NewComponents;

[TypeId("598A78A6-A1ED-44A9-878A-6987F60657ED")]
public class ConditionIsWeaponCategoryEquipped : ContextCondition
{
    public bool CheckOnCaster = true; 
    public WeaponGroupCategory Category;
    public bool CheckMainHand = true;
    
    public override string GetConditionCaption()
    {
        return "Check if target has weapon category group equipped";
    }

    public override bool CheckCondition()
    {
        UnitEntityData unitEntityData = (CheckOnCaster ? Context.MaybeCaster : Target.Unit);
        if (unitEntityData == null) return false;
        ItemEntityWeapon weapon = null;
        if (CheckMainHand && unitEntityData.Body.PrimaryHand.HasItem)
            weapon = unitEntityData.Body.PrimaryHand.MaybeWeapon;
        if (!CheckMainHand && unitEntityData.Body.SecondaryHand.HasItem)
            weapon = unitEntityData.Body.SecondaryHand.MaybeWeapon;
        if (weapon == null) return false;
        return Category switch
        {
            WeaponGroupCategory.Blunt => WeaponGroupCategoryHelpers.isBlunt(weapon),
            WeaponGroupCategory.Sharp => WeaponGroupCategoryHelpers.isSharp(weapon),
            _ => true
        };
    }
}