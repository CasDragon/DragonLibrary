using System.Reflection;
using System.Text;
using DragonLibrary.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using UnityModManagerNet;

namespace DragonLibrary
{
    public static class Main
    {
        private static Harmony HarmonyInstance;
        internal static UnityModManager.ModEntry.ModLogger Log;
        internal static UnityModManager.ModEntry entry;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {        
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var simpleName = new AssemblyName(args.Name).Name;
                var candidate = Path.Combine(LocalizedStringHelper.GetModFolderPath(modEntry), simpleName + ".dll");

                if (File.Exists(candidate))
                    return Assembly.LoadFrom(candidate);

                return null;
            };
            
            Log = modEntry.Logger;
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

        [HarmonyPatch(typeof(BlueprintsCache))]
        public static class BlueprintsCaches_Patch
        {
            private static bool Initialized = false;

            [HarmonyPriority(Priority.Last)]
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            public static void Init_Postfix()
            {
                try
                {
                    if (Initialized)
                    {
                        Log.Log("Already initialized blueprints cache.");
                        return;
                    }
                    Initialized = true;
                    Log.Log("Checking for mods for compatibility patches");
                    ModCompat.CheckForMods();
                }
                catch (Exception e)
                {
                    Log.Log(string.Concat("Failed to initialize.", e));
                }
            }
        }
    }
}
