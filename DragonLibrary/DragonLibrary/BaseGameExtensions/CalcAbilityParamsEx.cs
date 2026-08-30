using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;

namespace DragonLibrary.BaseGameExtensions;

public static class CalcAbilityParamsEx
{
    public static RuleCalculateAbilityParams AddBonusDC(
        this RuleCalculateAbilityParams ruleCalculateAbilityParams,
        int bonus,
        EntityFact fact,
        ModifierDescriptor descriptor)
    {
        var modifier = new Modifier(bonus, fact, descriptor);
        ruleCalculateAbilityParams.m_BonusDC ??= new ModifiableBonus();
        ruleCalculateAbilityParams.m_BonusDC.Add(modifier);
        return ruleCalculateAbilityParams;
    }
}