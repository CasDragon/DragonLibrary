using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;

namespace DragonLibrary.NewComponents
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("D842B0E7-57D2-452D-91BC-94BDEF63447F")]
    [Serializable]
    internal class FeatMythicScaling : UnitFactComponentDelegate<FeatMythicScaling.ComponentData>
    {
        public ModifierDescriptor Descriptor;
        public StatType Stat;
        public int Value;
        public BlueprintFeatureReference Feature;
        public BlueprintFeatureReference MythicFeature;
        public BlueprintParametrizedFeatureReference ParametrizedFeature;
        public BlueprintParametrizedFeatureReference MythicParametrizedFeature;
        public string ScalingType = "Full";

        public override void OnTurnOn()
        {
            if (!Owner.HasFact(Feature) && !Owner.HasFact(ParametrizedFeature))
            {
                return;
            }
            ModifiableValue stat = Owner.Stats.GetStat(this.Stat);
            if (stat == null)
            {
                return;
            }
            int num = Value * Fact.GetRank();
            int mythicvalue = Owner.Progression.MythicLevel;
            int finalvalue = 1 + (num * mythicvalue);
            if (ScalingType == "Half" && (!Owner.HasFact(MythicFeature) && !Owner.HasFact(MythicParametrizedFeature)))
                finalvalue = ((int)(finalvalue * 0.5));
            stat.AddModifierUnique(finalvalue, Runtime, Descriptor);
        }

        public override void OnTurnOff()
        {
            Owner.Stats.GetStat(Stat).RemoveModifiersFrom(Runtime);
        }
        public class ComponentData
        {
        }
    }
}
