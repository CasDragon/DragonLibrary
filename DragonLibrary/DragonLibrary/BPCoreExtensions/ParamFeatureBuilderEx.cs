using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Facts;
using DragonLibrary.NewComponents;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonLibrary.BPCoreExtensions
{
    public static class ParamFeatureBuilderEx
    {
        public static ParametrizedFeatureConfigurator AddFeatMythicScaling(
            this ParametrizedFeatureConfigurator configurator,
            int? Value = 1,
            StatType? Stat = StatType.Unknown,
            BlueprintFeatureReference Feature = null,
            BlueprintParametrizedFeatureReference ParametrizedFeature = null,
            BlueprintFeatureReference MythicFeature = null,
            BlueprintParametrizedFeatureReference MythicParametrizedFeature = null,
            string ScalingType = "Full",
            ModifierDescriptor? Descriptor = ModifierDescriptor.UntypedStackable)
        {
            FeatMythicScaling element = new()
            {
                Value = Value ?? 1,
                Stat = Stat ?? StatType.Charisma,
                Feature = Feature ?? null,
                ParametrizedFeature = ParametrizedFeature ?? null,
                MythicFeature = MythicFeature ?? null,
                MythicParametrizedFeature = MythicParametrizedFeature ?? null,
                ScalingType = ScalingType ?? "Full",
                Descriptor = Descriptor ?? ModifierDescriptor.UntypedStackable
            };
            return configurator.AddComponent(element);
        }
    }
}
