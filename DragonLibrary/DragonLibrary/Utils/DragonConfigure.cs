using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
    public class DragonConfigure : Attribute
    {
        private ConfigurePriority priority = ConfigurePriority.Normal;
        public DragonConfigure(ConfigurePriority priority = ConfigurePriority.Normal)
        {
            this.priority = priority;
        }
        public ConfigurePriority PatchPriority
        {
            get { return this.priority; }
        }
    }

    public class DragonConfigureAction
    {
        public static void DoPatches(UnityModManager.ModEntry entry)
        {
            var methods = entry.Assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(m => m.IsStatic && m.GetCustomAttribute<DragonConfigure>() is not null);
            foreach (ConfigurePriority priority in Enum.GetValues(typeof(ConfigurePriority)))
            {
                var methodnums = methods.Where(m => m.GetCustomAttribute<DragonConfigure>().PatchPriority == priority);
                foreach (var method in methodnums)
                    method.Invoke(null, []);
            }
        }

    }
}
