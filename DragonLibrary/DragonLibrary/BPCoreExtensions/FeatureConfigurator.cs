using Kingmaker.Blueprints.Facts;

namespace DragonLibrary.BPCoreExtensions
{
    public class FeatureConfigurator<T1, TBuilder>
        where T1 : BlueprintUnitFact
        where TBuilder : FeatureConfigurator<T1, TBuilder>
    {
    }
}