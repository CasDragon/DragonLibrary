using BlueprintCore.Blueprints.Configurators.UnitLogic.Abilities;
using DragonLibrary.NewComponents;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace DragonLibrary.BPCoreExtensions
{
    public static class AbilityBuilderEx
    {
        public static TBuilder AddChargingThrow<T1, TBuilder>(
            this BaseAbilityConfigurator<T1, TBuilder> configurator)
            where T1 : BlueprintAbility
            where TBuilder : BaseAbilityConfigurator <T1, TBuilder>
        {
            return configurator.AddComponent(new ChargingThrow());
        }
    }
}
