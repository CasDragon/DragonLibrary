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
            AddDamageResistancePhysical component = TTTHelpers.CreateCopy(BuffRefs.ArmorFocusHeavyMythicFeatureVar1SubBuff.Reference.Get().GetComponent<AddDamageResistancePhysical>());
            component.Value = value ?? component.Value;
            return configurator.AddComponent(component);
        }
    }
}
