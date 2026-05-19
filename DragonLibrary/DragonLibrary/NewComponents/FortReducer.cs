using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Utility;
using Owlcat.Runtime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonLibrary.NewComponents
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("FDEC26D9-A25C-446C-BE4C-4E714B2E91DD")]
    [Serializable]
    public class FortReducer: BlueprintComponent
    {
        public int Value = 25;

    }
    [HarmonyPatch]
    public class FortCheckPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.FortificationOvercomed), MethodType.Getter)]
        public static void PatchFortCheck(RuleAttackRoll __instance)
        {
            if (__instance.FortificationChance == 0)
                return;
            IEnumerable<EntityFact> fact = __instance.Initiator.Facts.List.Where(f => f.Components.OfType<FortReducer>().Any());
            foreach (EntityFact x in fact)
            {
                if (fact == null)
                    continue;
                FortReducer component = x.GetComponent<FortReducer>();
                int y = component.Value;
                int z = x.GetRank();
                int yolo = __instance.FortificationChance - (y * z);
                __instance.FortificationChance = yolo >= 0 ? yolo : 0;
                if (__instance.FortificationChance == 0)
                    break;
            }
        }
    }
}
