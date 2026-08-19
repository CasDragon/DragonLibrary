using System.Diagnostics;
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

                return File.Exists(candidate) ? Assembly.LoadFrom(candidate) : null;
            };
            
            Log = modEntry.Logger;
            entry = modEntry;
            UpdateHarmony();
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
        // Harmony Update Stuff
        private static Version ParseFileVersion(string path) => Version.Parse(FileVersionInfo.GetVersionInfo(path).FileVersion);

        private static void UpdateHarmony()
        {
            var baseGamePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(entry.Path)!)!)!;
            var managedDir = Path.Combine(baseGamePath, "Wrath_Data", "Managed");
            var harmonyPath = Path.Combine(managedDir, "0Harmony.dll");
            var winhttpPath = Path.Combine(baseGamePath, "winhttp.dll");

            if (File.Exists(winhttpPath))
            {
                Log.Log("Found winhttp.dll, is not Assembly install, not updating Harmony");
                return;
            }

            if (!File.Exists(harmonyPath))
            {
                Log.Log("Harmony.dll not found, not updating Harmony");
                return;
            }

            var includedHarmonyPath = Path.Combine(entry.Path, "0Harmony.dll");

            var harmonyAss = AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(ass => ass.GetName().Name == "0Harmony");

            Log.Log(harmonyAss is not null
                ? $"Harmony version {harmonyAss.GetName().Version} is loaded already"
                : "Harmony is not loaded (yet)");

            var currentVersion = ParseFileVersion(harmonyPath);

            Log.Log($"Current Harmony version = {currentVersion}");

            var includedVersion = ParseFileVersion(includedHarmonyPath);

            Log.Log($"Bundled Harmony version = {includedVersion}");

            byte[] newHarmony = [];
            
            void doUpdate()
            {
                if (newHarmony.Length == 0 && includedVersion > currentVersion)
                    newHarmony = File.ReadAllBytes(includedHarmonyPath);

                if (newHarmony.Length == 0)
                    return;

                File.Move(harmonyPath, $"{harmonyPath}.{currentVersion}");
                File.WriteAllBytes(harmonyPath, newHarmony);
                var newHarmonyVersion = ParseFileVersion(harmonyPath);
                Log.Log($"Harmony version is now {newHarmonyVersion}");

                if (harmonyAss is null) return;
                Log.Log("Restart for changes to take effect");
            }
            
            doUpdate();
        }
        // End updating Harmony

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
