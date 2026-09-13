using BlueprintCore.Blueprints.CustomConfigurators;
using DragonLibrary.Utils;
using Kingmaker.Blueprints;

namespace DragonLibrary.BPCoreExtensions;

public static class RootBuilderEx
{
    // Detaches the component from its current blueprint, points it at the destination, and
    // adds it back through the configurator .

    public static TBuilder MoveComponent<T, TBuilder>(
        this RootConfigurator<T, TBuilder> configurator,
        BlueprintScriptableObject from,
        T to,
        BlueprintComponent component
    )
        where T : BlueprintScriptableObject
        where TBuilder : RootConfigurator<T, TBuilder>
    {
        DragonHelpers.RemoveComponent(from, component);
        component.OwnerBlueprint = to;
        return configurator.AddComponent(component);
    }
}