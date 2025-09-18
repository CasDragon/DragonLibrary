using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityModManagerNet;

namespace DragonLibrary
{
    public static class Main
    {
        internal static Harmony HarmonyInstance;
        internal static UnityModManager.ModEntry.ModLogger Log;
        internal static UnityModManager.ModEntry entry;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Log = modEntry.Logger;
            modEntry.OnGUI = OnGUI;
            entry = modEntry;
            HarmonyInstance = new Harmony(modEntry.Info.Id);
            try
            {
                HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch
            {
                HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
                throw;
            }
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {

        }
    }
}
