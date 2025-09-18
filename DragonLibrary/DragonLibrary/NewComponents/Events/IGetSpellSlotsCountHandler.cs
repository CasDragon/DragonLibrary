using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace DragonLibrary.NewComponents.Events
{
    public interface IGetSpellSlotsCountHandler : IUnitSubscriber
    {
        // Vek did this, thanks Vek
        void HandleGetSlotsCount(Spellbook spellbook, int spellLevel, ref int __result);

        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.GetSpellSlotsCount))]
        static class Spellbook_GetCommandType_IGetSpellSlotsCountHandler_Patch
        {
            static void Postfix(ref int __result, Spellbook __instance, int spellLevel)
            {
                var result = __result;
                EventBus.RaiseEvent<IGetSpellSlotsCountHandler>(__instance.Owner, h => h.HandleGetSlotsCount(__instance, spellLevel, ref result));
                __result = result;
            }
        }
    }
}
