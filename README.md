Useage:

`[DragonConfigure]`
* Sets up this method to be called by `DragonConfigureAction.DoPatches();`
* Optional arg, for setting priority on it. Defaults to Normal if not passed in
* [First, High, Normal, Low, Last]

_______________________________________________
--- Settings ---
_______________________________________________

`[DragonSetting(settingCategory, settingName, settingDescription)]`
* Used on a method
* Setups a new Modmenu toggle setting
* settingCategory - Uses the `SettingCategories` enum, no default
* settingName - string, just a key, not shown to user, must be unique
* settingDescription - string, English text to be shown to the user

`SettingsAction.GetSetting<bool>(settingName)`
* Used inside a method
* Gets the setting value
* settingName - string, unique key that was set above

`SettingsAction.InitializeSettings(rootKey, displayName)`
* Use this in your BlueprintCache.Init patch
* Sets up your Modmenu enviroment
* rootKey - string, a unique key, I recommend just using your modID
* displayName - string, English text to be shown to the user

_______________________________________________
--- String Helpers ---
_______________________________________________

`[DragonLocalizedString(key, text, CHtext)]`
* Used on a variable
* Creates new localized string in your LocalizedStrings.json
* key - string, a unique key
* text - string, English text
* CHtext - (optional) string, Chinese text
Example useage
```csharp
internal const string featurename = "Student Of Nethys";
internal const string featuredescription = "By studying the teachings of Nethys, your spells have an increased difficulty class, with a bonus of 2.";
[DragonLocalizedString(featurenamekey, featurename)]
internal const string featurenamekey = $"{feature}.name";
[DragonLocalizedString(featuredescriptionkey, featuredescription)]
internal const string featuredescriptionkey = $"{feature}.description";
```

`LocalizedStringHelper.disabledcontentstring`
* A helper string, just shows the English text "Content Disabled". Use for when you have settings, mostly.

`LocalizedStringHelper.CreateLocalizationFile(modfolder)`
* Use this in your BlueprintCache.Init patch
* Sets up your LocalizedStrings.json and builds it from scratch
* modfolder - string, your mods folder path (use `LocalizedStringHelper.GetModFolderPath()`)

_______________________________________________
--- Mod Compatibility Helper ---
_______________________________________________

`ModCompat.<modname>`
* These fields are set on run, basically mods I personally check for. [See the list here](https://github.com/CasDragon/DragonLibrary/blob/master/DragonLibrary/DragonLibrary/Utils/ModCompat.cs#L11)

`IsModEnabled(string modName, string modtype = "umm")`
* Used when it's for a mod that I don't have set above
* Returns true if the mod is found, false if it isn't
* modname - string, modID for the mod
* modtype = string, what type of mod it is


_______________________________________________
--- Custom components ---
_______________________________________________

* TODO later

Gost, you'll want to look at [this component](https://github.com/CasDragon/DragonLibrary/blob/master/DragonLibrary/DragonLibrary/NewComponents/BonusToBuffDC.cs) 
Can use it on your feature by `.AddBonusToBuffDC(buff, statType, descriptor)`
