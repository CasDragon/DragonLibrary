using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Modding;
using UnityModManagerNet;
using WrathScalingItemDCs.ScalingDC;

namespace DragonLibrary.Utils
{
    public class ModCompat
    {
        public static bool microscopic = false;
        public static bool expandedcontent = false;
        public static bool homebrewarchetypes = false;
        public static bool tttbase = false;
        public static bool cop = false;
        public static bool pp = false;
        public static bool randomequipment = false;
        public static bool scalingequip = false;
        public static bool tttcore = false;
        public static bool wooloo = false;
        public static bool dragonchanges = false;

        public static void CheckForMods()
        {
            microscopic = IsModEnabled("MicroscopicContentExpansion");
            expandedcontent = IsModEnabled("ExpandedContent");
            homebrewarchetypes = IsModEnabled("HomebrewArchetypes", "owlcat");
            tttbase = IsModEnabled("TabletopTweaks-Base");
            cop = IsModEnabled("CharacterOptionsPlus");
            pp = IsModEnabled("PrestigePlus");
            randomequipment = IsModEnabled("RandomEquipment");
            scalingequip = IsModEnabled("WrathScalingItemDCs");
            tttcore = IsModEnabled("TabletopTweaks-Core.dll") || IsModEnabled("TabletopTweaks-Core");
            wooloo = IsModEnabled("WoolooMod", "owlcat");
            dragonchanges = IsModEnabled("DragonChanges");
        }
        public static bool IsModEnabled(string modName, string modtype = "umm")
        {
            Main.Log.Log($"Checking for {modName}");
            bool found = modtype switch
            {
                "umm" => UnityModManager.modEntries.Any(mod =>
                    mod.Info.Id.Equals(modName) && mod.Enabled && !mod.ErrorOnLoading),
                "owlcat" => OwlcatModificationsManager.Instance.AppliedModifications.Any(x =>
                    x.Manifest.UniqueName == modName),
                _ => false
            };
            LogModState(found, modName);
            return found;
        }
        public static void LogModState(bool mod, string modname)
        {
            Main.Log.Log(mod
                ? $"{modname} is found and enabled"
                : $"{modname} wasn't found, disabling compatibility patches");
        }
        public static void AddEquipmentToScalingDC(BlueprintItemEquipment equipment)
        {
            if (!scalingequip)
                return;
            ScalingDCAPI.AddItem(equipment);
        }
    }
}
