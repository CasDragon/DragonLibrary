using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.References;
using DragonLibrary.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace DragonLibrary.BPCoreExtensions
{
    public static class AddTTTComponents
    {
        public static TBuilder AddTTTAddDamageResistancePhysical<T1, TBuilder>(
            this BaseUnitFactConfigurator<T1, TBuilder> configurator,
            ContextValue value = null)
            where T1 : BlueprintUnitFact
            where TBuilder : BaseUnitFactConfigurator<T1, TBuilder>
        {
            AddDamageResistancePhysical comp = BuffRefs.ArmorFocusHeavyMythicFeatureVar1SubBuff.Reference.Get().GetComponent<AddDamageResistancePhysical>();
            comp ??= FeatureRefs.DamageReduction.Reference.Get().GetComponent<AddDamageResistancePhysical>();
            if (comp == null)
                return configurator.AddDamageResistancePhysical(value: value);
            AddDamageResistancePhysical component = TTTHelpers.CreateCopy(comp);
            component.Value = value ?? component.Value;
            return configurator.AddComponent(component);
        }
    }
}
