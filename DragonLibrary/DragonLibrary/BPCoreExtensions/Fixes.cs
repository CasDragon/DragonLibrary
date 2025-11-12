using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace DragonLibrary.BPCoreExtensions
{
    public static class Fixes
    {
        public static ActionsBuilder ApplyBuffWithDurationSecondsFixed(
            this ActionsBuilder builder,
            Blueprint<BlueprintBuffReference> buff,
            float durationSeconds,
            bool? asChild = null,
            bool? ignoreParentContext = null,
            bool? isFromSpell = null,
            bool? isNotDispelable = null,
            bool? notLinkToAreaEffect = null,
            bool? sameDuration = null,
            bool? toCaster = null,
            bool? isExtendable = false)
        {
            var element = ElementTool.Create<ContextActionApplyBuff>();
            element.m_Buff = buff?.Reference;
            element.DurationSeconds = durationSeconds;
            element.AsChild = asChild ?? element.AsChild;
            element.IgnoreParentContext = ignoreParentContext ?? element.IgnoreParentContext;
            element.IsFromSpell = isFromSpell ?? element.IsFromSpell;
            element.IsNotDispelable = isNotDispelable ?? element.IsNotDispelable;
            element.NotLinkToAreaEffect = notLinkToAreaEffect ?? element.NotLinkToAreaEffect;
            element.SameDuration = sameDuration ?? element.SameDuration;
            element.ToCaster = toCaster ?? element.ToCaster;
            element.Permanent = false;
            element.UseDurationSeconds = true;
            element.DurationValue = new ContextDurationValue() 
            { 
                m_IsExtendable = isExtendable ?? true,
                DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                Rate = DurationRate.Rounds,
                DiceCountValue = 0,
                BonusValue = 0
            };
            return builder.Add(element);
        }
        public static ActionsBuilder ApplyBuffPermanentFixed(
        this ActionsBuilder builder,
        Blueprint<BlueprintBuffReference> buff,
        bool? asChild = null,
        bool? ignoreParentContext = null,
        bool? isFromSpell = null,
        bool? isNotDispelable = null,
        bool? notLinkToAreaEffect = null,
        bool? sameDuration = null,
        bool? toCaster = null)
        {
            var element = ElementTool.Create<ContextActionApplyBuff>();
            element.m_Buff = buff?.Reference;
            element.AsChild = asChild ?? element.AsChild;
            element.IgnoreParentContext = ignoreParentContext ?? element.IgnoreParentContext;
            element.IsFromSpell = isFromSpell ?? element.IsFromSpell;
            element.IsNotDispelable = isNotDispelable ?? element.IsNotDispelable;
            element.NotLinkToAreaEffect = notLinkToAreaEffect ?? element.NotLinkToAreaEffect;
            element.SameDuration = sameDuration ?? element.SameDuration;
            element.ToCaster = toCaster ?? element.ToCaster;
            element.Permanent = true;
            element.UseDurationSeconds = false;
            element.DurationValue = new ContextDurationValue() { m_IsExtendable = false };
            return builder.Add(element);
        }
    }
}
