using BlueprintCore.Blueprints.Configurators.Facts;
using DragonLibrary.NewComponents;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace DragonLibrary.BPCoreExtensions
{
    public static class FeatureBuilderEx
    {
        public static TBuilder AddBonusToAbilityDC<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            BlueprintAbility ability,
            StatType stat,
            ModifierDescriptor descriptor)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            BonusToAbilityDC element = new()
            {
                Ability = ability,
                Stat = stat,
                Descriptor = descriptor
            };
            return configurator.AddComponent(element);
        }
        public static TBuilder AddBonusToBuffDC<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            BlueprintBuff buff,
            StatType stat,
            ModifierDescriptor descriptor)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            BonusToBuffDC element = new()
            {
                Buff = buff,
                Stat = stat,
                Descriptor = descriptor
            };
            return configurator.AddComponent(element);
        }
        public static TBuilder AddCriticalRangeBonus<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            int? bonus)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            CriticalRangeBonus element = new()
            {
                bonus = bonus ?? 1,
            };
            return configurator.AddComponent(element);
        }
        public static TBuilder AddSneakAttackRollTrigger<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            ActionList actionList)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            AddSneakAttackRollTrigger element = new()
            {
                Action = actionList
            };
            return configurator.AddComponent(element);
        }
        public static TBuilder AddCriticalMultiplierBonus<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            int? bonus,
            ModifierDescriptor bonusDescriptor = ModifierDescriptor.UntypedStackable)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            CriticalMultiplierBonus element = new()
            {
                bonus = bonus ?? 1,
                stackType = bonusDescriptor
            };
            return configurator.AddComponent(element);
        }
        public static TBuilder AddArcanistSpellSlotsComponent<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            int? bonusSlots,
            int[] Levels)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            AddArcanistSpellSlots element = new()
            {
                Amount = bonusSlots ?? 1,
                Levels = Levels ?? [1]
            };
            return configurator.AddComponent(element);
        }
        public static TBuilder AddSpellToBookComponent<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            BlueprintAbilityReference spell,
            int? spellLevel = null)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            SpellToBook element = new()
            {
                spelllevel = spellLevel ?? 1,
                spell = spell
            };
            return configurator.AddComponent(element);
        }
        public static TBuilder AddWorkingAttackStatReplacementForWeaponGroup<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            StatType? ReplacementStat = StatType.Charisma,
            WeaponFighterGroupFlags? FighterGroupFlag = WeaponFighterGroupFlags.Natural)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            var element = new AttackStatReplacementForWeaponGroup();
            element.ReplacementStat = ReplacementStat ?? element.ReplacementStat;
            element.FighterGroupFlag = FighterGroupFlag ?? element.FighterGroupFlag;
            return configurator.AddComponent(element);
        }
        public static TBuilder AddDRComponent<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            bool? stackable = true,
            ContextValue value = null,
            ContextValue pool = null,
            bool? usePool = false,
            bool? or = false,
            bool? bypassByMaterial = false,
            PhysicalDamageMaterial? material = PhysicalDamageMaterial.Adamantite,
            bool? bypassedByForm = false,
            PhysicalDamageForm? form = PhysicalDamageForm.Bludgeoning,
            bool? bypassedByMagic = false,
            int? minEnhancementBonus = 1,
            bool? bypassedByAlignement = false,
            DamageAlignment? alignment = DamageAlignment.Good,
            bool? bypassedByReality = false,
            DamageRealityType? reality = DamageRealityType.Ghost,
            bool? bypassedByWeaponType = false,
            BlueprintWeaponTypeReference m_weaponType = null,
            bool? bypassedByMeleeWeapon = false,
            bool? bypassedByEpic = false,
            BlueprintUnitFactReference m_CheckedFactMythic = null,
            AddDamageResistancePhysical.WeaponFactFilter weaponFactFilter = AddDamageResistancePhysical.WeaponFactFilter.Any,
            AttackTypeFlag ValidWeaponAttackTypes = (AttackTypeFlag)(-1))
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            var element = new AddDamageResistancePhysical();
            element.Value = value ?? element.Value;
            element.UsePool = usePool ?? element.UsePool;
            element.Pool = pool ?? element.Pool;
            element.m_IsStackable = stackable ?? element.m_IsStackable;
            element.Or = or ?? element.Or;
            element.BypassedByMaterial = bypassByMaterial ?? element.BypassedByMaterial;
            element.Material = material ?? element.Material;
            element.BypassedByForm = bypassedByForm ?? element.BypassedByForm;
            element.Form = form ?? element.Form;
            element.BypassedByMagic = bypassedByMagic ?? element.BypassedByMagic;
            element.MinEnhancementBonus = minEnhancementBonus ?? element.MinEnhancementBonus;
            element.BypassedByAlignment = bypassedByAlignement ?? element.BypassedByAlignment;
            element.Alignment = alignment ?? element.Alignment;
            element.BypassedByReality = bypassedByReality ?? element.BypassedByReality;
            element.Reality = reality ?? element.Reality;
            element.BypassedByWeaponType = bypassedByWeaponType ?? element.BypassedByWeaponType;
            element.m_WeaponType = m_weaponType ?? element.m_WeaponType;
            element.BypassedByMeleeWeapon = bypassedByMeleeWeapon ?? element.BypassedByMeleeWeapon;
            element.BypassedByEpic = bypassedByEpic ?? element.BypassedByEpic;
            element.m_CheckedFactMythic = m_CheckedFactMythic ?? element.m_CheckedFactMythic;
            element.ValidWeaponFact = weaponFactFilter;
            element.ValidWeaponAttackTypes = ValidWeaponAttackTypes;
            return configurator.AddComponent(element);
        }
    }
}
