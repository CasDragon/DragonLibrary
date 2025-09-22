using DragonLibrary.NewComponents.Events;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace DragonLibrary.NewComponents
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [TypeId("8D7DDA99-E1A0-451D-98C8-F6BA07415FC2")]
    [Serializable]
    public class AddArcanistSpellSlots : UnitFactComponentDelegate, IGetSpellSlotsCountHandler
    {
        /* For Badger
        TypeID is 8D7DDA99-E1A0-451D-98C8-F6BA07415FC2
        Amount is the amount of spell slots to add
        Levels is what spell book levels to add it for

        End result should look more or less identical to base game's AddSpellsPerDay component
        */

        [SerializeField]
        public int Amount;
        [SerializeField]
        public int[] Levels;

        public void HandleGetSlotsCount(Spellbook spellbook, int spellLevel, ref int __result)
        {
            if (spellbook.Blueprint.IsArcanist)
            {
                foreach (int num in this.Levels)
                {
                    if (spellLevel == num)
                    {
                        __result += Amount;
                    }
                }
            }
        }
    }
}
