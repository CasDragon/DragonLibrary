using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;

namespace DragonLibrary.NewComponents
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("2D51354C-0457-44DE-8ADC-64B675471121")]
    [Serializable]
    public class BonusDoubler
    {
    }
}
