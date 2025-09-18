using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using DragonLibrary.NewComponents;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace DragonLibrary.BPCoreExtensions
{
    public static class ConditionsBuilderNewExEx
    {
        public static ConditionsBuilder HasSpecificWeapon(
            this ConditionsBuilder builder,
            SimpleBlueprint weapon,
            bool? isShield = null,
            bool? bothHandsAreEmpty = null,
            WeaponCategory? category = null,
            bool? checkOnCaster = null,
            bool? checkSlot = null,
            bool? checkWeaponCategory = null,
            bool? checkWeaponRangeType = null,
            bool? justCheckEmptyHand = null,
            bool negate = false,
            WeaponRangeType? rangeType = null,
            ContextConditionIsWeaponEquipped.CheckedSlot? slot = null)
        {
            var element = ElementTool.Create<ConditionSpecificWeapon>();
            element.weapon = weapon;
            element.isShield = isShield ?? element.isShield;
            element.BothHandsAreEmpty = bothHandsAreEmpty ?? element.BothHandsAreEmpty;
            element.Category = category ?? element.Category;
            element.CheckOnCaster = checkOnCaster ?? element.CheckOnCaster;
            element.CheckSlot = checkSlot ?? element.CheckSlot;
            element.CheckWeaponCategory = checkWeaponCategory ?? element.CheckWeaponCategory;
            element.CheckWeaponRangeType = checkWeaponRangeType ?? element.CheckWeaponRangeType;
            element.JustCheckEmptyHand = justCheckEmptyHand ?? element.JustCheckEmptyHand;
            element.Not = negate;
            element.RangeType = rangeType ?? element.RangeType;
            element.Slot = slot ?? element.Slot;
            return builder.Add(element);
        }
    }
}
