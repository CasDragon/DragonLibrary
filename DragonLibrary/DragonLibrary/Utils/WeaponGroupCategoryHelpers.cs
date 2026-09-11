using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;

namespace DragonLibrary.Utils;

public static class WeaponGroupCategoryHelpers
{
    public static bool isBlunt(BlueprintItemWeapon weapon)
    {
        if (weapon.DamageType.IsEnergy) return false;
        return weapon.DamageType.Physical.Form == PhysicalDamageForm.Bludgeoning;
    }
    public static bool isSharp(BlueprintItemWeapon weapon)
    {
        if (weapon.DamageType.IsEnergy) return false;
        return weapon.DamageType.Physical.Form != PhysicalDamageForm.Bludgeoning;
    }
    public static bool isBlunt(BlueprintItemWeaponReference weapon)
    {
        if (weapon.GetBlueprint() is not BlueprintItemWeapon x) return false;
        if (x.DamageType.IsEnergy) return false;
        return x.DamageType.Physical.Form == PhysicalDamageForm.Bludgeoning;
    }
    public static bool isSharp(BlueprintItemWeaponReference weapon)
    {
        if (weapon.GetBlueprint() is not BlueprintItemWeapon x) return false;
        if (x.DamageType.IsEnergy) return false;
        return x.DamageType.Physical.Form != PhysicalDamageForm.Bludgeoning;
    }
    public static bool isBlunt(ItemEntityWeapon weapon)
    {
        if (weapon.Blueprint is not { } x) return false;
        if (x.DamageType.IsEnergy) return false;
        return x.DamageType.Physical.Form == PhysicalDamageForm.Bludgeoning;
    }
    public static bool isSharp(ItemEntityWeapon weapon)
    {
        if (weapon.Blueprint is not { } x) return false;
        if (x.DamageType.IsEnergy) return false;
        return x.DamageType.Physical.Form != PhysicalDamageForm.Bludgeoning;
    }
}

public enum WeaponGroupCategory
{
    Blunt,
    Sharp,
    None
}