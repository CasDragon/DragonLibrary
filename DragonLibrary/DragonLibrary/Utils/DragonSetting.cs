using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kingmaker.Localization;
using ModMenu.Settings;
using UnityModManagerNet;

namespace DragonLibrary.Utils
{
    public enum SettingCategories
    {
        None,
        ModCompatability,
        Various,
        NewItems,
        NewArchetypes,
        NewBackgrounds,
        NewClasses,
        NewSpells,
        NewAbilities,
        NewFeatures
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DragonSetting : Attribute
    {
        private readonly SettingCategories category;
        private readonly string name;
        private readonly string description;
        private readonly bool enabledByDefault;

        public DragonSetting(SettingCategories category, string name, string description, bool enabledByDefault = true)
        {
            this.category = category;
            this.name = name;
            this.description = description;
            this.enabledByDefault = enabledByDefault;
        }
        public SettingCategories SettingCategory
        { get { return this.category; } }
        public string SettingName
        { get { return this.name; } }
        public string SettingDescription
        { get { return this.description; } }
        public bool EnabledByDefault
        { get { return this.enabledByDefault; } }
    }
    public class SettingsAction
    {
        private static string RootKey = "";

        public static void InitializeSettings(string rootKey, string modName, UnityModManager.ModEntry entry)
        {
            RootKey = rootKey;
            SettingsBuilder builder = SettingsBuilder.New(RootKey, CreateString(GetKey($"{modName}-title"), modName))
                    .SetMod(Main.entry);
            var settings = entry.Assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(m => m.GetCustomAttribute<DragonSetting>() is not null)
                .SelectMany(a => a.GetCustomAttributes())
                .OfType<DragonSetting>()
                .OrderBy(c => c.SettingCategory)
                .ToList();
            SettingCategories currentCategory = SettingCategories.None;
            string categoryName;
            foreach (var setting in settings)
            {
                if (setting.SettingCategory != currentCategory)
                {
                    currentCategory = setting.SettingCategory;
                    categoryName = currentCategory.ToString();
                    builder.AddAnotherSettingsGroup(GetKey(categoryName), CreateString(GetKey(categoryName), GetTitle(categoryName)));
                }
                builder.AddToggle(
                    Toggle.New(GetKey(setting.SettingName), setting.EnabledByDefault, CreateString(GetKey(setting.SettingName), setting.SettingDescription)));
            }
            ModMenu.ModMenu.AddSettings(builder);
        }
        public static T GetSetting<T>(string key)
        {
            try
            {
                return ModMenu.ModMenu.GetSettingValue<T>(GetKey(key));
            }
            catch (Exception ex)
            {
                Main.Log.Error(ex.ToString());
                return default(T);
            }
        }
        private static LocalizedString CreateString(string partialkey, string text)
        {
            return Helpers.CreateString(GetKey(partialkey), text);
        }
        private static string GetKey(string partialKey)
        {
            return $"{RootKey}.{partialKey}";
        }
        public static string GetTitle(string name)
        {
            return string.Join(" ", Regex.Split(name, @"(?=[A-Z](?![A-Z]|$))"));
        }
    }
    public static class Helpers
    {
        private static Dictionary<string, LocalizedString> textToLocalizedString = [];
        public static LocalizedString CreateString(string key, string value)
        {
            // See if we used the text previously.
            // (It's common for many features to use the same localized text.
            // In that case, we reuse the old entry instead of making a new one.)
            if (textToLocalizedString.TryGetValue(value, out LocalizedString localized))
            {
                return localized;
            }
            var strings = LocalizationManager.CurrentPack?.m_Strings;
            if (strings!.TryGetValue(key, out var oldValue) && value != oldValue.Text)
            {
                Main.Log.Log($"Info: duplicate localized string `{key}`, different text.");
            }
            var sE = new LocalizationPack.StringEntry
            {
                Text = value
            };
            strings[key] = sE;
            localized = new LocalizedString
            {
                m_Key = key
            };
            textToLocalizedString[value] = localized;
            return localized;
        }
    }
}
