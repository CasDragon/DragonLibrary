using System.Reflection;
using Kingmaker.Blueprints.JsonSystem;
using Newtonsoft.Json;
using UnityModManagerNet;

namespace DragonLibrary.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DragonLocalizedString(string key, string eNvalue, bool template = false, string cNvalue = "")
        : Attribute
    {
        public string Key => key;

        public string English => eNvalue;

        public string Chinese => cNvalue;

        public bool Template => template;
    }


    public class LocalizedStringHelper
    {
        [DragonLocalizedString(disabledcontentstring, "Disabled Content")]
        public const string disabledcontentstring = "dragonlibrary.disabled";

        public static void CreateLocalizationFile(string path, UnityModManager.ModEntry entry)
        {
            var fields = entry.Assembly.GetTypes()
                .SelectMany(t => t.GetFields(BindingFlags.NonPublic | BindingFlags.Static))
                .Where(t => t.GetCustomAttribute<DragonLocalizedString>() is not null);
            List<LocString> locales = [];
            if (File.Exists(Path.Combine(path, "LocalizedStrings.json")))
            {
                var x = File.ReadAllText(Path.Combine(path, "LocalizedStrings.json"));
                locales = JsonConvert.DeserializeObject<List<LocString>>(x);
                /*Main.Log.Log("printing keys");
                foreach (var str in locales)
                {
                    Main.Log.Log($"{str.Key} - {str.enGB}");
                }*/
            }
            foreach (var field in fields)
            {
                var str = field.GetCustomAttribute<DragonLocalizedString>();
                if (locales.All(l => l.Key != str.Key))
                {
                    locales.Add(new LocString()
                    {
                        Key = str.Key,
                        ProcessTemplates = str.Template,
                        enGB = str.English,
                        zhCN = str.Chinese
                    });
                }
            }
            string json = JsonConvert.SerializeObject(locales.ToArray(), Formatting.Indented);
            File.WriteAllText(path: Path.Combine(path, "LocalizedStrings.json"), json);
        }

        public static string GetModFolderPath(UnityModManager.ModEntry entry)
        {
            return new FileInfo(entry.Assembly.Location).Directory!.FullName;
        }

        private class LocString
        {
            public string Key;
            public bool ProcessTemplates;
            public string enGB;
            public string zhCN;

        }
    }
}
