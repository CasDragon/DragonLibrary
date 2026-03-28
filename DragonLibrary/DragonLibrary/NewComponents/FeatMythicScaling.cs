using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (!base.Owner.HasFact(Feature) && !base.Owner.HasFact(ParametrizedFeature))
            {
                return;
            }
            ModifiableValue stat = base.Owner.Stats.GetStat(this.Stat);
            if (stat == null)
            {
                return;
            }
            int num = this.Value * base.Fact.GetRank();
            int mythicvalue = base.Owner.Progression.MythicLevel;
            int finalvalue = 1 + (num * mythicvalue);
            if (ScalingType == "Half" && (!base.Owner.HasFact(MythicFeature) && !base.Owner.HasFact(MythicParametrizedFeature)))
                finalvalue = ((int)(finalvalue * 0.5));
            stat.AddModifierUnique(finalvalue, base.Runtime, this.Descriptor);
        }

        public override void OnTurnOff()
        {
            base.Owner.Stats.GetStat(this.Stat).RemoveModifiersFrom(base.Runtime);
        }
        public class ComponentData
        {
        }
    }
}
