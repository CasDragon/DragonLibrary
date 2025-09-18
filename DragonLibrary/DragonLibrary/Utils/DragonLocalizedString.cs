using System.Reflection;
using Newtonsoft.Json;

namespace DragonLibrary.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class DragonLocalizedString : Attribute
    {
        private readonly string key;
        private readonly string ENvalue;
        private readonly string CHvalue;
        private readonly bool template;
        public DragonLocalizedString(string key, string ENvalue, bool template = false, string CNvalue = "")
        {
            this.key = key;
            this.ENvalue = ENvalue;
            this.template = template;
            this.CHvalue = CNvalue;
        }
        public string Key
        {
            get { return this.key; }
        }
        public string English
        {
            get { return this.ENvalue; }
        }
        public string Chinese
        {
            get { return this.CHvalue; }
        }
        public bool Template
        {
            get { return this.template; }
        }
    }


    internal class LocalizedStringHelper
    {
        [DragonLocalizedString(disabledcontentstring, "Disabled Content")]
        internal const string disabledcontentstring = "dragonlibrary.disabled";

        public static void CreateLocalizationFile(string path)
        {
            Main.Log.Log("Creating localization file! DEBUG");
            var fields = Assembly.GetExecutingAssembly().GetTypes()
                .SelectMany(t => t.GetFields(BindingFlags.NonPublic | BindingFlags.Static))
                .Where(t => t.GetCustomAttribute<DragonLocalizedString>() is not null);
            List<LocString> locales = [];
            foreach (var field in fields)
            {
                var str = field.GetCustomAttribute<DragonLocalizedString>();
                locales.Add(new LocString()
                {
                    Key = str.Key,
                    ProcessTemplates = str.Template,
                    enGB = str.English,
                    zhCN = str.Chinese
                });
            }
            string json = JsonConvert.SerializeObject(locales.ToArray(), Formatting.Indented);
            File.WriteAllText(path: Path.Combine(path, "LocalizedStrings.json"), json);
        }

        public static string GetModFolderPath()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        }

        internal class LocString
        {
            public string Key;
            public bool ProcessTemplates;
            public string enGB;
            public string zhCN;

        }
    }
}
