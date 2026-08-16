using System.Reflection;
using UnityModManagerNet;

namespace DragonLibrary.Utils
{
    public enum ConfigurePriority
    {
        First,
        High,
        Normal,
        Low,
        Last
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DragonConfigure(ConfigurePriority priority = ConfigurePriority.Normal) : Attribute
    {
        private ConfigurePriority priority = priority;
        public ConfigurePriority PatchPriority => priority;
    }

    public class DragonConfigureAction
    {
        public static void DoPatches(UnityModManager.ModEntry entry)
        {
            var methods = entry.Assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(m => m.IsStatic && m.GetCustomAttribute<DragonConfigure>() is not null);
            var methodInfos = methods.ToList();
            foreach (ConfigurePriority priority in Enum.GetValues(typeof(ConfigurePriority)))
            {
                var methodnums = methodInfos.Where(m => m.GetCustomAttribute<DragonConfigure>().PatchPriority == priority);
                foreach (var method in methodnums)
                {
                    try
                    {
                        method.Invoke(null, []);
                    }
                    catch (TargetInvocationException e)
                    {
                        Main.Log.Log("Error invoking method - " + method.Name);
                        Main.Log.Log(e.ToString());
                    }
                }
            }
        }

    }
}
